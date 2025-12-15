using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http;
using Xunit;

namespace AnalizePublicationsUITests.UITests
{
    /// <summary>
    /// UI тесты для страниц публикаций
    /// </summary>
    public class PublicationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public PublicationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task PublicationBrowse_ShouldReturnSuccessStatusCode()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/Publication/Browse");
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task PublicationSearch_ShouldReturnSuccessStatusCode()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/Publication/Search");
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task PublicationIndex_ShouldReturnSuccessStatusCode()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/Publication");
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task PublicationCreate_ShouldReturnSuccessStatusCode()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/Publication/Create");
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}


