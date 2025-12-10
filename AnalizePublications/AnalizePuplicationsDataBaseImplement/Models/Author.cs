using AnalyzePublicationsContracts.BindingModels;
using AnalyzePublicationsContracts.ViewModels;
using AnalyzePublicationsDataModels.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnalyzePublicationsDatabaseImplement.Models
{
    public class Author : IAuthorModel
    {
        [Key]
        public int Id { get; private set; }

        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public string Affiliation { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        // Метрики
        public int PublicationsCount { get; set; }
        public int CoauthorsCount { get; set; }
        public int CitationCount { get; set; }
        public int HIndex { get; set; }
        public double AveragePages { get; set; }

        // Навигационные свойства
        public virtual List<PublicationAuthor> PublicationAuthors { get; set; } = new();

        // ---------------- Methods ----------------

        public static Author? Create(AuthorBindingModel model)
        {
            if (model == null) return null;
            return new Author
            {
                Id = model.Id,
                FullName = model.FullName,
                Affiliation = model.Affiliation,
                Email = model.Email,
                PublicationsCount = model.PublicationsCount,
                CoauthorsCount = model.CoauthorsCount,
                CitationCount = model.CitationCount,
                HIndex = model.HIndex,
                AveragePages = model.AveragePages
            };
        }

        public void Update(AuthorBindingModel model)
        {
            if (model == null) return;
            FullName = model.FullName;
            Affiliation = model.Affiliation;
            Email = model.Email;
            PublicationsCount = model.PublicationsCount;
            CoauthorsCount = model.CoauthorsCount;
            CitationCount = model.CitationCount;
            HIndex = model.HIndex;
            AveragePages = model.AveragePages;
        }

        public AuthorViewModel GetViewModel => new()
        {
            Id = Id,
            FullName = FullName,
            Affiliation = Affiliation,
            Email = Email,
            PublicationsCount = PublicationsCount,
            CoauthorsCount = CoauthorsCount,
            CitationCount = CitationCount,
            HIndex = HIndex,
            AveragePages = AveragePages
        };
    }
}
