# Check PDF OCR Text Extractor

A Windows desktop application that scans PDF files containing cheque images (printed and handwritten) and extracts the text from them using OCR (Optical Character Recognition).

## Features

- **Select any PDF file** containing scanned cheque images
- **Parallel processing** – all pages in the PDF are processed concurrently for speed
- **Live progress bar** – shows page-by-page progress as OCR runs
- **Streaming output** – extracted text appears in the output box as each page finishes
- **Signature filtering** – low-confidence, scribble-like regions (signatures, stamps) are automatically skipped
- **Supports both printed and handwritten text** using Tesseract's LSTM (neural-network) engine

## Requirements

| Requirement | Details |
|---|---|
| Operating System | Windows 10 / 11 (64-bit) |
| .NET Runtime | [.NET 8 Desktop Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) |
| Tesseract language data | `eng.traineddata` (see setup below) |

## Setup

### 1 – Install .NET 8 Desktop Runtime

Download and install the **.NET 8 Desktop Runtime** from:  
https://dotnet.microsoft.com/en-us/download/dotnet/8.0

### 2 – Download Tesseract language data

The application uses the Tesseract OCR engine and needs at least the English language file.

1. Create a folder called `tessdata` **next to** `CheckOcrExtractor.exe`:
   ```
   CheckOcrExtractor.exe
   tessdata\
       eng.traineddata
   ```
2. Download `eng.traineddata` from:
   - **Standard accuracy** (smaller file, ~10 MB):  
     https://github.com/tesseract-ocr/tessdata/raw/main/eng.traineddata
   - **Best accuracy for handwriting** (~35 MB, recommended for handwritten cheques):  
     https://github.com/tesseract-ocr/tessdata_best/raw/main/eng.traineddata

3. Place the downloaded file inside the `tessdata` folder.

### 3 – Run the application

Double-click `CheckOcrExtractor.exe`. If `.NET 8` is installed and the `tessdata` folder is in place, the application opens immediately.

## Usage

1. Click **Browse…** to select a PDF file.
2. Click **▶ Start Processing** to begin OCR.
3. The progress bar shows how many pages have been processed.
4. Extracted text appears page-by-page in the output box.
5. Use **■ Cancel** to stop processing mid-way.
6. Use **🗑 Clear Output** to reset the output box.

## Building from source

```bash
# Prerequisites: .NET 8 SDK on Windows (or with EnableWindowsTargeting on Linux/macOS)
git clone https://github.com/sagarchenchu/imagetoTextExtract.git
cd imagetoTextExtract/CheckOcrExtractor
dotnet restore
dotnet build -c Release
```

The compiled executable is placed in:
```
CheckOcrExtractor\bin\Release\net8.0-windows\CheckOcrExtractor.exe
```

Copy the `tessdata` folder next to the `.exe` before running.

## Dependencies

| Package | Purpose |
|---|---|
| [Tesseract 5.2](https://github.com/charlesw/tesseract) | .NET wrapper around the Tesseract OCR engine |
| [PDFtoImage 4.0](https://github.com/sungaila/PDFtoImage) | Renders each PDF page to a high-resolution bitmap using PDFium + SkiaSharp |

## How signature filtering works

Tesseract assigns a **confidence score** (0–100) to every recognised word.  
Signature strokes and decorative lines are not real characters, so Tesseract typically returns them with very low confidence (< 30).  
The application discards any word below that threshold, keeping only printed labels, typed amounts, and clearly legible handwriting.

## Project structure

```
CheckOcrExtractor/
├── CheckOcrExtractor.csproj   # MSBuild project (net8.0-windows, WinForms)
├── Program.cs                 # Application entry point
├── Form1.cs                   # Main form – event handlers and UI logic
├── Form1.Designer.cs          # Auto-generated form layout
└── OcrProcessor.cs            # PDF→image conversion + parallel Tesseract OCR
```
