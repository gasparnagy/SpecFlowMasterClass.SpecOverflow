using System;

namespace SpecFlowMasterClass.SpecOverflow.Web.DataAccess
{
    public class Tag
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Label { get; set; }
    }
}
