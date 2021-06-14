using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpecFlowMasterClass.SpecOverflow.Web.Models
{
    public class AnswerDetailModel
    {
        public Guid Id { get; set; }

        public string Content { get; set; }
        public int Votes { get; set; }

        public DateTime AnsweredAt { get; set; }
        public UserReferenceModel AnsweredBy { get; set; }
    }
}
