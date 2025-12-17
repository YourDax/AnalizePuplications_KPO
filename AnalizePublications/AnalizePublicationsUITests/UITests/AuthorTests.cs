using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http;
using Xunit;

namespace AnalizePublicationsUITests.UITests
{
    /// <summary>
    /// UI тесты для страниц авторов
    /// </summary>
    public class AuthorTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public AuthorTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task AuthorBrowse_ShouldReturnSuccessStatusCode()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/Author/Browse");
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task AuthorSearch_ShouldReturnSuccessStatusCode()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/Author/Search");
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task AuthorIndex_ShouldReturnSuccessStatusCode()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/Author");
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task AuthorCreate_ShouldReturnSuccessStatusCode()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/Author/Create");
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task AuthorCreate_ShouldContainForm()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/Author/Create");
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("FullName", content);
            Assert.Contains("Affiliation", content);
            Assert.Contains("Email", content);
        }
    }
}




