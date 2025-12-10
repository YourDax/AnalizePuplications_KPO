using AnalyzePublicationsContracts.BindingModels;
using AnalyzePublicationsContracts.SearchModels;
using AnalyzePublicationsContracts.ViewModels;

namespace AnalizePuplicationsContracts.BusinessLogicContracts
{
    public interface IPublicationLogic
    {
        List<PublicationViewModel>? ReadList(PublicationSearchModel? model);
        PublicationViewModel? ReadElement(PublicationSearchModel model);
        bool Create(PublicationBindingModel model);
        bool Update(PublicationBindingModel model);
        bool Delete(PublicationBindingModel model);

        /// <summary>
        /// Получить список публикаций по конкретному автору
        /// включая данные об источниках
        /// </summary>
        List<PublicationViewModel>? ReadListByAuthor(int authorId);
    }
}
