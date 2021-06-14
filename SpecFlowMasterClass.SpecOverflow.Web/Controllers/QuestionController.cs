using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SpecFlowMasterClass.SpecOverflow.Web.DataAccess;
using SpecFlowMasterClass.SpecOverflow.Web.Models;
using SpecFlowMasterClass.SpecOverflow.Web.Services;
using SpecFlowMasterClass.SpecOverflow.Web.Utils;

namespace SpecFlowMasterClass.SpecOverflow.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController: ControllerBase
    {
        private readonly DataContext _dataContext = new();
        private readonly ModelTransformationService _modelTransformationService;
        private readonly IConfiguration _config;

        public QuestionController(IConfiguration config = null)
        {
            _config = config;
            _modelTransformationService = new ModelTransformationService(_dataContext);
        }

        // GET: api/question -- returns all questions
        [HttpGet]
        public List<QuestionSummaryModel> GetQuestions()
        {
            var sortedQuestions = _dataContext.Questions
                .OrderByDescending(q => q.AskedAt)
                .Select(_modelTransformationService.ToQuestionSummary)
                .ToList();
            return sortedQuestions;
        }

        // GET: api/question/[guid] -- returns the details of a question
        [HttpGet("{id}")]
        public QuestionDetailModel GetQuestionDetails(Guid id)
        {
            var question = _dataContext.Questions.FirstOrDefault(q => q.Id == id);
            if (question == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            question.Views++;
            _dataContext.SaveChanges();
            
            return _modelTransformationService.ToQuestionDetails(question);
        }

        private Question EnsureQuestion(Guid questionId)
        {
            var question = _dataContext.GetQuestionById(questionId);
            if (question == null)
                throw new HttpResponseException(HttpStatusCode.BadRequest, "Invalid question");
            return question;
        }

        // POST: api/question/ -- ask a question
        [HttpPost]
        public QuestionSummaryModel AskQuestion([FromBody] AskInputModel askInputModel, string token = null)
        {
            var userName = AuthenticationServices.EnsureAuthenticated(HttpContext, token);

            if (string.IsNullOrWhiteSpace(askInputModel.Title) || string.IsNullOrWhiteSpace(askInputModel.Body))
                throw new HttpResponseException(HttpStatusCode.BadRequest, "Title and Body cannot be empty");

            var tagIds = new List<Guid>();
            if (askInputModel.Tags != null)
                foreach (var tagLabel in askInputModel.Tags.Where(t => !string.IsNullOrWhiteSpace(t)).Select(t => t.Trim()))
                {
                    var tag = _dataContext.FindTagByLabel(tagLabel);
                    if (tag == null)
                    {
                        tag = new Tag {Label = tagLabel};
                        _dataContext.Tags.Add(tag);
                    }
                    tagIds.Add(tag.Id);
                }
            
            var question = new Question
            {
                Title = askInputModel.Title,
                Body = askInputModel.Body,
                AskedBy = _dataContext.FindUserByName(userName).Id,
                TagIds = tagIds
            };
            _dataContext.Questions.Add(question);

            _dataContext.SaveChanges();

            return _modelTransformationService.ToQuestionSummary(question);
        }

        // PUT: api/question/[guid] -- post an answer
        [HttpPut("{questionId}")]
        public AnswerDetailModel PostAnswer(Guid questionId, [FromBody] AnswerInputModel answerInputModel, string token = null)
        {
            var userName = AuthenticationServices.EnsureAuthenticated(HttpContext, token);

            var question = EnsureQuestion(questionId);
            if (string.IsNullOrWhiteSpace(answerInputModel.Content))
                throw new HttpResponseException(HttpStatusCode.BadRequest, "Content cannot be empty");

            var answer = new Answer
            {
                Content = answerInputModel.Content,
                AnsweredBy = _dataContext.FindUserByName(userName).Id
            };
            question.Answers.Add(answer);

            _dataContext.SaveChanges();

            return _modelTransformationService.ToAnswerDetails(answer);
        }

        // PUT: api/question/[guid]/vote -- vote for a question
        [HttpPut("{questionId}/vote")]
        public QuestionSummaryModel VoteQuestion(Guid questionId, [FromBody] int vote, string token = null)
        {
            var userName = AuthenticationServices.EnsureAuthenticated(HttpContext, token);

            var question = EnsureQuestion(questionId);
            if (vote != 1 && vote != -1)
                throw new HttpResponseException(HttpStatusCode.BadRequest, "You can only have one vote!");

            if (!_config.AllowVotingForYourItems() && question.AskedBy == _dataContext.FindUserByName(userName).Id)
                throw new HttpResponseException(HttpStatusCode.BadRequest, "You cannot vote for your own question!");

            question.Votes += vote;
            
            _dataContext.SaveChanges();

            return _modelTransformationService.ToQuestionSummary(question);
        }

        // PUT: api/question/[guid]/[guid]/vote -- vote for an answer
        [HttpPut("{questionId}/{answerId}/vote")]
        public AnswerDetailModel VoteAnswer(Guid questionId, Guid answerId, [FromBody] int vote, string token = null)
        {
            var userName = AuthenticationServices.EnsureAuthenticated(HttpContext, token);

            var question = EnsureQuestion(questionId);
            if (vote != 1 && vote != -1)
                throw new HttpResponseException(HttpStatusCode.BadRequest, "You can only have one vote!");

            var answer = question.Answers.FirstOrDefault(a => a.Id == answerId);
            if (answer == null)
                throw new HttpResponseException(HttpStatusCode.BadRequest, "Invalid answer");

            if (!_config.AllowVotingForYourItems() && answer.AnsweredBy == _dataContext.FindUserByName(userName).Id)
                throw new HttpResponseException(HttpStatusCode.BadRequest, "You cannot vote for your own answer!");

            answer.Votes += vote;
            
            _dataContext.SaveChanges();

            return _modelTransformationService.ToAnswerDetails(answer);
        }
    }
}
