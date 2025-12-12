using AnalizePuplicationsContracts.BusinessLogicContracts;
using AnalyzePublicationsContracts.BindingModels;
using AnalyzePublicationsContracts.SearchModels;
using AnalyzePublicationsContracts.StorageContracts;
using AnalyzePublicationsContracts.ViewModels;
using Microsoft.Extensions.Logging;

namespace AnalizePuplicationsBusinessLogic.BusinessLogic
{
    public class SourceLogic : ISourceLogic
    {
        private readonly ILogger _logger;
        private readonly ISourceStorage _sourceStorage;

        public SourceLogic(ILogger<SourceLogic> logger, ISourceStorage sourceStorage)
        {
            _logger = logger;
            _sourceStorage = sourceStorage;
        }

        public List<SourceViewModel>? ReadList(SourceSearchModel? model)
        {
            _logger.LogInformation("ReadList. Type:{Type}. Id:{Id}", model?.SourceType, model?.Id);

            var list = model == null
                ? _sourceStorage.GetFullList()
                : _sourceStorage.GetFilteredList(model);

            if (list == null)
            {
                _logger.LogWarning("ReadList returned null");
                return null;
            }

            _logger.LogInformation("ReadList. Count:{Count}", list.Count);
            return list;
        }

        public SourceViewModel? ReadElement(SourceSearchModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            _logger.LogInformation("ReadElement. Type:{Type}, Id:{Id}", model.SourceType, model.Id);

            var element = _sourceStorage.GetElement(model);
            if (element == null)
            {
                _logger.LogWarning("Source not found");
                return null;
            }

            return element;
        }

        public bool Create(SourceBindingModel model)
        {
            CheckModel(model);
            if (_sourceStorage.Insert(model) == null)
            {
                _logger.LogWarning("Insert failed");
                return false;
            }
            return true;
        }

        public bool Update(SourceBindingModel model)
        {
            CheckModel(model);
            if (_sourceStorage.Update(model) == null)
            {
                _logger.LogWarning("Update failed");
                return false;
            }
            return true;
        }

        public bool Delete(SourceBindingModel model)
        {
            CheckModel(model, false);
            if (_sourceStorage.Delete(model) == null)
            {
                _logger.LogWarning("Delete failed");
                return false;
            }
            return true;
        }

        private void CheckModel(SourceBindingModel model, bool withParams = true)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (!withParams)
                return;

            if (string.IsNullOrEmpty(model.Url))
                throw new ArgumentNullException(nameof(model.Url), "Не указана ссылка источника");

            _logger.LogInformation("Check source model. Type:{Type}, Link:{Link}, Id:{Id}",
                model.SourceType, model.Url, model.Id);
        }
    }
}
