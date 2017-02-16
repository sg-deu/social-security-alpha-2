using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace FormUI.Tests
{
    public static class VstsSettings
    {
        private const string WebConfigFile      = @"..\..\..\FormUI\Web.config";
        private const string WebConfigCopyFile  = @"..\..\..\FormUI\Web.config.copy";

        private static Lazy<IDictionary<string, string>> _settings = new Lazy<IDictionary<string, string>>(ReadVstsSettings);

        public static string GetSetting(string name, string defaultValue)
        {
            var settings = _settings.Value;

            return settings.ContainsKey(name)
                ? settings[name]
                : defaultValue;
        }

        public static void UpdateWebConfig()
        {
            RestoreWebConfig();

            File.Move(WebConfigFile, WebConfigCopyFile);

            var webConfig = new XmlDocument();
            webConfig.Load(WebConfigCopyFile);
            var webConfigSettings = webConfig.SelectNodes("//appSettings/add");
            var settings = _settings.Value;

            foreach (XmlElement setting in webConfigSettings)
            {
                var key = setting.GetAttribute("key");

                if (!settings.ContainsKey(key))
                    continue;

                setting.SetAttribute("value", settings[key]);
            }

            webConfig.Save(WebConfigFile);
        }

        public static void RestoreWebConfig()
        {
            if (File.Exists(WebConfigCopyFile))
            {
                File.Delete(WebConfigFile);
                File.Move(WebConfigCopyFile, WebConfigFile);
            }
        }

        private static IDictionary<string, string> ReadVstsSettings()
        {
            var settings = new Dictionary<string, string>();

            var settingsFile = @"..\..\..\VstsSettings.txt";

            if (!File.Exists(settingsFile))
            {
                Console.WriteLine("No custom VSTS settings file found at: {0}", settingsFile);
                return settings;
            }

            var fileContents = File.ReadAllLines(settingsFile);
            var settingsLine = fileContents[0].Trim();
            if (settingsLine.StartsWith("(")) settingsLine = settingsLine.Substring(1);
            if (settingsLine.EndsWith(")")) settingsLine = settingsLine.Substring(0, settingsLine.Length - 1);
            var splitSettings = settingsLine.Split(new[] { ") (" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var setting in splitSettings)
            {
                var split = setting.IndexOf('=');
                var key = setting.Substring(0, split);
                var value = setting.Substring(split + 1, setting.Length - split - 1);
                Console.WriteLine("Custom setting {0}={1}", key, value);
                settings.Add(key, value);
            }

            return settings;
        }
    }
}
