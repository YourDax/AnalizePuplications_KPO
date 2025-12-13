using AnalyzePublicationsDataModels.Models;
using AnalyzePublicationsDataModels.Enums;

namespace AnalyzePublicationsContracts.BindingModels
{
    public class PublicationBindingModel : IPublicationModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime? PublicationDate { get; set; }
        public PublicationType PublicationType { get; set; }
        public int SourceId { get; set; }
        public string? Doi { get; set; }
        public int? Pages { get; set; }
        public string? Abstract { get; set; }
        public string? Keywords { get; set; }
        public int CitationCount { get; set; }

        // Для связи many-to-many с вкладом авторов
        public List<PublicationAuthorBindingModel> Authors { get; set; } = new();
    }
}
