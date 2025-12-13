using AnalyzePublicationsDataModels.Models;

namespace AnalyzePublicationsContracts.BindingModels
{
    public class AuthorBindingModel : IAuthorModel
    {
        public int Id { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Affiliation { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        // Метрики (вычисляемые, не используются при создании/обновлении)
        public int PublicationsCount { get; set; }
        public int CoauthorsCount { get; set; }
        public int CitationCount { get; set; }
        public int HIndex { get; set; }
        public double AveragePages { get; set; }
    }
}
