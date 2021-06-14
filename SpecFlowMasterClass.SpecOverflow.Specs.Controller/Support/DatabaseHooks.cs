using SpecFlowMasterClass.SpecOverflow.Web.DataAccess;
using TechTalk.SpecFlow;

// ReSharper disable once CheckNamespace
namespace SpecFlowMasterClass.SpecOverflow.Specs.Support
{
    [Binding]
    public class DatabaseHooks
    {
        private readonly DataContext.IDataPersist _dataPersist = new DataContext.InMemoryPersist();

        [BeforeScenario(Order = 100)]
        public void ResetDatabaseToBaseline()
        {
            // configure app to use in-memory database
            DataContext.CreateDataPersist = () => _dataPersist;
            
            ClearDatabase();
            DomainDefaults.AddDefaultUsers();
        }

        private static void ClearDatabase()
        {
            var db = new DataContext();
            db.TruncateTables();
        }
    }
}
