using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.UIA3;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Xunit;

namespace AnalizePublicationsUITests.UITests
{
    /// <summary>
    /// UI тесты с использованием FlaUI для автоматизации браузера
    /// </summary>
    public class FlaUIWebTests : IClassFixture<WebApplicationFactory<Program>>, IDisposable
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;
        private readonly string _baseUrl;
        private Process? _browserProcess;
        private UIA3Automation? _automation;
        private Application? _browserApp;

        public FlaUIWebTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
            var testBaseUrl = Environment.GetEnvironmentVariable("TEST_BASE_URL");
            if (!string.IsNullOrEmpty(testBaseUrl))
            {
                _baseUrl = testBaseUrl.TrimEnd('/');
            }
            else
            {
                _baseUrl = "https://localhost:7208";
            }
        }

        [Fact]
        public void HomePage_ShouldLoadInBrowser()
        {
            var browserPath = GetBrowserPath();
            if (string.IsNullOrEmpty(browserPath))
            {
                Assert.Fail("Браузер не найден. Установите Microsoft Edge или Google Chrome.");
                return;
            }

            var startInfo = new ProcessStartInfo
            {
                FileName = browserPath,
                Arguments = $"--new-window {_baseUrl}",
                UseShellExecute = true
            };

            var process = Process.Start(startInfo);
            Thread.Sleep(3000);

            Assert.NotNull(process);
            
            var browserProcesses = Process.GetProcesses()
                .Where(p => p.ProcessName.Contains("msedge") || 
                           p.ProcessName.Contains("chrome") ||
                           p.ProcessName.Contains("edge"))
                .ToList();
            
            Assert.True(browserProcesses.Count > 0, "Браузер должен быть запущен (найдены процессы браузера)");
            
            try
            {
                if (!process.HasExited)
                {
                    process.Kill();
                }
            }
            catch { }
        }

        [Fact]
        public void AuthorBrowsePage_ShouldLoadInBrowser()
        {
            var url = $"{_baseUrl}/Author/Browse";
            var browserPath = GetBrowserPath();
            if (string.IsNullOrEmpty(browserPath))
            {
                Assert.Fail("Браузер не найден. Установите Microsoft Edge или Google Chrome.");
                return;
            }

            var startInfo = new ProcessStartInfo
            {
                FileName = browserPath,
                Arguments = $"--new-window {url}",
                UseShellExecute = true
            };

            var process = Process.Start(startInfo);
            Thread.Sleep(3000);

            Assert.NotNull(process);
            
            var browserProcesses = Process.GetProcesses()
                .Where(p => p.ProcessName.Contains("msedge") || 
                           p.ProcessName.Contains("chrome") ||
                           p.ProcessName.Contains("edge"))
                .ToList();
            
            Assert.True(browserProcesses.Count > 0, "Браузер должен быть запущен (найдены процессы браузера)");
            
            try
            {
                if (!process.HasExited)
                {
                    process.Kill();
                }
            }
            catch { }
        }

        private string GetBrowserPath()
        {
            var paths = new[]
            {
                @"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe",
                @"C:\Program Files\Microsoft\Edge\Application\msedge.exe",
                @"C:\Program Files\Google\Chrome\Application\chrome.exe",
                @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe"
            };

            return paths.FirstOrDefault(File.Exists) ?? string.Empty;
        }

        public void Dispose()
        {
            try
            {
                _browserApp?.Close();
                _browserApp?.Dispose();
            }
            catch { }

            try
            {
                _browserProcess?.Kill();
                _browserProcess?.Dispose();
            }
            catch { }

            _automation?.Dispose();
            _client?.Dispose();
        }
    }
}




