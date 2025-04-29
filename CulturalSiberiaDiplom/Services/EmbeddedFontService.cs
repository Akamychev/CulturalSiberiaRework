using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using iText.IO.Font;
using iText.Kernel.Font;

namespace CulturalSiberiaDiplom.Services;

public static class EmbeddedFontService
{
    private static readonly Dictionary<string, byte[]> FontCache = new();

    public static PdfFont GetFont(string fontName, string encoding = PdfEncodings.IDENTITY_H)
    {
        if (FontCache.TryGetValue(fontName, out var fontBytes))
            return PdfFontFactory.CreateFont(fontBytes, encoding, PdfFontFactory.EmbeddingStrategy.FORCE_EMBEDDED);

        var uri = new Uri($"pack://application:,,,/Resources/Fonts/{fontName}");
        var resourceInfo = Application.GetResourceStream(uri);

        if (resourceInfo == null)
            throw new FileNotFoundException($"Шрифт {fontName} не найден в ресурсах");

        using var ms = new MemoryStream();
        resourceInfo.Stream.CopyTo(ms);
        fontBytes = ms.ToArray();

        FontCache[fontName] = fontBytes;
        
        return PdfFontFactory.CreateFont(fontBytes, encoding, PdfFontFactory.EmbeddingStrategy.FORCE_EMBEDDED);
    }
}