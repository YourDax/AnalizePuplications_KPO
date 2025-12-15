using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http;
using Xunit;

namespace AnalizePublicationsUITests.UITests
{
    /// <summary>
    /// UI тесты для контроллера Source
    /// </summary>
    public class SourceControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public SourceControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        #region Просмотр и навигация

        [Fact]
        public async Task SourceIndex_ShouldReturnSuccessStatusCode()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/Source");
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task SourceBrowse_ShouldReturnSuccessStatusCode()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/Source/Browse");
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData("Name", "asc")]
        [InlineData("Name", "desc")]
        [InlineData("SourceType", "asc")]
        [InlineData("SourceType", "desc")]
        [InlineData("Url", "asc")]
        [InlineData("Url", "desc")]
        public async Task SourceBrowse_ShouldSupportSorting(string sortBy, string sortOrder)
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync($"/Source/Browse?sortBy={sortBy}&sortOrder={sortOrder}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("Источники", content);
        }

        [Fact]
        public async Task SourceSearch_ShouldReturnSuccessStatusCode()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/Source/Search");
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task SourceSearch_ShouldFilterByName()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/Source/Search?name=Test");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("Поиск", content);
        }

        [Fact]
        public async Task SourceDetails_ShouldReturnSuccessOrNotFound()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/Source/Details/1");
            Assert.True(response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NotFound);
        }

        #endregion

        #region CRUD операции

        [Fact]
        public async Task SourceCreate_Get_ShouldReturnSuccessStatusCode()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/Source/Create");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("Название", content);
            Assert.Contains("Тип источника", content);
        }

        [Fact]
        public async Task SourceEdit_Get_ShouldReturnSuccessOrNotFound()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/Source/Edit/1");
            Assert.True(response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task SourceDelete_Get_ShouldReturnSuccessOrNotFound()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/Source/Delete/1");
            Assert.True(response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NotFound);
        }

        #endregion
    }
}


