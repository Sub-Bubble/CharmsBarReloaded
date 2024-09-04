using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CharmsBarReloaded.Config
{
    public class TranslationManager
    {
        private Dictionary<string, string> localizedStrings;
        private Dictionary<string, string> fallbackStrings;
        public int TotalKeys { get; } = 45;
        public TranslationManager Load(string cultureCode)
        {
            string localizedPath = $"lang/{cultureCode}.json";
            string fallbackLanguagePath = "lang/en-us.json";

            //no need to do fallback to the same language
            if (cultureCode == "en-us")
            {
                localizedStrings = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(localizedPath));
                return this;
            }
            else
            {
                localizedStrings = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(localizedPath));
                fallbackStrings = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(fallbackLanguagePath));
                return this;
            }

        }
        public string GetTranslation(string key)
        {
            if (localizedStrings.TryGetValue(key, out string value)) return value;
            if (fallbackStrings != null && fallbackStrings.TryGetValue(key, out string fallbackValue)) return fallbackValue;
            return key;
        }
        public int GetKeysAmount(string cultureCode)
        {
            string localizedPath = $"lang/{cultureCode}.json";
            if (!File.Exists(localizedPath))
            {
                return 0;
            }
            Dictionary<string, string> translations = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(localizedPath));
            return translations.Count;
        }
    }
}
