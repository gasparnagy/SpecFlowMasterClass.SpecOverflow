using System;
using System.Diagnostics;
using System.Net.Http;
using SpecFlowMasterClass.SpecOverflow.Web;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace SpecFlowMasterClass.SpecOverflow.Specs.API.Support
{
    public class AppHostingContext : IDisposable
    {
        class TestAppFactory : WebApplicationFactory<Startup>
        {
            protected override void ConfigureWebHost(IWebHostBuilder builder)
            {
                base.ConfigureWebHost(builder);
                builder.ConfigureLogging((context, loggingBuilder) =>
                {
                    loggingBuilder.ClearProviders();
                    loggingBuilder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, DebugLoggerProvider>());
                });
            }

            #region Debug Logger
            [ProviderAlias("Debug")]
            // ReSharper disable once ClassNeverInstantiated.Local
            class DebugLoggerProvider : ILoggerProvider
            {
                public ILogger CreateLogger(string name) => new DebugLogger(name);

                public void Dispose()
                {
                }
            }

            class DebugLogger : ILogger
            {
                private readonly string _name;

                public DebugLogger(string name)
                {
                    _name = string.IsNullOrEmpty(name) ? nameof(DebugLogger) : name;
                }

                public IDisposable BeginScope<TState>(TState state)
                {
                    return NoopDisposable.Instance;
                }

                public bool IsEnabled(LogLevel logLevel)
                {
                    return Debugger.IsAttached && logLevel != LogLevel.None;
                }

                public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
                {
                    if (!IsEnabled(logLevel))
                        return;

                    var message = formatter(state, exception);
                    if (string.IsNullOrEmpty(message))
                        return;

                    message = $"{ logLevel }: {message}";

                    if (exception != null)
                        message += Environment.NewLine + Environment.NewLine + exception;

                    Debug.WriteLine(message, _name);
                }

                private class NoopDisposable : IDisposable
                {
                    public static readonly NoopDisposable Instance = new NoopDisposable();

                    public void Dispose()
                    {
                    }
                }
            }
            #endregion
        }

        private static WebApplicationFactory<Startup> _webApplicationFactory;

        public HttpClient CreateClient()
        {
            StartApp();

            _webApplicationFactory.Should().NotBeNull("the app should be running");
            return _webApplicationFactory.CreateClient();
        }

        public void Dispose()
        {
            //nop
        }

        public void StartApp()
        {
            if (_webApplicationFactory == null)
            {
                Console.WriteLine("Starting Web Application...");
                _webApplicationFactory = new TestAppFactory();
            }
        }

        public static void StopApp()
        {
            if (_webApplicationFactory != null)
            {
                Console.WriteLine("Shutting down Web Application...");
                _webApplicationFactory.Dispose();
                _webApplicationFactory = null;
            }
        }
    }
}
