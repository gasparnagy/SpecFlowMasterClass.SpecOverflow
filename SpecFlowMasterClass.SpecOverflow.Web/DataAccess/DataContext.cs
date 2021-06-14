using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using SpecFlowMasterClass.SpecOverflow.Web.Services;
using Newtonsoft.Json;

namespace SpecFlowMasterClass.SpecOverflow.Web.DataAccess
{
    /// <summary>
    /// A database connection to perform data query and manipulation
    /// </summary>
    public class DataContext
    {
        public List<User> Users { get; set; } = new List<User>();
        public List<Question> Questions { get; set; } = new();
        public List<Tag> Tags { get; set; } = new();

        public DataContext()
        {
            LoadData();
        }
        
        public User FindUserByName(string userName)
        {
            return Users.FirstOrDefault(u => u.Name.Equals(userName, StringComparison.CurrentCultureIgnoreCase));
        }

        public IEnumerable<Tag> GetTagsByIds(IEnumerable<Guid> tagsIds)
        {
            if (tagsIds == null)
                return new Tag[0];
            return tagsIds
                .Select(tid => Tags.FirstOrDefault(t => t.Id == tid))
                .Where(t => t != null);
        }

        public List<Guid> GetTagIds(IEnumerable<string> tagNames) =>
            tagNames.Select(tn => Tags.FirstOrDefault(t => t.Label == tn))
                .Where(t => t != null)
                .Select(t => t.Id)
                .ToList();

        public User GetUserById(Guid userId)
        {
            return Users.FirstOrDefault(t => t.Id == userId);
        }

        public Question GetQuestionById(Guid questionId)
        {
            return Questions.FirstOrDefault(t => t.Id == questionId);
        }

        public Tag FindTagByLabel(string tagLabel)
        {
            return Tags.FirstOrDefault(t => t.Label == tagLabel);
        }

        public void EnsureTags(IEnumerable<string> tagLabels)
        {
            foreach (var tagLabel in tagLabels)
            {
                var tag = FindTagByLabel(tagLabel);
                if (tag == null)
                {
                    tag = new Tag
                    {
                        Label = tagLabel
                    };
                    Tags.Add(tag);
                }
            }
        }

        public void SaveChanges()
        {
            SaveData();
        }

        public void TruncateTables()
        {
            ClearData();
            SaveData();
        }

        #region Simulating Database

        internal static Func<IDataPersist> CreateDataPersist = () => new TempFileDataPersist();
        
        private class Database
        {
            public User[] Users { get; set; }
            public Question[] Questions { get; set; }
            public Tag[] Tags { get; set; }
        }

        private readonly IDataPersist _dataPersist = CreateDataPersist();

        internal DataContext(IDataPersist dataPersist)
        {
            _dataPersist = dataPersist;
            LoadData();
        }

        private void ClearData()
        {
            Users.Clear();
            Questions.Clear();
            Tags.Clear();
        }

        private void SaveData()
        {
            var db = new Database
            {
                Users = Users.ToArray(),
                Questions = Questions.ToArray(),
                Tags = Tags.ToArray()
            };
            var json = JsonConvert.SerializeObject(db);
            _dataPersist.SaveToFile(json);
        }

        private void LoadData()
        {
            var json = _dataPersist.LoadFromFile();
            ClearData();
            if (string.IsNullOrEmpty(json) || !DeserializeDatabase(json, out var db))
            {
                DefaultDataServices.SeedWithDefaultData(this);
                return;
            }
            Users.AddRange(db.Users ?? new User[0]);
            Questions.AddRange(db.Questions ?? new Question[0]);
            Tags.AddRange(db.Tags ?? new Tag[0]);
        }

        private bool DeserializeDatabase(string json, out Database database)
        {
            try
            {
                database = JsonConvert.DeserializeObject<Database>(json);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex, "DeserializeDatabase");
                database = null;
                return false;
            }
        }

        internal interface IDataPersist
        {
            void SaveToFile(string json);
            string LoadFromFile();
        }

        internal class TempFileDataPersist : IDataPersist
        {
            private static readonly object LockObj = new object();

            private readonly string _databaseFilePath = Environment.ExpandEnvironmentVariables(@"%TMP%\SpecOverflowDb.json");

            public void SaveToFile(string json)
            {
                lock (LockObj)
                {
                    File.WriteAllText(_databaseFilePath, json, Encoding.UTF8);
                }
            }

            public string LoadFromFile()
            {
                lock (LockObj)
                {
                    if (!File.Exists(_databaseFilePath))
                        return string.Empty;
                    return File.ReadAllText(_databaseFilePath, Encoding.UTF8);
                }
            }
        }

        internal class InMemoryPersist : IDataPersist
        {
            private string _databaseContent = string.Empty;

            public void SaveToFile(string json)
            {
                _databaseContent = json;
            }

            public string LoadFromFile()
            {
                return _databaseContent;
            }
        }

        #endregion
    }
}