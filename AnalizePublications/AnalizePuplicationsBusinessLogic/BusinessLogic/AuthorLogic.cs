using AnalizePuplicationsContracts.BusinessLogicContracts;
using AnalyzePublicationsContracts.BindingModels;
using AnalyzePublicationsContracts.SearchModels;
using AnalyzePublicationsContracts.StorageContracts;
using AnalyzePublicationsContracts.ViewModels;
using Microsoft.Extensions.Logging;

namespace AnalizePuplicationsBusinessLogic.BusinessLogic
{
    public class AuthorLogic : IAuthorLogic
    {
        private readonly ILogger _logger;
        private readonly IAuthorStorage _authorStorage;

        public AuthorLogic(ILogger<AuthorLogic> logger, IAuthorStorage authorStorage)
        {
            _logger = logger;
            _authorStorage = authorStorage;
        }

        public List<AuthorViewModel>? ReadList(AuthorSearchModel? model)
        {
            _logger.LogInformation(
                "ReadList. FullName:{FullName}. Id:{Id}",
                model?.FullName, model?.Id
            );

            var list = model == null
                ? _authorStorage.GetFullList()
                : _authorStorage.GetFilteredList(model);

            if (list == null)
            {
                _logger.LogWarning("ReadList returned null");
                return null;
            }

            _logger.LogInformation("ReadList. Count:{Count}", list.Count);
            return list;
        }

        public AuthorViewModel? ReadElement(AuthorSearchModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            _logger.LogInformation("ReadElement. FullName:{FullName}, Id:{Id}",
                model.FullName, model.Id);

            var element = _authorStorage.GetElement(model);
            if (element == null)
            {
                _logger.LogWarning("ReadElement: element not found");
                return null;
            }

            return element;
        }

        public bool Create(AuthorBindingModel model)
        {
            CheckModel(model);
            if (_authorStorage.Insert(model) == null)
            {
                _logger.LogWarning("Create operation failed");
                return false;
            }
            return true;
        }

        public bool Update(AuthorBindingModel model)
        {
            CheckModel(model);
            if (_authorStorage.Update(model) == null)
            {
                _logger.LogWarning("Update operation failed");
                return false;
            }
            return true;
        }

        public bool Delete(AuthorBindingModel model)
        {
            CheckModel(model, false);
            if (_authorStorage.Delete(model) == null)
            {
                _logger.LogWarning("Delete operation failed");
                return false;
            }
            return true;
        }

        private void CheckModel(AuthorBindingModel model, bool withParams = true)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (!withParams)
                return;

            if (string.IsNullOrEmpty(model.FullName))
                throw new ArgumentNullException(nameof(model.FullName), "Не заполнено имя автора");

            _logger.LogInformation("Checking author model. FullName:{FullName}, Id:{Id}",
                model.FullName, model.Id);

            var existing = _authorStorage.GetElement(new AuthorSearchModel
            {
                FullName = model.FullName
            });

            if (existing != null && existing.Id != model.Id)
                throw new InvalidOperationException("Автор с таким именем уже существует");
        }
    }
}
