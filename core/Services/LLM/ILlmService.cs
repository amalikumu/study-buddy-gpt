using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace studyBuddy.Core.Services.LLM
{
    public interface ILlmService
    {
        public Task<string> GetAnswerAsync(string context, string question);
    }
}
