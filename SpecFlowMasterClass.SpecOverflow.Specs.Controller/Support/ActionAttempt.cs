using System;
using BoDi;
using FluentAssertions;
using FluentAssertions.Execution;
using TechTalk.SpecFlow.Infrastructure;

// ReSharper disable once CheckNamespace
namespace SpecFlowMasterClass.SpecOverflow.Specs.Support
{
    public class ActionAttempt<TInput, TResult>: IContainerDependentObject
    {
        protected ErrorMessageProvider _errorMessageProvider;
        protected TestLogger _testLogger;
        private readonly Func<TInput, TResult> _action;
        public Exception Error { get; private set; }
        public bool WasPerformed { get; private set; }
        public TInput LastInput { get; private set; }

        public bool WasSuccessful
        {
            get
            {
                using (new AssertionScope($"{ActionName} attempt performed"))
                    WasPerformed.Should().BeTrue("the action should have been performed");
                return Error == null;
            }
        }

        public string ErrorMessage => Error?.Message;
        public string ActionName { get; }

        public ActionAttempt(Func<TInput, TResult> action, string actionName = null)
        {
            _action = action;
            ActionName = actionName ?? GetType().Name.Replace("Driver", "");
        }

        public ActionAttempt()
        {
            _action = DoAction;
            ActionName = GetType().Name.Replace("Driver", "");
        }

        protected virtual TResult DoAction(TInput input)
        {
            throw new NotSupportedException();
        }

        // SpecFlow calls this method when the object is created, so you can get
        // additional dependencies without passing them through the derived ctor
        void IContainerDependentObject.SetObjectContainer(IObjectContainer container)
        {
            _errorMessageProvider = container.Resolve<ErrorMessageProvider>();
            _testLogger = container.Resolve<TestLogger>();
        }

        public TResult Perform(TInput input, bool attemptOnly = false)
        {
            WasPerformed = true;
            LastInput = input;
            try
            {
                _testLogger.LogPerformAction(ActionName, input);
                return _action(input);
            }
            catch (Exception e)
            {
                Error = e;
                _testLogger.LogPerformActionFailed(ActionName, e);
                if (!attemptOnly)
                    throw;
            }

            return default;
        }

        public void ShouldBeSuccessful()
        {
            using (new AssertionScope($"{ActionName} attempt successful"))
                WasSuccessful.Should().BeTrue(ErrorMessage);
        }

        public void ShouldFailWithError(string expectedErrorMessageKey)
        {
            var expectedErrorMessage = _errorMessageProvider.GetExpectedErrorMessage(expectedErrorMessageKey);

            var wasSuccessful = WasSuccessful;
            using (new AssertionScope($"{ActionName} attempt successful"))
                wasSuccessful.Should().BeFalse("the attempt should fail");

            using (new AssertionScope($"{ActionName} error message"))
                Error.Message.Should().ContainEquivalentOf(expectedErrorMessage);
        }
    }

    public class ActionAttemptFactory
    {
        private readonly IObjectContainer _container;

        public ActionAttemptFactory(IObjectContainer container)
        {
            _container = container;
        }

        public ActionAttempt<TInput, TResult> Create<TInput, TResult>(string name, Func<TInput, TResult> action)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (action == null) throw new ArgumentNullException(nameof(action));

            var result = new ActionAttempt<TInput, TResult>(action, name);
            ((IContainerDependentObject)result).SetObjectContainer(_container);
            return result;
        }
    }
}