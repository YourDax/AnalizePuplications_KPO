using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http;
using Xunit;

namespace AnalizePublicationsUITests.UITests
{
    /// <summary>
    /// UI тесты для проверки содержимого страниц
    /// </summary>
    public class ContentTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public ContentTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task AuthorBrowse_ShouldContainTableHeaders()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/Author/Browse");
            var content = await response.Content.ReadAsStringAsync();

            Assert.Contains("Полное имя", content);
            Assert.Contains("Организация", content);
            Assert.Contains("Email", content);
        }

        [Fact]
        public async Task AuthorBrowse_ShouldContainMetrics()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/Author/Browse");
            var content = await response.Content.ReadAsStringAsync();

            Assert.Contains("Публикаций", content);
            Assert.Contains("H-индекс", content);
            Assert.Contains("Цитирований", content);
        }

        [Fact]
        public async Task PublicationBrowse_ShouldContainTableHeaders()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/Publication/Browse");
            var content = await response.Content.ReadAsStringAsync();

            Assert.Contains("Название", content);
            Assert.Contains("Дата публикации", content);
            Assert.Contains("Тип", content);
        }

        [Fact]
        public async Task PublicationCreate_ShouldContainAllFields()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/Publication/Create");
            var content = await response.Content.ReadAsStringAsync();

            Assert.Contains("Дата публикации", content);
            Assert.Contains("Тип публикации", content);
            Assert.Contains("Количество страниц", content);
            Assert.Contains("Аннотация", content);
            Assert.Contains("Ключевые слова", content);
            Assert.Contains("DOI", content);
            Assert.Contains("Количество цитирований", content);
            Assert.Contains("Авторы", content);
        }

        [Fact]
        public async Task SourceBrowse_ShouldContainTableHeaders()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/Source/Browse");
            var content = await response.Content.ReadAsStringAsync();

            Assert.Contains("Название", content);
            Assert.Contains("Тип источника", content);
            Assert.Contains("URL", content);
        }

        [Fact]
        public async Task AuthorDetails_ShouldContainMetricsWithTooltips()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/Author/Details/1");
            
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                Assert.Contains("H-индекс", content);
                Assert.Contains("i10-индекс", content);
                Assert.Contains("Индекс продуктивности", content);
                Assert.Contains("data-bs-toggle=\"tooltip\"", content);
            }
        }
    }
}


