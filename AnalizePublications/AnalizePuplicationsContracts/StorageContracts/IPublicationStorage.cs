using AnalyzePublicationsContracts.BindingModels;
using AnalyzePublicationsContracts.SearchModels;
using AnalyzePublicationsContracts.ViewModels;

public interface IPublicationStorage
{
    List<PublicationViewModel> GetFullList();
    List<PublicationViewModel> GetFilteredList(PublicationSearchModel model);
    PublicationViewModel? GetElement(PublicationSearchModel model);
    PublicationViewModel? Insert(PublicationBindingModel model);
    PublicationViewModel? Update(PublicationBindingModel model);
    PublicationViewModel? Delete(PublicationBindingModel model);
    List<PublicationViewModel> GetPublicationsByAuthor(int authorId);
}
