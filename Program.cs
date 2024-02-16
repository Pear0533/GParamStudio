namespace GParamStudio
{
    internal static class Program
    {
        public static GParamStudio window = new();
        public static string windowTitle = "GParam Studio";
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            window.Text = windowTitle;
            Application.Run(window);
        }
    }
}