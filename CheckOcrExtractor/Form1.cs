using System.Diagnostics;

namespace CheckOcrExtractor;

public partial class Form1 : Form
{
    // Path to the tessdata directory shipped alongside the executable.
    private static readonly string TessDataPath =
        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tessdata");

    private CancellationTokenSource? _cts;
    private readonly OcrProcessor _processor;

    // ──────────────────────────────────────────────────────────────────────
    public Form1()
    {
        InitializeComponent();
        _processor = new OcrProcessor(TessDataPath);
        EnsureTessData();
    }

    // ──────────────────────────────────────────────────────────────────────
    // Startup: verify tessdata exists or guide the user.
    // ──────────────────────────────────────────────────────────────────────
    private void EnsureTessData()
    {
        string engData = Path.Combine(TessDataPath, "eng.traineddata");
        if (!File.Exists(engData))
        {
            AppendOutput(
                "⚠  Tesseract language data not found!\n" +
                "   Expected location: " + engData + "\n\n" +
                "   Steps to fix:\n" +
                "   1. Create the folder:  " + TessDataPath + "\n" +
                "   2. Download 'eng.traineddata' from:\n" +
                "      https://github.com/tesseract-ocr/tessdata/raw/main/eng.traineddata\n" +
                "   3. Place the file in the tessdata folder.\n" +
                "   4. Re-launch the application.\n\n" +
                "   (For handwritten text, also download 'eng.traineddata' from\n" +
                "    the tessdata_best repository for higher accuracy.)\n");

            btnProcess.Enabled = false;
            SetStatus("⚠  Tessdata missing – see output for instructions.");
        }
    }

    // ──────────────────────────────────────────────────────────────────────
    // Browse button
    // ──────────────────────────────────────────────────────────────────────
    private void BtnBrowse_Click(object? sender, EventArgs e)
    {
        using var dlg = new OpenFileDialog
        {
            Title = "Select a PDF file",
            Filter = "PDF Files (*.pdf)|*.pdf|All Files (*.*)|*.*",
            FilterIndex = 1,
            RestoreDirectory = true
        };

        if (dlg.ShowDialog(this) == DialogResult.OK)
        {
            txtFilePath.Text = dlg.FileName;
            ResetProgress();
            btnProcess.Enabled = File.Exists(dlg.FileName)
                && File.Exists(Path.Combine(TessDataPath, "eng.traineddata"));
            SetStatus($"File selected: {Path.GetFileName(dlg.FileName)}");
        }
    }

    // ──────────────────────────────────────────────────────────────────────
    // Start Processing button
    // ──────────────────────────────────────────────────────────────────────
    private async void BtnProcess_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtFilePath.Text) || !File.Exists(txtFilePath.Text))
        {
            MessageBox.Show(this, "Please select a valid PDF file first.",
                "No File", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        SetBusy(true);
        ResetProgress();
        rtbOutput.Clear();

        _cts = new CancellationTokenSource();
        var sw = Stopwatch.StartNew();

        var progress = new Progress<OcrProgress>(p =>
        {
            // This callback runs on the UI thread.
            progressBar.Maximum = p.TotalPages;
            progressBar.Value = p.CompletedPages;

            lblPageStatus.Text = $"Page: {p.CompletedPages} / {p.TotalPages}";
            SetStatus($"Finished page {p.LastProcessedPage} of {p.TotalPages}…");

            if (!string.IsNullOrWhiteSpace(p.PartialText))
            {
                AppendPageResult(p.LastProcessedPage, p.PartialText);
            }
        });

        try
        {
            AppendOutput($"Processing: {txtFilePath.Text}\n");
            AppendOutput(new string('─', 70) + "\n");

            string[] results = await _processor.ProcessPdfAsync(
                txtFilePath.Text, progress, _cts.Token);

            sw.Stop();

            // If any pages produced no text, note that.
            for (int i = 0; i < results.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(results[i]))
                {
                    AppendOutput($"\n[Page {i + 1}: No readable text detected]\n");
                }
            }

            AppendOutput(new string('─', 70) + "\n");
            AppendOutput($"✔  Done – {results.Length} page(s) processed in {sw.Elapsed.TotalSeconds:F1}s\n");
            SetStatus($"✔  Complete – {results.Length} page(s) in {sw.Elapsed.TotalSeconds:F1}s");
        }
        catch (OperationCanceledException)
        {
            AppendOutput("\n⚠  Processing cancelled by user.\n");
            SetStatus("Cancelled.");
        }
        catch (Exception ex)
        {
            AppendOutput($"\n❌  Error: {ex.Message}\n");
            SetStatus($"Error: {ex.Message}");

            MessageBox.Show(this,
                $"An error occurred during processing:\n\n{ex.Message}\n\n{ex.InnerException?.Message}",
                "Processing Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            _cts?.Dispose();
            _cts = null;
            SetBusy(false);
        }
    }

    // ──────────────────────────────────────────────────────────────────────
    // Cancel button
    // ──────────────────────────────────────────────────────────────────────
    private void BtnCancel_Click(object? sender, EventArgs e)
    {
        _cts?.Cancel();
        SetStatus("Cancelling…");
        btnCancel.Enabled = false;
    }

    // ──────────────────────────────────────────────────────────────────────
    // Clear button
    // ──────────────────────────────────────────────────────────────────────
    private void BtnClear_Click(object? sender, EventArgs e)
    {
        rtbOutput.Clear();
        ResetProgress();
        SetStatus("Output cleared.");
    }

    // ──────────────────────────────────────────────────────────────────────
    // Helpers
    // ──────────────────────────────────────────────────────────────────────

    private void AppendPageResult(int pageNumber, string text)
    {
        if (rtbOutput.InvokeRequired)
        {
            rtbOutput.BeginInvoke(() => AppendPageResult(pageNumber, text));
            return;
        }

        int start = rtbOutput.TextLength;
        rtbOutput.AppendText($"\n── Page {pageNumber} ─────────────────────────────────────────\n");

        // Style the page header
        rtbOutput.Select(start, rtbOutput.TextLength - start);
        rtbOutput.SelectionColor = Color.FromArgb(0, 100, 160);
        rtbOutput.SelectionFont = new Font("Consolas", 10F, FontStyle.Bold);

        int textStart = rtbOutput.TextLength;
        rtbOutput.AppendText(text + "\n");
        rtbOutput.Select(textStart, rtbOutput.TextLength - textStart);
        rtbOutput.SelectionColor = Color.Black;
        rtbOutput.SelectionFont = new Font("Consolas", 10F);

        // Deselect and scroll to end
        rtbOutput.Select(rtbOutput.TextLength, 0);
        rtbOutput.ScrollToCaret();
    }

    private void AppendOutput(string text)
    {
        if (rtbOutput.InvokeRequired)
        {
            rtbOutput.BeginInvoke(() => AppendOutput(text));
            return;
        }
        rtbOutput.AppendText(text);
        rtbOutput.Select(rtbOutput.TextLength, 0);
        rtbOutput.ScrollToCaret();
    }

    private void SetStatus(string message)
    {
        if (InvokeRequired)
        {
            BeginInvoke(() => SetStatus(message));
            return;
        }
        tsslStatus.Text = message;
        lblStatus.Text = message;
    }

    private void SetBusy(bool busy)
    {
        if (InvokeRequired)
        {
            BeginInvoke(() => SetBusy(busy));
            return;
        }
        btnProcess.Enabled = !busy;
        btnBrowse.Enabled = !busy;
        btnCancel.Enabled = busy;
        Cursor = busy ? Cursors.WaitCursor : Cursors.Default;
    }

    private void ResetProgress()
    {
        progressBar.Value = 0;
        progressBar.Maximum = 100;
        lblPageStatus.Text = "Page: –";
        lblStatus.Text = "Ready.";
    }
}
