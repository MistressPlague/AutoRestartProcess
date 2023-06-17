using System;
using System.IO;
using Newtonsoft.Json;

namespace Libraries
{
    public class ConfigLib<T> where T : class
    {
        private string ConfigCache;

        public ConfigLib(string configPath, int RefreshInterval = 1000)
        {
            ConfigPath = configPath;

            var PathToWatch = Path.GetDirectoryName(ConfigPath);

            if (PathToWatch != null)
            {
                var watcher = new FileSystemWatcher(PathToWatch, Path.GetFileName(ConfigPath))
                {
                    NotifyFilter = NotifyFilters.LastWrite,
                    EnableRaisingEvents = true
                };

                watcher.Changed += UpdateConfig;
            }

            if (!File.Exists(ConfigPath))
            {
                File.WriteAllText(ConfigPath, JsonConvert.SerializeObject(Activator.CreateInstance(typeof(T)), Formatting.Indented));
            }

            InternalConfig = JsonConvert.DeserializeObject<T>(File.ReadAllText(ConfigPath));

            var timer = new System.Timers.Timer(RefreshInterval);

            ConfigCache = JsonConvert.SerializeObject(InternalConfig);

            timer.Elapsed += (_, _) =>
            {
                if (JsonConvert.SerializeObject(InternalConfig, Formatting.Indented) != ConfigCache)
                {
                    ConfigCache = JsonConvert.SerializeObject(InternalConfig, Formatting.Indented);

                    SaveConfig();

                    //MessageBox.Show("Saved!");
                }
            };

            timer.Enabled = true;
            timer.Start();
        }

        private string ConfigPath
        {
            get;
        }

        public T InternalConfig
        {
            get; private set;
        }

        public event Action OnConfigUpdated;

        private void UpdateConfig(object obj, FileSystemEventArgs args)
        {
            try
            {
                var ConfigData = File.ReadAllText(ConfigPath);

                if (ConfigCache != ConfigData)
                {
                    InternalConfig = JsonConvert.DeserializeObject<T>(ConfigData);

                    OnConfigUpdated?.Invoke();
                }
            }
            catch
            {
            }
        }

        public void SaveConfig()
        {
            File.WriteAllText(ConfigPath, JsonConvert.SerializeObject(InternalConfig,  Formatting.Indented));
        }
    }
}
