using Microsoft.Extensions.Caching.Memory;
using PiwigoScreenSaver.Models;
using System;

namespace PiwigoScreenSaver.Domain;

public class SettingsService : ISettingsService
{
    private readonly IMemoryCache _cache;
    private readonly ISettingsRepository _repository;

    public SettingsService(IMemoryCache cache, ISettingsRepository repository)
    {
        _cache = cache;
        _repository = repository;
    }

    public void Save(SettingKey key, string value)
    {
        _repository.SetValue(key.ToString(), value);
        _cache.Set(key.ToString(), value);
    }

    public string Get(SettingKey key)
    {
        if (!_cache.TryGetValue(key.ToString(), out string? value))
        {
            if (_repository.GetValue(key.ToString()) is string valueFromRepo)
            {
                value = valueFromRepo;
                _cache.Set(key.ToString(), value);
            }
            else
            {
                throw new FormatException($"Missing setting key '{key}'. Check settings again.");
            }
        }

        return value!;
    }

    public bool ValidateSettings(string url, string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            return false;
        }

        return Uri.TryCreate(url, UriKind.Absolute, out Uri? _);
    }
}
