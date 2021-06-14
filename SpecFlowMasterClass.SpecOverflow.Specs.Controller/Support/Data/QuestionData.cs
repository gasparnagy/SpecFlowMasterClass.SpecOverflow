using System;
using System.Collections.Generic;
using SpecFlowMasterClass.SpecOverflow.Web.Models;

// ReSharper disable once CheckNamespace
namespace SpecFlowMasterClass.SpecOverflow.Specs.Support.Data
{
    public class QuestionData
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string Tags { get; set; }
        public int Votes { get; set; }
        public int Views { get; set; }
        public int Answers { get; set; }
        public DateTime AskedAt { get; set; }
        public string AskedBy { get; set; }

        public IEnumerable<string> TagLabels =>
            Tags.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
    }

    internal static class QuestionDataExtensions
    {
        public static QuestionData ToQuestionData(this QuestionSummaryModel questionModel)
        {
            return new()
            {
                Title = questionModel.Title,
                Votes = questionModel.Votes,
                Views = questionModel.Views,
                AskedAt = questionModel.AskedAt,
                AskedBy = questionModel.AskedBy.Name,
                Answers = questionModel.Answers
            };
        }

        public static QuestionData ToQuestionData(this QuestionDetailModel questionModel)
        {
            return new()
            {
                Title = questionModel.Title,
                Body = questionModel.Body,
                Tags = string.Join(",", questionModel.Tags),
                Votes = questionModel.Votes,
                Views = questionModel.Views,
                Answers = questionModel.Answers.Count,
                AskedAt = questionModel.AskedAt,
                AskedBy = questionModel.AskedBy.Name
            };
        }
    }
}