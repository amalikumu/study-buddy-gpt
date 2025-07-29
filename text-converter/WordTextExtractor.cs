using DocumentFormat.OpenXml.Packaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace studyBuddy.DocumentChunker
{
    internal class WordTextExtractor : ITextExtractor
    {
        public string ExtractText(string filePath)
        {
            using var doc = WordprocessingDocument.Open(filePath, false);
            return doc.MainDocumentPart?.Document?.Body?.InnerText ?? string.Empty;
        }
    }
}
