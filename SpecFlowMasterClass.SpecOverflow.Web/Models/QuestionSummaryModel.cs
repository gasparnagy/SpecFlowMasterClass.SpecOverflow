using System;

namespace SpecFlowMasterClass.SpecOverflow.Web.Models
{
    public class QuestionSummaryModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; }
        public int Votes { get; set; }
        public int Views { get; set; }
        public int Answers { get; set; }

        public DateTime AskedAt { get; set; }
        public UserReferenceModel AskedBy { get; set; }
    }
}