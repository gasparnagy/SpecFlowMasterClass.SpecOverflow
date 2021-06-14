using System;

// ReSharper disable once CheckNamespace
namespace SpecFlowMasterClass.SpecOverflow.Specs.Support
{
    public class ErrorMessageProvider
    {
        public string GetExpectedErrorMessage(string messageKey)
        {
            if (messageKey.Contains(" "))
                return messageKey; // this is the ream message
            
            //TODO: load message from resource if message key like this-is-the-key is provided
            return messageKey.Replace("-", " ");
        }
    }
}
