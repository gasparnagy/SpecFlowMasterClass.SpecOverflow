using System;
using SpecFlowMasterClass.SpecOverflow.Specs.Support.Data;
using SpecFlowMasterClass.SpecOverflow.Web.DataAccess;
using SpecFlowMasterClass.SpecOverflow.Web.Models;

// ReSharper disable once CheckNamespace
namespace SpecFlowMasterClass.SpecOverflow.Specs.Support
{
    public static class DomainDefaults
    {
        public const string UserName = "Marvin";
        public const string UserPassword = "1234";

        public const string AltUserName = "Ford";
        public const string AltUserPassword = "1423";

        public const string OriginalOpUserName = "Jeltz";
        public const string OriginalOpUserPassword = "1423";

        public const string SampleTag1 = "tag1";
        public const string SampleTag2 = "tag2";
        public const string SampleTag3 = "tag3";

        public static AskInputModel GetDefaultAskInput()
        {
            return new()
            {
                Title = "T" + Guid.NewGuid().ToString("N"),
                Body = "B" + Guid.NewGuid().ToString("N"),
                Tags = new[] {SampleTag3}
            };
        }
        
        public static QuestionData GetDefaultQuestion(Action<QuestionData> overrides = null)
        {
            QuestionData question = new()
            {
                Title = "T" + Guid.NewGuid().ToString("N"),
                Body = "B" + Guid.NewGuid().ToString("N"),
                Tags = $"{SampleTag1},{SampleTag2}",
                Answers = 1,
                Views = 3,
                Votes = 0,
                AskedBy = OriginalOpUserName,
                AskedAt = CurrentTimeProvider.GetActionTime()
            };
            overrides?.Invoke(question);
            return question;
        }

        public static AnswerData GetDefaultAnswer(Action<AnswerData> overrides = null)
        {
            AnswerData answer = new()
            {
                Content = "Sample content",
                Votes = 0,
                AnsweredBy = OriginalOpUserName,
                AnsweredAt = CurrentTimeProvider.GetActionTime()
            };
            overrides?.Invoke(answer);
            return answer;
        }

        public static void AddDefaultUsers()
        {
            var db = new DataContext();
            db.Users.Add(new User {Name = UserName, Password = UserPassword});
            db.Users.Add(new User {Name = AltUserName, Password = AltUserPassword});
            db.Users.Add(new User {Name = OriginalOpUserName, Password = OriginalOpUserPassword});
            db.SaveChanges();
        }
    }
}
