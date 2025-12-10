using AnalyzePublicationsDataModels.Models;
using AnalyzePublicationsDataModels.Enums;

namespace AnalyzePublicationsContracts.BindingModels
{
    public class PublicationBindingModel : IPublicationModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int Year { get; set; }
        public PublicationType PublicationType { get; set; }
        public int SourceId { get; set; }
        public string? Doi { get; set; }
        public int CitationCount { get; set; }
    }
}
