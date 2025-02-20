// filepath: /c:/Users/zache/source/repos/msdocs-app-service-sqldb-dotnetcore/Controllers/HomeController.cs
using DotNetCoreSqlDb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using System.Diagnostics;

namespace DotNetCoreSqlDb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TelemetryClient _telemetryClient;

        public HomeController(ILogger<HomeController> logger, TelemetryClient telemetryClient)
        {
            _logger = logger;
            _telemetryClient = telemetryClient;
        }

        public IActionResult Index()
        {
            // Simulate an exception for demo purposes
            try
            {
                throw new InvalidOperationException("This is a sample exception for demo purposes.");
            }
            catch (Exception ex)
            {
                // Log the exception to Application Insights
                _telemetryClient.TrackException(new ExceptionTelemetry(ex)
                {
                    SeverityLevel = SeverityLevel.Error,
                    Properties = { { "CustomProperty", "CustomValue" } }
                });
            }

            // Track a custom metric
            _telemetryClient.GetMetric("CustomMetric").TrackValue(1);

            // Track a dependency
            var dependencyTelemetry = new DependencyTelemetry
            {
                Name = "SampleDependency",
                Data = "SampleData",
                Target = "SampleTarget",
                Type = "SampleType",
                Duration = TimeSpan.FromMilliseconds(123),
                Success = true
            };
            _telemetryClient.TrackDependency(dependencyTelemetry);

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}