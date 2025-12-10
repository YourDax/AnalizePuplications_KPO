using AnalyzePublicationsDataModels.Enums;
using AnalyzePublicationsDataModels.Models;

namespace AnalyzePublicationsContracts.ViewModels
{
    public class SourceViewModel : ISourceModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public SourceType SourceType { get; set; }

        public string? Url { get; set; }
    }
}
