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
    public interface ISourceLogic
    {
        List<SourceViewModel>? ReadList(SourceSearchModel? model);
        SourceViewModel? ReadElement(SourceSearchModel model);
        bool Create(SourceBindingModel model);
        bool Update(SourceBindingModel model);
        bool Delete(SourceBindingModel model);
    }
}
