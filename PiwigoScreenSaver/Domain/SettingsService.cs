using Microsoft.Extensions.Caching.Memory;
using PiwigoScreenSaver.Models;
using System;

namespace PiwigoScreenSaver.Domain
{
    public class SettingsService : ISettingsService
    {
        private readonly IMemoryCache cache;
        private readonly ISettingsRepository repository;

        public SettingsService(IMemoryCache cache, ISettingsRepository repository)
        {
            this.cache = cache;
            this.repository = repository;
        }

        public void Save(SettingKey key, string value)
        {
            repository.SetValue(key.ToString(), value);
            cache.Set(key.ToString(), value);
        }

        public string Get(SettingKey key)
        {
            if (!cache.TryGetValue(key.ToString(), out string value))
            {
                if (repository.GetValue(key.ToString()) is string valueFromRepo)
                {
                    value = valueFromRepo;
                    cache.Set(key.ToString(), value);
                }
                else
                {
                    throw new Exception($"Missing setting key '{key.ToString()}'. Check settings again.");
                }
            }

            return value;
        }

        public bool ValidateSettings(string url, string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            if (!Uri.TryCreate(url, UriKind.Absolute, out Uri _))
            {
                return false;
            }

            return true;
        }
    }
}
