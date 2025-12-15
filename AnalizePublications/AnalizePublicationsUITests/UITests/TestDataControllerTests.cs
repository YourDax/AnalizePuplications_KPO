using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http;
using Xunit;

namespace AnalizePublicationsUITests.UITests
{
    /// <summary>
    /// UI тесты для контроллера TestData
    /// </summary>
    public class TestDataControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public TestDataControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task TestDataShowDialog_ShouldReturnJson()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/TestData/ShowDialog");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("hasData", content);
        }

        [Fact]
        public async Task TestDataShowDialog_ShouldReturnValidJson()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/TestData/ShowDialog");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            Assert.True(json.StartsWith("{") && json.EndsWith("}"));
        }
    }
}


