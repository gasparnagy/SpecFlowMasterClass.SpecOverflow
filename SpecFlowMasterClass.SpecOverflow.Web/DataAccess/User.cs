using System;
using System.Linq;

namespace SpecFlowMasterClass.SpecOverflow.Web.DataAccess
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Password { get; set; }
    }
}