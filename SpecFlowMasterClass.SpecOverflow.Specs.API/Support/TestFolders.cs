using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using TechTalk.SpecFlow;

namespace SpecFlowMasterClass.SpecOverflow.Specs.API.Support
{
    public class TestFolders
    {
        public static readonly string UniqueId = GetRawTimestamp();

        private readonly FeatureContext _featureContext;
        private readonly ScenarioContext _scenarioContext;

        public TestFolders(FeatureContext featureContext, ScenarioContext scenarioContext)
        {
            _featureContext = featureContext;
            _scenarioContext = scenarioContext;
        }

        public string InputFolder => 
            Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);

        public string OutputFolder
        {
            //a simple solution that puts everything to the output folder directly would look like this:
            //get { return Directory.GetCurrentDirectory(); }
            get
            {
                var outputFolder = Path.Combine(Directory.GetCurrentDirectory(), UniqueId);
                if (!Directory.Exists(outputFolder))
                    Directory.CreateDirectory(outputFolder);
                return outputFolder;
            }
        }

        public string TempFolder => Path.GetTempPath();

        // very simple helper methods that can improve the test code readability

        public string GetInputFilePath(string fileName)
        {
            return Path.GetFullPath(Path.Combine(InputFolder, fileName));
        }

        public string GetOutputFilePath(string fileName)
        {
            return Path.GetFullPath(Path.Combine(OutputFolder, fileName));
        }

        public string GetTempFilePath(string fileName)
        {
            return Path.GetFullPath(Path.Combine(TempFolder, fileName));
        }

        /// <summary>
        /// Returns a raw timestamp value, that can be included in paths
        /// </summary>
        public static string GetRawTimestamp()
        {
            return DateTime.Now.ToString("s", CultureInfo.InvariantCulture).Replace(":", "");
        }

        /// <summary>
        /// Makes string path-compatible, ie removes characters not allowed in path and replaces whitespace with '_'
        /// </summary>
        public string ToPath(string s)
        {
            var builder = new StringBuilder(s);
            foreach (var invalidChar in Path.GetInvalidFileNameChars())
            {
                builder.Replace(invalidChar.ToString(), "");
            }
            builder.Replace(' ', '_');
            return builder.ToString();
        }

        public string GetScenarioSpecificFileName(string extension = "")
        {
            return $"{ToPath(_featureContext.FeatureInfo.Title)}_{ToPath(_scenarioContext.ScenarioInfo.Title)}_{DateTime.Now.Ticks}" + extension;
        }
    }
}