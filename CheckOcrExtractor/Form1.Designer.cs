namespace CheckOcrExtractor;

partial class Form1
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
            components.Dispose();
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();

        // ── Top panel: File selection ──────────────────────────────────────
        grpFile = new GroupBox();
        lblFilePath = new Label();
        txtFilePath = new TextBox();
        btnBrowse = new Button();

        // ── Middle panel: Controls + Progress ─────────────────────────────
        pnlControls = new Panel();
        btnProcess = new Button();
        btnCancel = new Button();
        btnClear = new Button();

        grpProgress = new GroupBox();
        lblPageStatus = new Label();
        progressBar = new ProgressBar();
        lblStatus = new Label();

        // ── Bottom panel: Output ───────────────────────────────────────────
        grpOutput = new GroupBox();
        rtbOutput = new RichTextBox();

        // ── Status strip ──────────────────────────────────────────────────
        statusStrip = new StatusStrip();
        tsslStatus = new ToolStripStatusLabel();

        // ══════════════════════════════════════════════════════════════════
        // Form
        // ══════════════════════════════════════════════════════════════════
        SuspendLayout();
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(900, 680);
        MinimumSize = new Size(700, 560);
        Text = "Check PDF OCR Text Extractor";
        Font = new Font("Segoe UI", 9F, FontStyle.Regular);
        BackColor = Color.FromArgb(245, 245, 245);

        // ══════════════════════════════════════════════════════════════════
        // grpFile
        // ══════════════════════════════════════════════════════════════════
        grpFile.Text = "PDF File";
        grpFile.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        grpFile.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        grpFile.Location = new Point(12, 12);
        grpFile.Size = new Size(876, 62);
        grpFile.Padding = new Padding(8);

        lblFilePath.Text = "File:";
        lblFilePath.AutoSize = true;
        lblFilePath.Location = new Point(8, 28);
        lblFilePath.Font = new Font("Segoe UI", 9F);

        txtFilePath.ReadOnly = true;
        txtFilePath.PlaceholderText = "No file selected – click Browse to choose a PDF...";
        txtFilePath.Location = new Point(40, 24);
        txtFilePath.Size = new Size(720, 23);
        txtFilePath.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        txtFilePath.BackColor = Color.White;

        btnBrowse.Text = "Browse…";
        btnBrowse.Location = new Point(770, 23);
        btnBrowse.Size = new Size(90, 26);
        btnBrowse.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnBrowse.FlatStyle = FlatStyle.Flat;
        btnBrowse.BackColor = Color.FromArgb(0, 120, 212);
        btnBrowse.ForeColor = Color.White;
        btnBrowse.FlatAppearance.BorderSize = 0;
        btnBrowse.Cursor = Cursors.Hand;
        btnBrowse.Click += BtnBrowse_Click;

        grpFile.Controls.AddRange(new Control[] { lblFilePath, txtFilePath, btnBrowse });

        // ══════════════════════════════════════════════════════════════════
        // pnlControls (buttons row)
        // ══════════════════════════════════════════════════════════════════
        pnlControls.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        pnlControls.Location = new Point(12, 82);
        pnlControls.Size = new Size(876, 40);

        btnProcess.Text = "▶  Start Processing";
        btnProcess.Location = new Point(0, 4);
        btnProcess.Size = new Size(150, 32);
        btnProcess.FlatStyle = FlatStyle.Flat;
        btnProcess.BackColor = Color.FromArgb(16, 137, 62);
        btnProcess.ForeColor = Color.White;
        btnProcess.FlatAppearance.BorderSize = 0;
        btnProcess.Cursor = Cursors.Hand;
        btnProcess.Enabled = false;
        btnProcess.Click += BtnProcess_Click;

        btnCancel.Text = "■  Cancel";
        btnCancel.Location = new Point(160, 4);
        btnCancel.Size = new Size(110, 32);
        btnCancel.FlatStyle = FlatStyle.Flat;
        btnCancel.BackColor = Color.FromArgb(196, 43, 28);
        btnCancel.ForeColor = Color.White;
        btnCancel.FlatAppearance.BorderSize = 0;
        btnCancel.Cursor = Cursors.Hand;
        btnCancel.Enabled = false;
        btnCancel.Click += BtnCancel_Click;

        btnClear.Text = "🗑  Clear Output";
        btnClear.Location = new Point(280, 4);
        btnClear.Size = new Size(130, 32);
        btnClear.FlatStyle = FlatStyle.Flat;
        btnClear.BackColor = Color.FromArgb(100, 100, 100);
        btnClear.ForeColor = Color.White;
        btnClear.FlatAppearance.BorderSize = 0;
        btnClear.Cursor = Cursors.Hand;
        btnClear.Click += BtnClear_Click;

        pnlControls.Controls.AddRange(new Control[] { btnProcess, btnCancel, btnClear });

        // ══════════════════════════════════════════════════════════════════
        // grpProgress
        // ══════════════════════════════════════════════════════════════════
        grpProgress.Text = "Progress";
        grpProgress.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        grpProgress.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        grpProgress.Location = new Point(12, 130);
        grpProgress.Size = new Size(876, 75);
        grpProgress.Padding = new Padding(8);

        lblPageStatus.Text = "Page: –";
        lblPageStatus.AutoSize = true;
        lblPageStatus.Location = new Point(8, 22);
        lblPageStatus.Font = new Font("Segoe UI", 9F);

        progressBar.Location = new Point(8, 40);
        progressBar.Size = new Size(860, 20);
        progressBar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        progressBar.Style = ProgressBarStyle.Continuous;
        progressBar.Minimum = 0;
        progressBar.Value = 0;

        lblStatus.Text = "Ready.";
        lblStatus.AutoSize = true;
        lblStatus.Location = new Point(100, 22);
        lblStatus.ForeColor = Color.FromArgb(80, 80, 80);
        lblStatus.Font = new Font("Segoe UI", 9F, FontStyle.Italic);

        grpProgress.Controls.AddRange(new Control[] { lblPageStatus, progressBar, lblStatus });

        // ══════════════════════════════════════════════════════════════════
        // grpOutput
        // ══════════════════════════════════════════════════════════════════
        grpOutput.Text = "Extracted Text Output";
        grpOutput.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        grpOutput.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        grpOutput.Location = new Point(12, 215);
        grpOutput.Size = new Size(876, 416);
        grpOutput.Padding = new Padding(8);

        rtbOutput.Location = new Point(8, 22);
        rtbOutput.Size = new Size(860, 380);
        rtbOutput.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        rtbOutput.ReadOnly = true;
        rtbOutput.BackColor = Color.White;
        rtbOutput.Font = new Font("Consolas", 10F);
        rtbOutput.BorderStyle = BorderStyle.None;
        rtbOutput.ScrollBars = RichTextBoxScrollBars.Vertical;
        rtbOutput.WordWrap = true;

        grpOutput.Controls.Add(rtbOutput);

        // ══════════════════════════════════════════════════════════════════
        // StatusStrip
        // ══════════════════════════════════════════════════════════════════
        tsslStatus.Text = "Ready";
        tsslStatus.Spring = true;
        tsslStatus.TextAlign = ContentAlignment.MiddleLeft;

        statusStrip.Items.Add(tsslStatus);
        statusStrip.SizingGrip = true;

        // ══════════════════════════════════════════════════════════════════
        // Add controls to Form
        // ══════════════════════════════════════════════════════════════════
        Controls.AddRange(new Control[]
        {
            grpFile,
            pnlControls,
            grpProgress,
            grpOutput,
            statusStrip
        });

        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    // Controls
    private GroupBox grpFile = null!;
    private Label lblFilePath = null!;
    private TextBox txtFilePath = null!;
    private Button btnBrowse = null!;

    private Panel pnlControls = null!;
    private Button btnProcess = null!;
    private Button btnCancel = null!;
    private Button btnClear = null!;

    private GroupBox grpProgress = null!;
    private Label lblPageStatus = null!;
    private ProgressBar progressBar = null!;
    private Label lblStatus = null!;

    private GroupBox grpOutput = null!;
    private RichTextBox rtbOutput = null!;

    private StatusStrip statusStrip = null!;
    private ToolStripStatusLabel tsslStatus = null!;
}
