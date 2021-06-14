using System;
using SpecFlowMasterClass.SpecOverflow.Web.Models;

// ReSharper disable once CheckNamespace
namespace SpecFlowMasterClass.SpecOverflow.Specs.Support.Data
{
    public class AnswerData
    {
        public string Content { get; set; }
        public int Votes { get; set; }

        public DateTime AnsweredAt { get; set; }
        public string AnsweredBy { get; set; }
    }

    internal static class AnswerDataExtensions
    {
        public static AnswerData ToAnswerData(this AnswerDetailModel answerModel)
        {
            return new()
            {
                Content = answerModel.Content,
                Votes = answerModel.Votes,
                AnsweredAt = answerModel.AnsweredAt,
                AnsweredBy = answerModel.AnsweredBy.Name
            };
        }
    }
}
