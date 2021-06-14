using System;
using System.Linq;
using SpecFlowMasterClass.SpecOverflow.Web.DataAccess;
using SpecFlowMasterClass.SpecOverflow.Web.Models;

namespace SpecFlowMasterClass.SpecOverflow.Web.Services
{
    public class ModelTransformationService
    {
        private readonly DataContext _dataContext;

        public ModelTransformationService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public QuestionDetailModel ToQuestionDetails(Question question)
        {
            return new()
            {
                Id = question.Id,
                Title = question.Title,
                Body = question.Body,
                AskedAt = question.AskedAt,
                Views = question.Views,
                Votes = question.Votes,
                Tags = _dataContext.GetTagsByIds(question.TagIds).Select(t => t.Label).ToList(),
                AskedBy = ToUserReference(_dataContext.GetUserById(question.AskedBy)),
                Answers = question.Answers.OrderByDescending(a => a.Votes).Select(ToAnswerDetails).ToList()
            };
        }

        public QuestionSummaryModel ToQuestionSummary(Question question)
        {
            return new()
            {
                Id = question.Id,
                Title = question.Title,
                AskedAt = question.AskedAt,
                Views = question.Views,
                Votes = question.Votes,
                AskedBy = ToUserReference(_dataContext.GetUserById(question.AskedBy)),
                Answers = question.Answers.Count
            };
        }

        public AnswerDetailModel ToAnswerDetails(Answer answer)
        {
            return new()
            {
                Id = answer.Id,
                Content = answer.Content,
                AnsweredAt = answer.AnsweredAt,
                Votes = answer.Votes,
                AnsweredBy = ToUserReference(_dataContext.GetUserById(answer.AnsweredBy))
            };
        }

        public UserReferenceModel ToUserReference(User user)
        {
            return new()
            {
                Id = user?.Id ?? Guid.Empty,
                Name = user?.Name ?? "???"
            };
        }
    }
}
