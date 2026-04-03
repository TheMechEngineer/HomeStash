namespace FrontEnd.Forms
{
    /// <summary>
    /// Static Class That Serves As The Entry Point For The Application
    /// </summary>
    internal static class Program
    {
        /// <summary>
        ///  The Main Entry Point For The Application
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            // Starts The Application And Opens The Dashboard Form
            Application.Run(new Dashboard());
        }
    }
}