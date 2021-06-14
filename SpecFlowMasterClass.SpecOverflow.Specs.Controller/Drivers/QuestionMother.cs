using System;
using System.Collections.Generic;
using System.Linq;
using SpecFlowMasterClass.SpecOverflow.Specs.Support;
using SpecFlowMasterClass.SpecOverflow.Specs.Support.Data;
using SpecFlowMasterClass.SpecOverflow.Web.DataAccess;
using SpecFlowMasterClass.SpecOverflow.Web.Services;

// ReSharper disable once CheckNamespace
namespace SpecFlowMasterClass.SpecOverflow.Specs.Drivers
{
    public class QuestionMother
    {
        private readonly QuestionContext _questionContext;
        private readonly ModelTransformationService _modelTransformationService;
        private readonly DataContext _testDataContext;
        private readonly TestLogger _testLogger;

        public QuestionMother(QuestionContext questionContext, ModelTransformationService modelTransformationService, DataContext testDataContext, TestLogger testLogger)
        {
            _questionContext = questionContext;
            _modelTransformationService = modelTransformationService;
            _testDataContext = testDataContext;
            _testLogger = testLogger;
        }

        private Question CreateQuestion(QuestionData questionData)
        {
            _testDataContext.EnsureTags(questionData.TagLabels);
            
            return new()
            {
                Title = questionData.Title,
                Body = questionData.Body,
                TagIds = _testDataContext.GetTagIds(questionData.TagLabels),
                Views = questionData.Views,
                Votes = questionData.Votes,
                AskedAt = questionData.AskedAt,
                AskedBy = _testDataContext.FindUserByName(questionData.AskedBy).Id
            };
        }

        private Answer CreateAnswer(AnswerData ad)
        {
            return new()
            {
                Content = ad.Content,
                Votes = ad.Votes,
                AnsweredAt = ad.AnsweredAt,
                AnsweredBy = _testDataContext.FindUserByName(ad.AnsweredBy).Id,
            };
        }

        public void GenerateQuestions(IEnumerable<QuestionData> questions)
        {
            foreach (var questionData in questions)
            {
                var question = CreateQuestion(questionData);
                GenerateAnswers(question, questionData.Answers);
                _testDataContext.Questions.Add(question);
                var questionModel = _modelTransformationService.ToQuestionDetails(question);
                _questionContext.QuestionsCreated.Add(questionModel);
                _questionContext.CurrentQuestion = questionModel;

                _testLogger.LogCreatedQuestion(questionModel);
            }
            _testDataContext.SaveChanges();
        }

        private void GenerateAnswers(Question question, in int answerCount)
        {
            var answers = Enumerable.Range(0, answerCount)
                .Select(_ => DomainDefaults.GetDefaultAnswer());
            GenerateAnswers(question, answers);
        }

        private void GenerateAnswers(Question question, IEnumerable<AnswerData> answersData)
        {
            var answers = answersData
                .Select(CreateAnswer)
                .ToList();
            question.Answers = answers;

            if (answers.Any())
                _questionContext.CurrentAnswer = _modelTransformationService.ToAnswerDetails(answers.Last());
        }

        public void GenerateAnswersForCurrentQuestion(IEnumerable<AnswerData> answers)
        {
            var question = _testDataContext.GetQuestionById(_questionContext.CurrentQuestionId);
            GenerateAnswers(question, answers);
            _testDataContext.SaveChanges();
        }
    }
}
