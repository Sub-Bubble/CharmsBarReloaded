using System.IO;
using System.Reflection;
using System.Text.Json;

namespace CharmsBarReloaded.Config
{
    public partial class CharmsConfig
    {
        #region App Version
        private static Assembly assembly = Assembly.GetExecutingAssembly();
        public static string GetBuild { get { return assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion.Trim(); } }
        public static int GetVersion { get { return assembly.GetName().Version.Major; } }
        #endregion App Version
        public string CurrentLocale { get; set; } = "en-us";
        public bool EnableAnimations { get; set; } = true;
        public CharmsBarConfig charmsBarConfig { get; set; } = new CharmsBarConfig();
        public CharmsClockConfig charmsClockConfig { get; set; } = new CharmsClockConfig();

        #region Methods
        private static readonly string configPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CharmsBarReloaded");
        private static readonly string configFile = Path.Combine(configPath, "config.json");
        public void Save()
        {
            if (!File.Exists(configFile))
            {
                if (!Directory.Exists(configPath)) Directory.CreateDirectory(configPath);
                using (FileStream fs = File.Create(configFile)) { }
                File.Create(configPath);
            }
            File.WriteAllText(configFile, JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true }));
        }
        public CharmsConfig Load()
        {
            if (!File.Exists(configFile))
            {
                if (!Directory.Exists(configPath)) Directory.CreateDirectory(configPath);
                using (FileStream fs = File.Create(configFile)) { }
                File.WriteAllText(configFile, JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true}));
                return this;
            }

            try
            {
                return JsonSerializer.Deserialize<CharmsConfig>(File.ReadAllText(configFile));
            }
            catch
            {
                MessageBox.Show("Invalid Config file! Using default.", "CharmsBar: Reloaded", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Save();
                return this;
            }
        }
        #endregion Methods
    }
}
