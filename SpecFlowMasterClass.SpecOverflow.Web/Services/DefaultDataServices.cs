using System;
using System.Collections.Generic;
using System.Linq;
using SpecFlowMasterClass.SpecOverflow.Web.DataAccess;

namespace SpecFlowMasterClass.SpecOverflow.Web.Services
{
    /// <summary>
    /// Helper methods to add default data to the database
    /// </summary>
    public static class DefaultDataServices
    {
        public const string DefaultUserName = "Marvin";
        public const string DefaultPassword = "1234";

        public const string AltUserName = "Ford";
        public const string AltUserPassword = "1423";

        public const string OriginalOpUserName = "Jeltz";
        public const string OriginalOpUserPassword = "1423";

        public const string AdminUserName = "admin";
        public const string AdminPassword = "secret";

        internal static void EnsureDefaultUser()
        {
            var db = new DataContext();
            var user = db.FindUserByName(DefaultUserName);
            if (user == null)
            {
                db.Users.Add(new User { Name = DefaultUserName, Password = DefaultPassword });
                db.SaveChanges();
            }
            var admin = db.FindUserByName(AdminUserName);
            if (admin == null)
            {
                db.Users.Add(new User { Name = AdminUserName, Password = AdminPassword });
                db.SaveChanges();
            }
        }

        public static void SeedWithDefaultData(DataContext db)
        {
            AddDefaultUsers(db);
            AddDefaultTags(db);
            AddDefaultQuestions(db);
            db.SaveChanges();
        }

        private static void AddDefaultTags(DataContext db)
        {
            db.Tags.Add(new Tag {Label = "BDD"});
            db.Tags.Add(new Tag { Label = "definition" });
            db.Tags.Add(new Tag { Label = "Gherkin" });
        }

        private static void AddDefaultQuestions(DataContext db)
        {
            List<Guid> GetTagIds(IEnumerable<string> tagNames) =>
                tagNames.Select(tn => db.Tags.FirstOrDefault(t => t.Label == tn))
                    .Where(t => t != null)
                    .Select(t => t.Id)
                    .ToList();

            var defaultUser = db.FindUserByName(DefaultUserName);
            var altUser = db.FindUserByName(AltUserName);
            var originalOpUser = db.FindUserByName(OriginalOpUserName);

            db.Questions.Add(new Question
            {
                Title = "What is SpecFlow?",
                Body = "Is it the same as Cucumber?",
                Views = 13,
                Votes = 4,
                AskedBy = originalOpUser.Id,
                TagIds = GetTagIds(new[] {"BDD", "definition"})
            });
            db.Questions.Add(new Question
            {
                Title = "How to write better BDD scenarios?",
                Body = "I need help for that in my project...",
                Views = 8,
                Votes = 3,
                AskedBy = originalOpUser.Id,
                TagIds = GetTagIds(new[] {"Gherkin", "tips"}),
                Answers = new List<Answer>
                {
                    new()
                    {
                        Content = @"Learn about the BRIEF principles:

* Business Language
* Real data
* Intention revealing
* Essential
* Focused
* Brief",
                        AnsweredBy = originalOpUser.Id
                    },
                    new()
                    {
                        Content = @"Get the Formulation book!",
                        AnsweredBy = altUser.Id
                    }
                }
            });
            db.Questions.Add(new Question
            {
                Title = "What is Cucumber?",
                Body = "Just interested...",
                Views = 42,
                Votes = 2,
                AskedBy = originalOpUser.Id,
                TagIds = GetTagIds(new[] { "BDD", "definition" })
            });
        }

        private static void AddDefaultUsers(DataContext db)
        {
            db.Users.Add(new User { Name = AdminUserName, Password = AdminPassword });
            db.Users.Add(new User { Name = DefaultUserName, Password = DefaultPassword });
            db.Users.Add(new User { Name = AltUserName, Password = AltUserPassword });
            db.Users.Add(new User { Name = OriginalOpUserName, Password = OriginalOpUserPassword });
        }
    }
}