using Microsoft.Extensions.Logging;
using PiwigoScreenSaver.Domain.JsonConverters;
using PiwigoScreenSaver.Models;
using PiwigoScreenSaver.Models.Piwigo;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PiwigoScreenSaver.Domain;

public class PiwigoService : IGalleryService
{
    private string? lastJsonResponse;

    private readonly ILogger<PiwigoService> _logger;
    private readonly ISettingsService _settingsService;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly JsonSerializerOptions _jsonOptions;

    private readonly string[] derivativeSizes = new string[]
    {
        "xxlarge",
        "xlarge",
        "large",
        "medium",
        "small",
        "xsmall",
        "2small",
        "thumb",
        "square"
    };

    public PiwigoService(ILogger<PiwigoService> logger, ISettingsService settingsService,
        IHttpClientFactory httpClientFactory)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
        _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        // All dates are in "yyyy-MM-dd HH:mm:ss" format.
        _jsonOptions.Converters.Add(new DateTimeConverterUsingDateTimeParse());

        // Use an enum for the "stat" property. We want the json value
        // deserialized to the enum name, not the enum value.
        _jsonOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
    }

    public async Task<Image> GetRandomImage(Size boundingSize)
    {
        if (await NotAuthenticated())
        {
            await Authenticate();
        }

        var image = await MakeRequest<CategoryImages>("pwg.categories.getImages&per_page=1&order=random", HttpMethod.Get);

        if (image.Status != StatusCode.OK)
        {
            _logger.LogError("Unexpected status encountered from pwg.categories.getImages");
            _logger.LogDebug(lastJsonResponse);
            throw new InvalidOperationException($"Error getting image: {image.ErrorMessage}");
        }
        else if (image.Result.Paging.Count != 1)
        {
            throw new InvalidOperationException("No images returned");
        }

        var imageUrl = FindLargestImageWithinBounds(image.Result.Images.Single().Derivatives, boundingSize);
        return await ImageStreamFromUrl(imageUrl, image.Result.Images.Single().Name);
    }

    /// <summary>
    /// Encapsulate attempt to fetch the photo since it's possible that it
    /// doesn't exist. This can happen when the photo is manually deleted
    /// from the file system without removing it from the database.
    /// Include the photo's name in the exception message that's shown to
    /// the user as a way to find the offending photo in the gallery.
    /// </summary>
    /// <param name="url">The URL to the photo to download.</param>
    /// <param name="imageName">The photo's name in the gallery.</param>
    /// <returns>The image.</returns>
    /// <exception cref="Exception">
    /// Thrown if the image at the given URL can't be retrieved.
    /// </exception>
    internal async Task<Image> ImageStreamFromUrl(string url, string imageName)
    {
        try
        {
            var httpClient = CreateClient();
            using var stream = await httpClient.GetStreamAsync(url);
            return Image.FromStream(stream);
        }
        catch (Exception e)
        {
            _logger.LogWarning("Couldn't download {imageName}: {errorMessage}", imageName, e.Message);
            throw new InvalidOperationException($"{e.Message} Photo name was '{imageName}'", e);
        }
    }

    private HttpClient CreateClient()
    {
        var httpClient = _httpClientFactory.CreateClient();

        httpClient.BaseAddress = new Uri(_settingsService.Get(SettingKey.Url));

        var version = GetType().Assembly.GetName().Version?.ToString();
        httpClient.DefaultRequestHeaders.Add("User-Agent", $"PiwigoScreenSaver/{version}");

        return httpClient;
    }

    internal string FindLargestImageWithinBounds(IDictionary<string, Derivative> derivatives, Size boundingSize)
    {
        foreach (var size in derivativeSizes)
        {
            if (derivatives[size].Width < boundingSize.Width &&
                derivatives[size].Height < boundingSize.Height)
            {
                return derivatives[size].Url;
            }
        }

        throw new InvalidOperationException($"Couldn't find image small enough to fit the screen ({boundingSize.Width},{boundingSize.Height})");
    }

    internal async Task<bool> NotAuthenticated()
    {
        var status = await MakeRequest<SessionStatus>("pwg.session.getStatus", HttpMethod.Get);
        return status.Result.Username == "guest";
    }

    internal async Task Authenticate()
    {
        var nvc = new Dictionary<string, string>
        {
            { "username", _settingsService.Get(SettingKey.Username) },
            { "password", _settingsService.Get(SettingKey.Password) }
        };

        var loginResult = await MakeRequest<bool>("pwg.session.login", HttpMethod.Post, nvc);

        if (loginResult.Status != StatusCode.OK)
        {
            throw new InvalidOperationException($"Error logging in: {loginResult.ErrorMessage}");
        }
    }

    private async Task<BaseResult<T>> MakeRequest<T>(string method, HttpMethod httpMethod,
        IDictionary<string, string>? formValues = null)
    {
        var uri = $"ws.php?format=json&method={method}";
        var httpClient = CreateClient();

        try
        {
            using var request = new HttpRequestMessage(httpMethod, uri);

            if (formValues != null)
            {
                request.Content = new FormUrlEncodedContent(formValues);
            }

            using var response = await httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();
            lastJsonResponse = await response.Content.ReadAsStringAsync();

            var result = MapJson<T>(lastJsonResponse);

            if (result == null)
            {
                throw new InvalidCastException($"Unexpected response couldn't be mapped: {lastJsonResponse}");
            }

            return result;
        }
        catch (Exception e)
        {
            _logger.LogError("Error making {method} request to {baseAddress}{uri}: {errorMessage}",
                httpMethod, httpClient.BaseAddress, uri, e.Message);
            throw;
        }
    }

    internal BaseResult<T>? MapJson<T>(string json) =>
        JsonSerializer.Deserialize<BaseResult<T>>(json, _jsonOptions);
}
