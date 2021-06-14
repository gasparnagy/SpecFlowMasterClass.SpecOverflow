using System;
using System.Collections.Generic;

namespace SpecFlowMasterClass.SpecOverflow.Web.DataAccess
{
    public class Question
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Title { get; set; }
        public string Body { get; set; }
        public int Votes { get; set; }
        public int Views { get; set; }
        
        public DateTime AskedAt { get; set; } = DateTime.Now;
        public Guid AskedBy { get; set; }

        public List<Guid> TagIds { get; set; }
        public List<Answer> Answers { get; set; } = new();
    }
}
