namespace CharmsBarReloaded.Updater
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            if (args.Length > 1)
            {
                if (args.Contains<string>("-checkforupdates"))
                {
                    if (args.Contains<string>("-includebetas"))
                    Console.WriteLine("Coming soon in pre3!");
                    throw new NotImplementedException("Coming soon in pre3!");
                }
            }
            Application.Run(new UpdaterForm());
        }
    }
}