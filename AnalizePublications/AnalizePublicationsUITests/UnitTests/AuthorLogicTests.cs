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
    /// Модульные тесты для AuthorLogic
    /// </summary>
    public class AuthorLogicTests
    {
        private readonly Mock<ILogger<AuthorLogic>> _loggerMock;
        private readonly Mock<IAuthorStorage> _storageMock;
        private readonly AuthorLogic _authorLogic;

        public AuthorLogicTests()
        {
            _loggerMock = new Mock<ILogger<AuthorLogic>>();
            _storageMock = new Mock<IAuthorStorage>();
            _authorLogic = new AuthorLogic(_loggerMock.Object, _storageMock.Object);
        }

        [Fact]
        public void ReadList_WithNullModel_ShouldCallGetFullList()
        {
            // Arrange
            var expectedList = new List<AuthorViewModel>
            {
                new AuthorViewModel { Id = 1, FullName = "Test Author" }
            };
            _storageMock.Setup(s => s.GetFullList()).Returns(expectedList);

            // Act
            var result = _authorLogic.ReadList(null);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            _storageMock.Verify(s => s.GetFullList(), Times.Once);
            _storageMock.Verify(s => s.GetFilteredList(It.IsAny<AuthorSearchModel>()), Times.Never);
        }

        [Fact]
        public void ReadList_WithSearchModel_ShouldCallGetFilteredList()
        {
            // Arrange
            var searchModel = new AuthorSearchModel { FullName = "Test" };
            var expectedList = new List<AuthorViewModel>
            {
                new AuthorViewModel { Id = 1, FullName = "Test Author" }
            };
            _storageMock.Setup(s => s.GetFilteredList(searchModel)).Returns(expectedList);

            // Act
            var result = _authorLogic.ReadList(searchModel);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            _storageMock.Verify(s => s.GetFilteredList(searchModel), Times.Once);
            _storageMock.Verify(s => s.GetFullList(), Times.Never);
        }

        [Fact]
        public void ReadList_WhenStorageReturnsNull_ShouldReturnNull()
        {
            // Arrange
            _storageMock.Setup(s => s.GetFullList()).Returns((List<AuthorViewModel>?)null);

            // Act
            var result = _authorLogic.ReadList(null);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void ReadElement_WithNullModel_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _authorLogic.ReadElement(null!));
        }

        [Fact]
        public void ReadElement_WithValidModel_ShouldReturnViewModel()
        {
            // Arrange
            var searchModel = new AuthorSearchModel { Id = 1 };
            var expectedViewModel = new AuthorViewModel { Id = 1, FullName = "Test Author" };
            _storageMock.Setup(s => s.GetElement(searchModel)).Returns(expectedViewModel);

            // Act
            var result = _authorLogic.ReadElement(searchModel);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedViewModel.Id, result.Id);
            Assert.Equal(expectedViewModel.FullName, result.FullName);
            _storageMock.Verify(s => s.GetElement(searchModel), Times.Once);
        }

        [Fact]
        public void Create_WithValidModel_ShouldReturnTrue()
        {
            // Arrange
            var model = new AuthorBindingModel
            {
                FullName = "Test Author",
                Affiliation = "Test Affiliation",
                Email = "test@example.com"
            };
            var expectedViewModel = new AuthorViewModel { Id = 1, FullName = "Test Author" };
            // Мокируем проверку существования автора (не найден)
            _storageMock.Setup(s => s.GetElement(It.IsAny<AuthorSearchModel>())).Returns((AuthorViewModel?)null);
            _storageMock.Setup(s => s.Insert(model)).Returns(expectedViewModel);

            // Act
            var result = _authorLogic.Create(model);

            // Assert
            Assert.True(result);
            _storageMock.Verify(s => s.Insert(model), Times.Once);
        }

        [Fact]
        public void Create_WithInvalidModel_ShouldThrowException()
        {
            // Arrange
            var model = new AuthorBindingModel { FullName = "" }; // Invalid: empty name

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _authorLogic.Create(model));
        }

        [Fact]
        public void Create_WithDuplicateName_ShouldThrowException()
        {
            // Arrange
            var model = new AuthorBindingModel
            {
                FullName = "Existing Author",
                Affiliation = "Test Affiliation",
                Email = "test@example.com"
            };
            var existingAuthor = new AuthorViewModel { Id = 2, FullName = "Existing Author" };
            // Мокируем проверку существования автора (найден другой автор)
            _storageMock.Setup(s => s.GetElement(It.Is<AuthorSearchModel>(m => m.FullName == "Existing Author")))
                .Returns(existingAuthor);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => _authorLogic.Create(model));
        }

        [Fact]
        public void Create_WhenStorageReturnsNull_ShouldReturnFalse()
        {
            // Arrange
            var model = new AuthorBindingModel
            {
                FullName = "Test Author",
                Affiliation = "Test Affiliation",
                Email = "test@example.com"
            };
            _storageMock.Setup(s => s.Insert(model)).Returns((AuthorViewModel?)null);

            // Act
            var result = _authorLogic.Create(model);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Update_WithValidModel_ShouldReturnTrue()
        {
            // Arrange
            var model = new AuthorBindingModel
            {
                Id = 1,
                FullName = "Updated Author",
                Affiliation = "Updated Affiliation",
                Email = "updated@example.com"
            };
            var expectedViewModel = new AuthorViewModel { Id = 1, FullName = "Updated Author" };
            // Мокируем проверку существования автора (не найден или тот же автор)
            _storageMock.Setup(s => s.GetElement(It.IsAny<AuthorSearchModel>())).Returns((AuthorViewModel?)null);
            _storageMock.Setup(s => s.Update(model)).Returns(expectedViewModel);

            // Act
            var result = _authorLogic.Update(model);

            // Assert
            Assert.True(result);
            _storageMock.Verify(s => s.Update(model), Times.Once);
        }

        [Fact]
        public void Delete_WithValidModel_ShouldReturnTrue()
        {
            // Arrange
            var model = new AuthorBindingModel { Id = 1 };
            var expectedViewModel = new AuthorViewModel { Id = 1 };
            _storageMock.Setup(s => s.Delete(model)).Returns(expectedViewModel);

            // Act
            var result = _authorLogic.Delete(model);

            // Assert
            Assert.True(result);
            _storageMock.Verify(s => s.Delete(model), Times.Once);
        }
    }
}

