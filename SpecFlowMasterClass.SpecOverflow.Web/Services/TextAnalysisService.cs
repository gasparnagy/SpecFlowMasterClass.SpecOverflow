using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SpecFlowMasterClass.SpecOverflow.Web.Services
{
    public class TextAnalysisService
    {
        private static readonly string[] CommonWords = new[]
        {
            "a", "the", "to", "is", "I"
        };

        public string[] GetRelevantWords(IEnumerable<string> texts)
        {
            return texts
                .Where(t => !string.IsNullOrWhiteSpace(t))
                .SelectMany(t => Regex.Split(t, @"\W+"))
                .Distinct()
                .Select(w => w.Trim().ToLower())
                .Where(w => !string.IsNullOrWhiteSpace(w) && !CommonWords.Contains(w))
                .ToArray();
        }

        public double GetSimilarityIndex(IEnumerable<string> texts, string[] words)
        {
            var result = 0.0;
            var textWords = GetRelevantWords(texts);
            foreach (var _ in textWords.Intersect(words))
            {
                result = 1 - ((1 - result) * 0.5);
            }
            return result;
        }
    }
}
