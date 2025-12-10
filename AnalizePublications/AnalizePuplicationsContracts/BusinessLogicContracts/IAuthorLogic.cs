using AnalyzePublicationsContracts.BindingModels;
using AnalyzePublicationsContracts.SearchModels;
using AnalyzePublicationsContracts.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalizePuplicationsContracts.BusinessLogicContracts
{
    public interface IAuthorLogic
    {
        List<AuthorViewModel>? ReadList(AuthorSearchModel? model);
        AuthorViewModel? ReadElement(AuthorSearchModel model);
        bool Create(AuthorBindingModel model);
        bool Update(AuthorBindingModel model);
        bool Delete(AuthorBindingModel model);
    }
}
