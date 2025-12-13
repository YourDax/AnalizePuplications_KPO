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

        // Навигационные свойства
        public virtual List<PublicationAuthor> PublicationAuthors { get; set; } = new();

        // Вычисляемые метрики (не хранятся в БД, вычисляются в Storage)
        [NotMapped]
        public int PublicationsCount { get; set; }

        [NotMapped]
        public int CoauthorsCount { get; set; }

        [NotMapped]
        public int CitationCount { get; set; }

        [NotMapped]
        public int HIndex { get; set; }

        [NotMapped]
        public double AveragePages { get; set; }

        // ---------------- Methods ----------------

        public static Author? Create(AuthorBindingModel model)
        {
            if (model == null) return null;
            return new Author
            {
                Id = model.Id,
                FullName = model.FullName,
                Affiliation = model.Affiliation,
                Email = model.Email
            };
        }

        public void Update(AuthorBindingModel model)
        {
            if (model == null) return;
            FullName = model.FullName;
            Affiliation = model.Affiliation;
            Email = model.Email;
        }

        // GetViewModel будет вызываться из Storage с вычисленными метриками
        public AuthorViewModel GetViewModel(int publicationsCount, int coauthorsCount, int citationCount, int hIndex, double averagePages) => new()
        {
            Id = Id,
            FullName = FullName,
            Affiliation = Affiliation,
            Email = Email,
            PublicationsCount = publicationsCount,
            CoauthorsCount = coauthorsCount,
            CitationCount = citationCount,
            HIndex = hIndex,
            AveragePages = averagePages
        };
    }
}
