namespace CheckOcrExtractor;

static class Program
{
    [STAThread]
    static void Main()
    {
        // Sets high-DPI awareness, visual styles, and text rendering in one call.
        ApplicationConfiguration.Initialize();
        Application.Run(new Form1());
    }
}
