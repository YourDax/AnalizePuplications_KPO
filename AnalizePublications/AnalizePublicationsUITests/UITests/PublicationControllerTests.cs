using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http;
using Xunit;

namespace AnalizePublicationsUITests.UITests
{
    /// <summary>
    /// UI тесты для контроллера Publication
    /// </summary>
    public class PublicationControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public PublicationControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        #region Просмотр и навигация

        [Fact]
        public async Task PublicationIndex_ShouldReturnSuccessStatusCode()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/Publication");
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task PublicationBrowse_ShouldReturnSuccessStatusCode()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/Publication/Browse");
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData("Title", "asc")]
        [InlineData("Title", "desc")]
        [InlineData("PublicationDate", "asc")]
        [InlineData("PublicationDate", "desc")]
        [InlineData("PublicationType", "asc")]
        [InlineData("Pages", "desc")]
        [InlineData("CitationCount", "asc")]
        public async Task PublicationBrowse_ShouldSupportSorting(string sortBy, string sortOrder)
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync($"/Publication/Browse?sortBy={sortBy}&sortOrder={sortOrder}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("Публикации", content);
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
        public async Task PublicationSearch_ShouldFilterByTitle()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/Publication/Search?title=Test");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("Поиск", content);
        }

        [Fact]
        public async Task PublicationSearch_ShouldFilterByDateRange()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/Publication/Search?dateFrom=2020-01-01&dateTo=2024-12-31");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("Поиск", content);
        }

        [Fact]
        public async Task PublicationDetails_ShouldReturnSuccessOrNotFound()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/Publication/Details/1");
            Assert.True(response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NotFound);
        }

        #endregion

        #region CRUD операции

        [Fact]
        public async Task PublicationCreate_Get_ShouldReturnSuccessStatusCode()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/Publication/Create");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("Title", content);
            Assert.Contains("Дата публикации", content);
            Assert.Contains("Тип публикации", content);
            Assert.Contains("Источник", content);
        }

        [Fact]
        public async Task PublicationEdit_Get_ShouldReturnSuccessOrNotFound()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/Publication/Edit/1");
            Assert.True(response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task PublicationDelete_Get_ShouldReturnSuccessOrNotFound()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/Publication/Delete/1");
            Assert.True(response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NotFound);
        }

        #endregion
    }
}


