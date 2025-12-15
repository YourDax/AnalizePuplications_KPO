using AnalizePuplicationsBusinessLogic.BusinessLogic;
using AnalyzePublicationsContracts.BindingModels;
using AnalyzePublicationsContracts.SearchModels;
using AnalyzePublicationsContracts.StorageContracts;
using AnalyzePublicationsContracts.ViewModels;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace AnalizePublicationsUITests.UnitTests
{
    /// <summary>
    /// Модульные тесты для PublicationLogic
    /// </summary>
    public class PublicationLogicTests
    {
        private readonly Mock<ILogger<PublicationLogic>> _loggerMock;
        private readonly Mock<IPublicationStorage> _storageMock;
        private readonly PublicationLogic _publicationLogic;

        public PublicationLogicTests()
        {
            _loggerMock = new Mock<ILogger<PublicationLogic>>();
            _storageMock = new Mock<IPublicationStorage>();
            _publicationLogic = new PublicationLogic(_loggerMock.Object, _storageMock.Object);
        }

        [Fact]
        public void ReadList_WithNullModel_ShouldCallGetFullList()
        {
            // Arrange
            var expectedList = new List<PublicationViewModel>
            {
                new PublicationViewModel { Id = 1, Title = "Test Publication" }
            };
            _storageMock.Setup(s => s.GetFullList()).Returns(expectedList);

            // Act
            var result = _publicationLogic.ReadList(null);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            _storageMock.Verify(s => s.GetFullList(), Times.Once);
        }

        [Fact]
        public void ReadList_WithSearchModel_ShouldCallGetFilteredList()
        {
            // Arrange
            var searchModel = new PublicationSearchModel { Title = "Test" };
            var expectedList = new List<PublicationViewModel>
            {
                new PublicationViewModel { Id = 1, Title = "Test Publication" }
            };
            _storageMock.Setup(s => s.GetFilteredList(searchModel)).Returns(expectedList);

            // Act
            var result = _publicationLogic.ReadList(searchModel);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            _storageMock.Verify(s => s.GetFilteredList(searchModel), Times.Once);
        }

        [Fact]
        public void ReadElement_WithNullModel_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _publicationLogic.ReadElement(null!));
        }

        [Fact]
        public void ReadElement_WithValidModel_ShouldReturnViewModel()
        {
            // Arrange
            var searchModel = new PublicationSearchModel { Id = 1 };
            var expectedViewModel = new PublicationViewModel { Id = 1, Title = "Test Publication" };
            _storageMock.Setup(s => s.GetElement(searchModel)).Returns(expectedViewModel);

            // Act
            var result = _publicationLogic.ReadElement(searchModel);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedViewModel.Id, result.Id);
            _storageMock.Verify(s => s.GetElement(searchModel), Times.Once);
        }

        [Fact]
        public void Create_WithValidModel_ShouldReturnTrue()
        {
            // Arrange
            var model = new PublicationBindingModel
            {
                Title = "Test Publication",
                SourceId = 1,
                CitationCount = 0,
                Authors = new List<PublicationAuthorBindingModel>
                {
                    new PublicationAuthorBindingModel { AuthorId = 1 }
                }
            };
            var expectedViewModel = new PublicationViewModel { Id = 1, Title = "Test Publication" };
            _storageMock.Setup(s => s.Insert(model)).Returns(expectedViewModel);

            // Act
            var result = _publicationLogic.Create(model);

            // Assert
            Assert.True(result);
            _storageMock.Verify(s => s.Insert(model), Times.Once);
        }

        [Fact]
        public void Create_WithInvalidModel_ShouldThrowException()
        {
            // Arrange
            var model = new PublicationBindingModel { Title = "" }; // Invalid: empty title

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _publicationLogic.Create(model));
        }

        [Fact]
        public void Create_WithNoAuthors_ShouldThrowException()
        {
            // Arrange
            var model = new PublicationBindingModel
            {
                Title = "Test Publication",
                SourceId = 1,
                CitationCount = 0,
                Authors = new List<PublicationAuthorBindingModel>() // Empty list
            };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _publicationLogic.Create(model));
        }

        [Fact]
        public void Update_WithValidModel_ShouldReturnTrue()
        {
            // Arrange
            var model = new PublicationBindingModel
            {
                Id = 1,
                Title = "Updated Publication",
                SourceId = 1,
                CitationCount = 0,
                Authors = new List<PublicationAuthorBindingModel>
                {
                    new PublicationAuthorBindingModel { AuthorId = 1 }
                }
            };
            var expectedViewModel = new PublicationViewModel { Id = 1, Title = "Updated Publication" };
            _storageMock.Setup(s => s.Update(model)).Returns(expectedViewModel);

            // Act
            var result = _publicationLogic.Update(model);

            // Assert
            Assert.True(result);
            _storageMock.Verify(s => s.Update(model), Times.Once);
        }

        [Fact]
        public void Delete_WithValidModel_ShouldReturnTrue()
        {
            // Arrange
            var model = new PublicationBindingModel { Id = 1 };
            var expectedViewModel = new PublicationViewModel { Id = 1 };
            _storageMock.Setup(s => s.Delete(model)).Returns(expectedViewModel);

            // Act
            var result = _publicationLogic.Delete(model);

            // Assert
            Assert.True(result);
            _storageMock.Verify(s => s.Delete(model), Times.Once);
        }

        [Fact]
        public void ReadListByAuthor_WithValidAuthorId_ShouldReturnList()
        {
            // Arrange
            var authorId = 1;
            var expectedList = new List<PublicationViewModel>
            {
                new PublicationViewModel { Id = 1, Title = "Publication 1" },
                new PublicationViewModel { Id = 2, Title = "Publication 2" }
            };
            _storageMock.Setup(s => s.GetPublicationsByAuthor(authorId)).Returns(expectedList);

            // Act
            var result = _publicationLogic.ReadListByAuthor(authorId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            _storageMock.Verify(s => s.GetPublicationsByAuthor(authorId), Times.Once);
        }
    }
}

