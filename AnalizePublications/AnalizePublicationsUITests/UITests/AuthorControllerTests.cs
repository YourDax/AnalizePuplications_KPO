using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http;
using Xunit;

namespace AnalizePublicationsUITests.UITests
{
    /// <summary>
    /// UI тесты для контроллера Author
    /// </summary>
    public class AuthorControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public AuthorControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        #region Просмотр и навигация

        [Fact]
        public async Task AuthorIndex_ShouldReturnSuccessStatusCode()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/Author");
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task AuthorBrowse_ShouldReturnSuccessStatusCode()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/Author/Browse");
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData("FullName", "asc")]
        [InlineData("FullName", "desc")]
        [InlineData("Affiliation", "asc")]
        [InlineData("Email", "desc")]
        [InlineData("PublicationsCount", "asc")]
        [InlineData("CitationCount", "desc")]
        [InlineData("HIndex", "asc")]
        [InlineData("I10Index", "desc")]
        [InlineData("ProductivityIndex", "asc")]
        public async Task AuthorBrowse_ShouldSupportSorting(string sortBy, string sortOrder)
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync($"/Author/Browse?sortBy={sortBy}&sortOrder={sortOrder}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("Авторы", content);
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
        public async Task AuthorSearch_ShouldFilterByFullName()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/Author/Search?fullName=Test");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("Поиск", content);
        }

        [Fact]
        public async Task AuthorDetails_ShouldReturnSuccessStatusCode()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/Author/Details/1");
            Assert.True(response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task AuthorDetails_WithReport_ShouldReturnSuccessStatusCode()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/Author/Details/1?generateReport=true");
            Assert.True(response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NotFound);
        }

        #endregion

        #region CRUD операции

        [Fact]
        public async Task AuthorCreate_Get_ShouldReturnSuccessStatusCode()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/Author/Create");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("Полное имя", content);
            Assert.Contains("Организация", content);
            Assert.Contains("Email", content);
        }

        [Fact]
        public async Task AuthorEdit_Get_ShouldReturnSuccessOrNotFound()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/Author/Edit/1");
            Assert.True(response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task AuthorDelete_Get_ShouldReturnSuccessOrNotFound()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/Author/Delete/1");
            Assert.True(response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NotFound);
        }

        #endregion

        #region Экспорт отчетов

        [Fact]
        public async Task AuthorExportReport_Pdf_ShouldReturnFile()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/Author/ExportReport/1?format=pdf");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Assert.Equal("application/pdf", response.Content.Headers.ContentType?.MediaType);
            }
            else
            {
                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            }
        }

        [Fact]
        public async Task AuthorExportReport_Word_ShouldReturnFile()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/Author/ExportReport/1?format=word");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var contentType = response.Content.Headers.ContentType?.MediaType;
                Assert.True(contentType?.Contains("wordprocessingml") == true || contentType?.Contains("msword") == true);
            }
            else
            {
                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            }
        }

        [Fact]
        public async Task AuthorExportReport_Excel_ShouldReturnFile()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/Author/ExportReport/1?format=excel");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Assert.True(response.Content.Headers.ContentType?.MediaType?.Contains("spreadsheetml") == true);
            }
            else
            {
                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            }
        }

        [Fact]
        public async Task AuthorExportReport_InvalidFormat_ShouldReturnBadRequest()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/Author/ExportReport/1?format=invalid");
            Assert.True(response.StatusCode == HttpStatusCode.BadRequest || 
                       response.StatusCode == HttpStatusCode.NotFound);
        }

        #endregion
    }
}




