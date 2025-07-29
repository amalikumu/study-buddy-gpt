using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace studyBuddy.DocumentChunker
{
    public class TextExtractorFactory
    {
        public static ITextExtractor CreateExtractor(string filePath)
        {
            var extension = Path.GetExtension(filePath).ToLower();
            return extension switch
            {
                ".pdf" => new PdfTextExtractor(),
                ".docx" => new WordTextExtractor(),
                ".txt" => new TxtTextExtractor(),
                _ => throw new NotSupportedException($"Unsupported file type: {extension}")
            };
        }
    }
}
