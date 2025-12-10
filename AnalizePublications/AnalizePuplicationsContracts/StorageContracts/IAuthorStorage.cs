using AnalyzePublicationsContracts.BindingModels;
using AnalyzePublicationsContracts.SearchModels;
using AnalyzePublicationsContracts.ViewModels;

namespace AnalyzePublicationsContracts.StorageContracts
{
    public interface IAuthorStorage
    {
        List<AuthorViewModel> GetFullList();
        List<AuthorViewModel> GetFilteredList(AuthorSearchModel model);
        AuthorViewModel? GetElement(AuthorSearchModel model);
        AuthorViewModel? Insert(AuthorBindingModel model);
        AuthorViewModel? Update(AuthorBindingModel model);
        AuthorViewModel? Delete(AuthorBindingModel model);
    }
}
