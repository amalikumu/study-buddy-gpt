using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace studyBuddy.DocumentChunker
{
    public static class TextChunker
    {
        public static List<string> ChunkTextBySentences(string text, int maxCharsPerChunk = 1000)
        {
            var chunks = new List<string>();
            var sentences = Regex.Split(text, @"(?<=[\.!\?])\s+");

            var currentChunk = new StringBuilder();
            foreach (var sentence in sentences)
            {
                if (currentChunk.Length + sentence.Length > maxCharsPerChunk)
                {
                    chunks.Add(currentChunk.ToString().Trim());
                    currentChunk.Clear();
                }
                currentChunk.Append(sentence + " ");
            }

            if (currentChunk.Length > 0)
                chunks.Add(currentChunk.ToString().Trim());

            return chunks;
        }
    }
}
