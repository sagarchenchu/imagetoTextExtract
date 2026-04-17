namespace CheckOcrExtractor;

static class Program
{
    [STAThread]
    static void Main()
    {
        // Enable modern visual styles and DPI awareness.
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        ApplicationConfiguration.Initialize();
        Application.Run(new Form1());
    }
}
