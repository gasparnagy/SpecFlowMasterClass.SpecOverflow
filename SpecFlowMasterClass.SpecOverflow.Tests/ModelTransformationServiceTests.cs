using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpecFlowMasterClass.SpecOverflow.Web.DataAccess;
using SpecFlowMasterClass.SpecOverflow.Web.Services;

namespace SpecFlowMasterClass.SpecOverflow.Tests
{
    [TestClass]
    public class ModelTransformationServiceTests
    {
        private readonly DataContext _dataContext = new DataContext(new DataContext.InMemoryPersist());

        private Question CreateBaseQuestion()
        {
            return new Question
            {
                Id = Guid.NewGuid(),
                Title = "title1",
                Body = "body1",
                AskedAt = DateTime.Now,
                Views = 2,
                Votes = 3,
            };
        }

        [TestMethod]
        public void ToQuestionDetails_should_keep_simple_values()
        {
            var question = CreateBaseQuestion();
            var sut = new ModelTransformationService(_dataContext);

            var result = sut.ToQuestionDetails(question);
            
            Assert.AreEqual(question.Id, result.Id);
            Assert.AreEqual(question.Title, result.Title);
            Assert.AreEqual(question.Body, result.Body);
            Assert.AreEqual(question.AskedAt, result.AskedAt);
            Assert.AreEqual(question.Views, result.Views);
            Assert.AreEqual(question.Votes, result.Votes);
        }

        [TestMethod]
        public void ToQuestionDetails_should_get_tag_labels()
        {
            var tag1 = new Tag {Label = "tag1"};
            var tag2 = new Tag {Label = "tag2"};
            _dataContext.Tags.AddRange(new []{ tag1, tag2 });
            var question = CreateBaseQuestion();
            question.TagIds = new List<Guid>() {tag1.Id, tag2.Id};
            var sut = new ModelTransformationService(_dataContext);

            var result = sut.ToQuestionDetails(question);
            
            CollectionAssert.AreEqual(new[] {"tag1", "tag2"}, result.Tags);
        }

        [TestMethod]
        public void ToQuestionDetails_should_get_user_reference()
        {
            var user = new User {Name = "XY", Password = "1234"};
            _dataContext.Users.Add(user);
            var question = CreateBaseQuestion();
            question.AskedBy = user.Id;
            var sut = new ModelTransformationService(_dataContext);

            var result = sut.ToQuestionDetails(question);
            
            Assert.AreEqual(user.Id, result.AskedBy.Id);
            Assert.AreEqual(user.Name, result.AskedBy.Name);
        }

        [TestMethod]
        public void ToQuestionDetails_should_get_answers_by_vote_descending()
        {
            var answer1 = new Answer { Content = "Content1", Votes = 1};
            var answer2 = new Answer { Content = "Content2", Votes = 3};
            var question = CreateBaseQuestion();
            question.Answers.AddRange(new []{ answer1, answer2 });
            var sut = new ModelTransformationService(_dataContext);

            var result = sut.ToQuestionDetails(question);
            
            Assert.AreEqual(2, result.Answers.Count);
            Assert.AreEqual(answer2.Id, result.Answers[0].Id);
            Assert.AreEqual(answer1.Id, result.Answers[1].Id);
            Assert.AreEqual(answer2.Content, result.Answers[0].Content);
            Assert.AreEqual(answer2.Votes, result.Answers[0].Votes);
        }

        [TestMethod]
        public void ToQuestionSummary_should_keep_simple_values()
        {
            var question = CreateBaseQuestion();
            var sut = new ModelTransformationService(_dataContext);

            var result = sut.ToQuestionSummary(question);
            
            Assert.AreEqual(question.Id, result.Id);
            Assert.AreEqual(question.Title, result.Title);
            Assert.AreEqual(question.AskedAt, result.AskedAt);
            Assert.AreEqual(question.Views, result.Views);
            Assert.AreEqual(question.Votes, result.Votes);
        }

        [TestMethod]
        public void ToQuestionSummary_should_get_user_reference()
        {
            var user = new User {Name = "XY", Password = "1234"};
            _dataContext.Users.Add(user);
            var question = CreateBaseQuestion();
            question.AskedBy = user.Id;
            var sut = new ModelTransformationService(_dataContext);

            var result = sut.ToQuestionSummary(question);
            
            Assert.AreEqual(user.Id, result.AskedBy.Id);
            Assert.AreEqual(user.Name, result.AskedBy.Name);
        }

        [TestMethod]
        public void ToQuestionSummary_should_get_answers_count()
        {
            var answer1 = new Answer { Content = "Content1", Votes = 1};
            var answer2 = new Answer { Content = "Content2", Votes = 3};
            var question = CreateBaseQuestion();
            question.Answers.AddRange(new []{ answer1, answer2 });
            var sut = new ModelTransformationService(_dataContext);

            var result = sut.ToQuestionSummary(question);
            
            Assert.AreEqual(2, result.Answers);
        }

        [TestMethod]
        public void ToAnswerDetails_should_keep_simple_values()
        {
            var answer = new Answer { Content = "Content1", Votes = 1 };
            var sut = new ModelTransformationService(_dataContext);

            var result = sut.ToAnswerDetails(answer);
            
            Assert.AreEqual(answer.Id, result.Id);
            Assert.AreEqual(answer.Content, result.Content);
            Assert.AreEqual(answer.AnsweredAt, result.AnsweredAt);
            Assert.AreEqual(answer.Votes, result.Votes);
        }

        [TestMethod]
        public void ToAnswerDetails_should_get_user_reference()
        {
            var user = new User {Name = "XY", Password = "1234"};
            _dataContext.Users.Add(user);
            var answer = new Answer { Content = "Content1", AnsweredBy = user.Id };
            var sut = new ModelTransformationService(_dataContext);

            var result = sut.ToAnswerDetails(answer);
            
            Assert.AreEqual(user.Id, result.AnsweredBy.Id);
            Assert.AreEqual(user.Name, result.AnsweredBy.Name);
        }

        [TestMethod]
        public void ToUserReference_should_get_user_id_and_name()
        {
            var user = new User {Name = "XY", Password = "1234"};
            var sut = new ModelTransformationService(_dataContext);

            var result = sut.ToUserReference(user);
            
            Assert.AreEqual(user.Id, result.Id);
            Assert.AreEqual(user.Name, result.Name);
        }

        [TestMethod]
        public void ToUserReference_should_create_unknown_user_placeholder_for_null()
        {
            var sut = new ModelTransformationService(_dataContext);

            var result = sut.ToUserReference(null);
            
            Assert.AreEqual(Guid.Empty, result.Id);
            Assert.AreEqual("???", result.Name);
        }
    }
}
