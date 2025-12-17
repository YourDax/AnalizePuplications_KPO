using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http;
using Xunit;

namespace AnalizePublicationsUITests.UITests
{
    /// <summary>
    /// Интеграционные UI тесты для проверки полного цикла работы приложения
    /// </summary>
    public class IntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public IntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task HomePage_ShouldHaveNavigationLinks()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/");
            var content = await response.Content.ReadAsStringAsync();

            Assert.Contains("Просмотр и поиск", content);
            Assert.Contains("Редактирование", content);
        }

        [Fact]
        public async Task HomePage_ShouldHaveEntityCards()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/");
            var content = await response.Content.ReadAsStringAsync();

            Assert.Contains("Авторы", content);
            Assert.Contains("Публикации", content);
            Assert.Contains("Источники", content);
        }

        [Fact]
        public async Task Navigation_ShouldWorkForAllEntities()
        {
            var client = _factory.CreateClient();
            
            var authorBrowse = await client.GetAsync("/Author/Browse");
            authorBrowse.EnsureSuccessStatusCode();
            
            var authorSearch = await client.GetAsync("/Author/Search");
            authorSearch.EnsureSuccessStatusCode();
            
            var publicationBrowse = await client.GetAsync("/Publication/Browse");
            publicationBrowse.EnsureSuccessStatusCode();
            
            var publicationSearch = await client.GetAsync("/Publication/Search");
            publicationSearch.EnsureSuccessStatusCode();
            
            var sourceBrowse = await client.GetAsync("/Source/Browse");
            sourceBrowse.EnsureSuccessStatusCode();
            
            var sourceSearch = await client.GetAsync("/Source/Search");
            sourceSearch.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task AllManagementPages_ShouldBeAccessible()
        {
            var client = _factory.CreateClient();
            
            var authorIndex = await client.GetAsync("/Author");
            authorIndex.EnsureSuccessStatusCode();
            
            var publicationIndex = await client.GetAsync("/Publication");
            publicationIndex.EnsureSuccessStatusCode();
            
            var sourceIndex = await client.GetAsync("/Source");
            sourceIndex.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task AllCreatePages_ShouldBeAccessible()
        {
            var client = _factory.CreateClient();
            
            var authorCreate = await client.GetAsync("/Author/Create");
            authorCreate.EnsureSuccessStatusCode();
            
            var publicationCreate = await client.GetAsync("/Publication/Create");
            publicationCreate.EnsureSuccessStatusCode();
            
            var sourceCreate = await client.GetAsync("/Source/Create");
            sourceCreate.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Sorting_ShouldWorkForAllEntities()
        {
            var client = _factory.CreateClient();
            
            var authorAsc = await client.GetAsync("/Author/Browse?sortBy=FullName&sortOrder=asc");
            authorAsc.EnsureSuccessStatusCode();
            
            var authorDesc = await client.GetAsync("/Author/Browse?sortBy=FullName&sortOrder=desc");
            authorDesc.EnsureSuccessStatusCode();
            
            var pubAsc = await client.GetAsync("/Publication/Browse?sortBy=Title&sortOrder=asc");
            pubAsc.EnsureSuccessStatusCode();
            
            var pubDesc = await client.GetAsync("/Publication/Browse?sortBy=Title&sortOrder=desc");
            pubDesc.EnsureSuccessStatusCode();
            
            var sourceAsc = await client.GetAsync("/Source/Browse?sortBy=Name&sortOrder=asc");
            sourceAsc.EnsureSuccessStatusCode();
            
            var sourceDesc = await client.GetAsync("/Source/Browse?sortBy=Name&sortOrder=desc");
            sourceDesc.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task SearchPages_ShouldContainSearchForms()
        {
            var client = _factory.CreateClient();
            
            var authorSearch = await client.GetAsync("/Author/Search");
            var authorContent = await authorSearch.Content.ReadAsStringAsync();
            Assert.Contains("form", authorContent.ToLower());
            
            var publicationSearch = await client.GetAsync("/Publication/Search");
            var publicationContent = await publicationSearch.Content.ReadAsStringAsync();
            Assert.Contains("form", publicationContent.ToLower());
            
            var sourceSearch = await client.GetAsync("/Source/Search");
            var sourceContent = await sourceSearch.Content.ReadAsStringAsync();
            Assert.Contains("form", sourceContent.ToLower());
        }
    }
}




