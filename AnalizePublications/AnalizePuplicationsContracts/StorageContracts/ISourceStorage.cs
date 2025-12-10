using AnalyzePublicationsContracts.BindingModels;
using AnalyzePublicationsContracts.SearchModels;
using AnalyzePublicationsContracts.ViewModels;

namespace AnalyzePublicationsContracts.StorageContracts
{
    public interface ISourceStorage
    {
        List<SourceViewModel> GetFullList();

        List<SourceViewModel> GetFilteredList(SourceSearchModel model);

        SourceViewModel? GetElement(SourceSearchModel model);

        SourceViewModel? Insert(SourceBindingModel model);

        SourceViewModel? Update(SourceBindingModel model);

        SourceViewModel? Delete(SourceBindingModel model);
    }
}
