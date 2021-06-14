using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using SpecFlowMasterClass.SpecOverflow.Web;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace SpecFlowMasterClass.SpecOverflow.Specs.WebUI.Support
{
    public class AppHostingContext : IDisposable
    {
        class TestAppFactory : WebApplicationFactory<Startup>
        {
            protected override void ConfigureWebHost(IWebHostBuilder builder)
            {
                base.ConfigureWebHost(builder);
                builder.ConfigureLogging((_, loggingBuilder) =>
                {
                    loggingBuilder.ClearProviders();
                    loggingBuilder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, DebugLoggerProvider>());
                });
            }

            private IWebHost _host;
            public Uri RootUrl { get; private set; }

            protected override TestServer CreateServer(IWebHostBuilder builder)
            {
                ClientOptions.BaseAddress = new Uri("http://localhost");
                _host = builder.Build();
                _host.Start();
                var uriString = _host.ServerFeatures.Get<IServerAddressesFeature>().Addresses.FirstOrDefault();
                if (uriString == null)
                    throw new InvalidOperationException("Unable to get app host uri");
                RootUrl = new Uri(uriString);
                return new TestServer(new WebHostBuilder().UseStartup<Startup>());
            }

            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);
                if (disposing)
                {
                    _host?.Dispose();
                }
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

        public Uri RootUrl => ((TestAppFactory)_webApplicationFactory)?.RootUrl;

        public void StartApp()
        {
            if (_webApplicationFactory == null)
            {
                Console.WriteLine("Starting Web Application...");
                _webApplicationFactory = new TestAppFactory();
                _webApplicationFactory.CreateClient();
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
