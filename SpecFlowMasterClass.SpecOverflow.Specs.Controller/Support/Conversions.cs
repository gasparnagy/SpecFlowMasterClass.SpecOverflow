using System;
using SpecFlowMasterClass.SpecOverflow.Specs.Support;
using SpecFlowMasterClass.SpecOverflow.Web.Models;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace SpecFlowMasterClass.SpecOverflow.Specs.Controller.Support
{
    [Binding]
    public class Conversions
    {
        [StepArgumentTransformation]
        public AskInputModel ConvertAskInputModel(Table questionTable)
        {
            return questionTable.CreateInstance(DomainDefaults.GetDefaultAskInput);
        }
    }
}
