using Avalonia.Media;
using Avalonia.Platform.Storage;
using ImmichFrame.Core.Exceptions;
using ImmichFrame.Helpers;
using ImmichFrame.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ImmichFrame.Models
{
    public partial class Settings
    {
       
        public string ImmichServerUrl { get; set; } = string.Empty;
        
        public static string JsonSettingsPath
        {
            get
            {
                string basePath;
                if (PlatformDetector.IsAndroid())
                {
                    basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                }
                else
                {
                    basePath = AppDomain.CurrentDomain.BaseDirectory;
                }
                return Path.Combine(basePath, "Settings.json");
            }
        }

        private static Settings? currentSettings;
        public static Settings CurrentSettings
        {
            get
            {
                if (currentSettings == null)
                {
                    if (!File.Exists(JsonSettingsPath))
                    {
                        return GetDefaultSettingsFile();
                    }
                    currentSettings = ParseFromJson();
                    currentSettings.Validate();
                }
                return currentSettings;
            }
        }

        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(this.ImmichServerUrl))
                throw new SettingsNotValidException($"Settings element '{nameof(ImmichServerUrl)}' is required!");
        }

        public static void ReloadFromJson()
        {
            currentSettings = ParseFromJson();
            currentSettings.Validate();
        }
        private static Settings ParseFromJson()
        {
            var json = File.ReadAllText(Settings.JsonSettingsPath);
            JsonDocument doc;
            try
            {
                doc = JsonDocument.Parse(json);
            }
            catch (Exception ex)
            {
                throw new SettingsNotValidException($"Problem with parsing the settings: {ex.Message}", ex);
            }

            var settingsValues = new Dictionary<string, object>();

            foreach (var property in doc.RootElement.EnumerateObject())
            {
                object value = property.Value;

                if (property.Value.ValueKind == JsonValueKind.Array)
                {
                    var list = new List<string>();
                    foreach (var element in property.Value.EnumerateArray())
                    {
                        list.Add(element.GetString() ?? string.Empty);
                    }
                    value = list;
                }
                else
                {
                    value = property.Value.ToString();
                }

                settingsValues.Add(property.Name, value);
            }

            return ParseSettings(settingsValues);
        }

        private static Settings ParseSettings(Dictionary<string, object> SettingsValues)
        {
            var settings = new Settings();
            var properties = settings.GetType().GetProperties();

            foreach (var SettingsValue in SettingsValues)
            {
                var property = properties.First(x => x.Name == SettingsValue.Key);

                var value = SettingsValue.Value;

                if (value == null)
                    throw new SettingsNotValidException($"Value of '{SettingsValue.Key}' is not valid.");

                switch (SettingsValue.Key)
                {
                    case "ImmichServerUrl":
                        var url = value.ToString()!.TrimEnd('/');
                        if (!Uri.TryCreate(url, UriKind.Absolute, out Uri? result) || result == null || (result.Scheme != Uri.UriSchemeHttp && result.Scheme != Uri.UriSchemeHttps))
                        {
                            throw new SettingsNotValidException($"Value of '{SettingsValue.Key}' is not a valid URL: '{url}'");
                        }
                        property.SetValue(settings, url);
                        break;                    
                    default:
                        throw new SettingsNotValidException($"Element '{SettingsValue.Key}' is unknown. ('{value}')");
                }
            }

            return settings;
        }

        public static void SaveSettings(Settings settings)
        {
            var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(Settings.JsonSettingsPath, json);
        }       
        private static Settings GetDefaultSettingsFile()
        {
            var defaultSettings = new Settings
            {
                ImmichServerUrl = ""                
            };
            return defaultSettings;
        }
    }
}
