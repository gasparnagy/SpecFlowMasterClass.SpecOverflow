using System.Collections.Generic;

namespace SpecFlowMasterClass.SpecOverflow.Web.Models
{
    /// <summary>
    /// Represents the information that should be displayed on the home page
    /// </summary>
    public class HomePageModel
    {
        public string MainMessage { get; set; }
        public string UserName { get; set; }
        public bool IsAdmin { get; set; }
        
        public List<QuestionSummaryModel> LatestQuestions { get; set; }
    }
}