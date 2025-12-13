using AnalizePuplicationsContracts.BusinessLogicContracts;
using AnalyzePublicationsContracts.BindingModels;
using AnalyzePublicationsContracts.SearchModels;
using AnalyzePublicationsContracts.StorageContracts;
using AnalyzePublicationsContracts.ViewModels;
using Microsoft.Extensions.Logging;

namespace AnalizePuplicationsBusinessLogic.BusinessLogic
{
    public class PublicationLogic : IPublicationLogic
    {
        private readonly ILogger _logger;
        private readonly IPublicationStorage _publicationStorage;

        public PublicationLogic(ILogger<PublicationLogic> logger, IPublicationStorage publicationStorage)
        {
            _logger = logger;
            _publicationStorage = publicationStorage;
        }

        public List<PublicationViewModel>? ReadList(PublicationSearchModel? model)
        {
            _logger.LogInformation("ReadList. Title:{Title}. Id:{Id}",
                model?.Title, model?.Id);

            var list = model == null
                ? _publicationStorage.GetFullList()
                : _publicationStorage.GetFilteredList(model);

            if (list == null)
            {
                _logger.LogWarning("ReadList returned null");
                return null;
            }

            _logger.LogInformation("ReadList. Count:{Count}", list.Count);
            return list;
        }

        public PublicationViewModel? ReadElement(PublicationSearchModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            _logger.LogInformation("ReadElement. Title:{Title}, Id:{Id}",
                model.Title, model.Id);

            var element = _publicationStorage.GetElement(model);
            if (element == null)
            {
                _logger.LogWarning("Publication not found");
                return null;
            }

            return element;
        }

        public bool Create(PublicationBindingModel model)
        {
            CheckModel(model);
            if (_publicationStorage.Insert(model) == null)
            {
                _logger.LogWarning("Insert failed");
                return false;
            }
            return true;
        }

        public bool Update(PublicationBindingModel model)
        {
            CheckModel(model);
            if (_publicationStorage.Update(model) == null)
            {
                _logger.LogWarning("Update failed");
                return false;
            }
            return true;
        }

        public bool Delete(PublicationBindingModel model)
        {
            CheckModel(model, false);
            if (_publicationStorage.Delete(model) == null)
            {
                _logger.LogWarning("Delete failed");
                return false;
            }
            return true;
        }

        public List<PublicationViewModel>? ReadListByAuthor(int authorId)
        {
            _logger.LogInformation("ReadListByAuthor. AuthorId:{AuthorId}", authorId);

            var list = _publicationStorage.GetPublicationsByAuthor(authorId);

            if (list == null)
            {
                _logger.LogWarning("Author publications not found");
                return null;
            }

            _logger.LogInformation("ReadListByAuthor. Count:{Count}", list.Count);
            return list;
        }

        private void CheckModel(PublicationBindingModel model, bool withParams = true)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (!withParams)
                return;

            if (string.IsNullOrEmpty(model.Title))
                throw new ArgumentNullException(nameof(model.Title), "Не указано название публикации");

            if (model.Authors == null || model.Authors.Count == 0)
                throw new ArgumentException("Публикация должна иметь хотя бы одного автора");

            if (model.SourceId <= 0)
                throw new ArgumentException("Некорректный SourceId");

            _logger.LogInformation("Publication check. Title:{Title}, Authors:{Authors}, SourceId:{SourceId}, Id:{Id}",
                 model.Title, string.Join(",", model.Authors.Select(a => a.AuthorId)), model.SourceId, model.Id);
        }
    }
}
