using System;
using System.Collections.Generic;
using SpecFlowMasterClass.SpecOverflow.Web.Models;
using TechTalk.SpecFlow;

// ReSharper disable once CheckNamespace
namespace SpecFlowMasterClass.SpecOverflow.Specs.Support
{
    public class QuestionContext
    {
        public Table QuestionSpecification { get; set; }
        public Table AnswerSpecification { get; set; }

        public List<QuestionDetailModel> QuestionsCreated { get; } = new();

        public QuestionDetailModel CurrentQuestion { get; set; }
        public AnswerDetailModel CurrentAnswer { get; set; }

        public Guid CurrentQuestionId => CurrentQuestion.Id;
        public Guid CurrentAnswerId => CurrentAnswer.Id;
    }
}
