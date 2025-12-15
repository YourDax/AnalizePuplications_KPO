using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http;
using Xunit;

namespace AnalizePublicationsUITests.UITests
{
    /// <summary>
    /// UI тесты для главной страницы
    /// </summary>
    public class HomePageTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public HomePageTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task HomePage_ShouldReturnSuccessStatusCode()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/");
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task HomePage_ShouldContainApplicationTitle()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/");
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("AnalizePublicationsViewApp", content);
        }

        [Fact]
        public async Task HomePage_ShouldHaveNavigationLinks()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/");
            var content = await response.Content.ReadAsStringAsync();
            Assert.True(content.Contains("Просмотр и поиск") || content.Contains("Редактирование"));
        }
    }
}


