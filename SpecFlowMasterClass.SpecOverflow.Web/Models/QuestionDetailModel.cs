using System;
using System.Collections.Generic;

namespace SpecFlowMasterClass.SpecOverflow.Web.Models
{
    public class QuestionDetailModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; }
        public string Body { get; set; }
        public int Votes { get; set; }
        public int Views { get; set; }

        public DateTime AskedAt { get; set; }
        public UserReferenceModel AskedBy { get; set; }

        public List<string> Tags { get; set; }
        public List<AnswerDetailModel> Answers { get; set; } = new();
    }
}
