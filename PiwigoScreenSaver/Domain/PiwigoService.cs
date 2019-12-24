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

namespace PiwigoScreenSaver.Domain
{
    public class PiwigoService : IGalleryService
    {
        private string lastJsonResponse;

        private readonly ILogger logger;
        private readonly ISettingsService settingsService;
        private readonly HttpClient httpClient;
        private readonly JsonSerializerOptions jsonOptions;

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

        public PiwigoService(ILogger logger, ISettingsService settingsService, HttpClient httpClient)
        {
            this.logger = logger;
            this.settingsService = settingsService;
            this.httpClient = httpClient;
            this.settingsService = settingsService;

            jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            // All dates are in "yyyy-MM-dd HH:mm:ss" format.
            jsonOptions.Converters.Add(new DateTimeConverterUsingDateTimeParse());

            // Use an enum for the "stat" property. We want the json value
            // deserialized to the enum name, not the enum value.
            jsonOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
        }

        public async Task<Image> GetRandomImage(Size boundingSize)
        {
            Initialize();

            if (await NotAuthenticated())
            {
                await Authenticate();
            }

            var image = await MakeRequest<CategoryImages>("pwg.categories.getImages&per_page=1&order=random", HttpMethod.Get);

            if (image.Status != StatusCode.OK)
            {
                logger.LogError("Unexpected status encountered from pwg.categories.getImages");
                logger.LogDebug(lastJsonResponse);
                throw new Exception($"Error getting image: {image.ErrorMessage}");
            }
            else if (image.Result.Paging.Count != 1)
            {
                throw new Exception("No images returned");
            }

            var imageUrl = FindLargestImageWithinBounds(image.Result.Images.Single().Derivatives, boundingSize);

            using var stream = await httpClient.GetStreamAsync(imageUrl);
            return Image.FromStream(stream);
        }

        private void Initialize()
        {
            // Only want to perform this once.
            if (httpClient.BaseAddress != null)
            {
                return;
            }

            httpClient.BaseAddress = new Uri(settingsService.Get(SettingKey.Url));

            var version = GetType().Assembly.GetName().Version.ToString();
            httpClient.DefaultRequestHeaders.Add("User-Agent", $"Piwigo Screen Saver/{version}");
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

            throw new Exception($"Couldn't find image small enough to fit the screen ({boundingSize.Width},{boundingSize.Height})");
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
                { "username", settingsService.Get(SettingKey.Username) },
                { "password", settingsService.Get(SettingKey.Password) }
            };

            var loginResult = await MakeRequest<bool>("pwg.session.login", HttpMethod.Post, nvc);

            if (loginResult.Status != StatusCode.OK)
            {
                throw new Exception($"Error logging in: {loginResult.ErrorMessage}");
            }
        }

        private async Task<BaseResult<T>> MakeRequest<T>(string method, HttpMethod httpMethod,
            IDictionary<string, string> formValues = null)
        {
            using var request = new HttpRequestMessage(httpMethod, "ws.php?format=json&method=" + method);

            if (formValues != null)
            {
                request.Content = new FormUrlEncodedContent(formValues);
            }

            using var response = await httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();
            lastJsonResponse = await response.Content.ReadAsStringAsync();

            return MapJson<T>(lastJsonResponse);
        }

        internal BaseResult<T> MapJson<T>(string json)
        {
            return JsonSerializer.Deserialize<BaseResult<T>>(json, jsonOptions);
        }
    }
}
