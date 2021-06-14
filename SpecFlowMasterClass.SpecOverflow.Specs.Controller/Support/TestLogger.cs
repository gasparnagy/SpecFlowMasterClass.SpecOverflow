using System;
using SpecFlowMasterClass.SpecOverflow.Web.Models;

// ReSharper disable once CheckNamespace
namespace SpecFlowMasterClass.SpecOverflow.Specs.Support
{
    public class TestLogger
    {
        public void LogCreatedQuestion(QuestionDetailModel questionModel)
        {
            Console.WriteLine($"Question: Title: {questionModel.Title}, Body: {questionModel.Body}, Tags: {string.Join(",", questionModel.Tags)}, Votes: {questionModel.Votes}, AskedBy: {questionModel.AskedBy.Name}");
        }

        public void LogPerformAction(string actionName, object input)
        {
            Console.WriteLine($"Perform {actionName}: Input: {input}");
        }

        public void LogPerformActionFailed(string actionName, Exception exception)
        {
            Console.WriteLine($"Perform {actionName} failed: {exception.Message}");
        }
    }
}
