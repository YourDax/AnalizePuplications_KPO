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
    /// Модульные тесты для SourceLogic
    /// </summary>
    public class SourceLogicTests
    {
        private readonly Mock<ILogger<SourceLogic>> _loggerMock;
        private readonly Mock<ISourceStorage> _storageMock;
        private readonly SourceLogic _sourceLogic;

        public SourceLogicTests()
        {
            _loggerMock = new Mock<ILogger<SourceLogic>>();
            _storageMock = new Mock<ISourceStorage>();
            _sourceLogic = new SourceLogic(_loggerMock.Object, _storageMock.Object);
        }

        [Fact]
        public void ReadList_WithNullModel_ShouldCallGetFullList()
        {
            // Arrange
            var expectedList = new List<SourceViewModel>
            {
                new SourceViewModel { Id = 1, Name = "Test Source" }
            };
            _storageMock.Setup(s => s.GetFullList()).Returns(expectedList);

            // Act
            var result = _sourceLogic.ReadList(null);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            _storageMock.Verify(s => s.GetFullList(), Times.Once);
        }

        [Fact]
        public void ReadList_WithSearchModel_ShouldCallGetFilteredList()
        {
            // Arrange
            var searchModel = new SourceSearchModel { Name = "Test" };
            var expectedList = new List<SourceViewModel>
            {
                new SourceViewModel { Id = 1, Name = "Test Source" }
            };
            _storageMock.Setup(s => s.GetFilteredList(searchModel)).Returns(expectedList);

            // Act
            var result = _sourceLogic.ReadList(searchModel);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            _storageMock.Verify(s => s.GetFilteredList(searchModel), Times.Once);
        }

        [Fact]
        public void ReadElement_WithNullModel_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _sourceLogic.ReadElement(null!));
        }

        [Fact]
        public void ReadElement_WithValidModel_ShouldReturnViewModel()
        {
            // Arrange
            var searchModel = new SourceSearchModel { Id = 1 };
            var expectedViewModel = new SourceViewModel { Id = 1, Name = "Test Source" };
            _storageMock.Setup(s => s.GetElement(searchModel)).Returns(expectedViewModel);

            // Act
            var result = _sourceLogic.ReadElement(searchModel);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedViewModel.Id, result.Id);
            Assert.Equal(expectedViewModel.Name, result.Name);
            _storageMock.Verify(s => s.GetElement(searchModel), Times.Once);
        }

        [Fact]
        public void Create_WithValidModel_ShouldReturnTrue()
        {
            // Arrange
            var model = new SourceBindingModel
            {
                Name = "Test Source",
                Url = "https://example.com"
            };
            var expectedViewModel = new SourceViewModel { Id = 1, Name = "Test Source" };
            _storageMock.Setup(s => s.Insert(model)).Returns(expectedViewModel);

            // Act
            var result = _sourceLogic.Create(model);

            // Assert
            Assert.True(result);
            _storageMock.Verify(s => s.Insert(model), Times.Once);
        }

        [Fact]
        public void Create_WithInvalidModel_ShouldThrowException()
        {
            // Arrange
            var model = new SourceBindingModel { Url = "" }; // Invalid: empty URL

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _sourceLogic.Create(model));
        }

        [Fact]
        public void Update_WithValidModel_ShouldReturnTrue()
        {
            // Arrange
            var model = new SourceBindingModel
            {
                Id = 1,
                Name = "Updated Source",
                Url = "https://updated.com"
            };
            var expectedViewModel = new SourceViewModel { Id = 1, Name = "Updated Source" };
            _storageMock.Setup(s => s.Update(model)).Returns(expectedViewModel);

            // Act
            var result = _sourceLogic.Update(model);

            // Assert
            Assert.True(result);
            _storageMock.Verify(s => s.Update(model), Times.Once);
        }

        [Fact]
        public void Delete_WithValidModel_ShouldReturnTrue()
        {
            // Arrange
            var model = new SourceBindingModel { Id = 1 };
            var expectedViewModel = new SourceViewModel { Id = 1 };
            _storageMock.Setup(s => s.Delete(model)).Returns(expectedViewModel);

            // Act
            var result = _sourceLogic.Delete(model);

            // Assert
            Assert.True(result);
            _storageMock.Verify(s => s.Delete(model), Times.Once);
        }
    }
}

