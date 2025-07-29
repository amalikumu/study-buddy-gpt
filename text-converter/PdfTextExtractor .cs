using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;

namespace studyBuddy.DocumentChunker
{
    internal class PdfTextExtractor : ITextExtractor
    {
        public string ExtractText(string filePath)
        {
            var text = new StringBuilder();
            using var document = PdfDocument.Open(filePath);
            foreach (Page page in document.GetPages())
            {
                text.AppendLine(page.Text);
            }
            return text.ToString();
        }
    }
}
