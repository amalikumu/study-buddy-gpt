using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace studyBuddy.DocumentChunker
{
    internal class TxtTextExtractor: ITextExtractor
    {
        public string ExtractText(string filePath)
        {
            return File.ReadAllText(filePath);
        }
    }
}
