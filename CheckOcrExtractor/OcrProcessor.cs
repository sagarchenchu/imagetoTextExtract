using PDFtoImage;
using SkiaSharp;
using Tesseract;

namespace CheckOcrExtractor;

/// <summary>Holds the progress state while processing a PDF.</summary>
public sealed record OcrProgress(int CompletedPages, int TotalPages, int LastProcessedPage, string PartialText);

/// <summary>Service that converts a PDF to images and extracts text via Tesseract OCR.</summary>
public sealed class OcrProcessor
{
    private readonly string _tessDataPath;
    private readonly string _language;

    // Words that OCR commonly misreads from signature strokes – filter them out.
    private static readonly HashSet<string> _signatureNoise = new(StringComparer.OrdinalIgnoreCase)
    {
        "~~", "~", "—", "—-", "/", "\\", "|", "_"
    };

    public OcrProcessor(string tessDataPath, string language = "eng")
    {
        _tessDataPath = tessDataPath;
        _language = language;
    }

    /// <summary>
    /// Processes every page of the PDF in parallel and returns one OCR result string per page.
    /// Progress is reported after each page finishes.
    /// </summary>
    public async Task<string[]> ProcessPdfAsync(
        string pdfFilePath,
        IProgress<OcrProgress>? progress,
        CancellationToken cancellationToken = default)
    {
        byte[] pdfBytes = await File.ReadAllBytesAsync(pdfFilePath, cancellationToken);

        int pageCount;
        using (var ms = new MemoryStream(pdfBytes))
            pageCount = Conversion.GetPageCount(ms);

        var results = new string[pageCount];
        int completedPages = 0;

        // Run page processing in parallel on the thread-pool.
        await Task.Run(() =>
        {
            Parallel.For(0, pageCount,
                new ParallelOptions
                {
                    CancellationToken = cancellationToken,
                    MaxDegreeOfParallelism = Math.Max(1, Environment.ProcessorCount - 1)
                },
                pageIndex =>
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    string pageText = ProcessSinglePage(pdfBytes, pageIndex);
                    results[pageIndex] = pageText;

                    int completed = Interlocked.Increment(ref completedPages);
                    progress?.Report(new OcrProgress(completed, pageCount, pageIndex + 1, pageText));
                });
        }, cancellationToken);

        return results;
    }

    /// <summary>Renders one PDF page to an image and extracts text from it.</summary>
    private string ProcessSinglePage(byte[] pdfBytes, int pageIndex)
    {
        // Each parallel task needs its own MemoryStream because PDFium is not thread-safe
        // when multiple threads share the same stream position.
        using var ms = new MemoryStream(pdfBytes);
        using SKBitmap bitmap = Conversion.ToImage(ms, page: pageIndex,
            options: new RenderOptions(Dpi: 300));

        return ExtractTextFromBitmap(bitmap);
    }

    /// <summary>Converts an SKBitmap to a Pix and runs Tesseract OCR on it.</summary>
    private string ExtractTextFromBitmap(SKBitmap bitmap)
    {
        // Encode as PNG so Pix.LoadFromMemory can decode it.
        using SKImage skImage = SKImage.FromBitmap(bitmap);
        using SKData pngData = skImage.Encode(SKEncodedImageFormat.Png, 100);
        byte[] pngBytes = pngData.ToArray();

        // TesseractEngine is not thread-safe, so each task creates its own instance.
        using var engine = new TesseractEngine(_tessDataPath, _language, EngineMode.Default);

        // PageSegMode.Auto lets Tesseract decide the layout (useful for cheques which mix
        // printed labels, typed amounts and handwritten entries).
        engine.DefaultPageSegMode = PageSegMode.Auto;

        using Pix pix = Pix.LoadFromMemory(pngBytes);
        using Page page = engine.Process(pix);

        return FilterText(page);
    }

    /// <summary>
    /// Walks through the recognised words and drops very-low-confidence regions
    /// (signature strokes, decorative lines, stamps, etc.) while keeping
    /// printed and clearly handwritten text.
    /// </summary>
    private static string FilterText(Page page)
    {
        var sb = new System.Text.StringBuilder();

        using var iter = page.GetIterator();
        iter.Begin();

        // Confidence threshold: words below this are likely noise / signatures.
        const float MinWordConfidence = 30f;

        do
        {
            if (iter.IsAtBeginningOf(PageIteratorLevel.TextLine))
            {
                if (sb.Length > 0)
                    sb.AppendLine();
            }

            string? word = iter.GetText(PageIteratorLevel.Word);
            if (string.IsNullOrWhiteSpace(word))
                continue;

            float conf = iter.GetConfidence(PageIteratorLevel.Word);
            if (conf < MinWordConfidence)
                continue;

            if (_signatureNoise.Contains(word.Trim()))
                continue;

            sb.Append(word).Append(' ');

        } while (iter.Next(PageIteratorLevel.Word));

        return sb.ToString().Trim();
    }
}
