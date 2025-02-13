namespace CharmsBarReloaded.Updater
{
    public class UpdateItem
    {
        public string versionName { get; set; }
        public int build { get; set; }
        public string downloadLink { get; set; }
        public bool isBeta { get; set; }
        public bool isLegacy { get; set; }
    }
    public class UpdaterSettings
    {
        public bool includeBeta { get; set; }
        public bool includeLegacy { get; set; }
        public bool useCustomUpdateServer { get; set; }
        public string customUpdateServer { get; set; }
        public string customInstallPath { get; set; }
        public bool doPortableInstall { get; set; }
    }
}
