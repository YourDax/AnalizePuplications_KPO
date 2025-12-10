using AnalyzePublicationsContracts.BindingModels;
using AnalyzePublicationsContracts.ViewModels;
using AnalyzePublicationsDataModels.Models;
using AnalyzePublicationsDataModels.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnalyzePublicationsDatabaseImplement.Models
{
    public class Publication : IPublicationModel
    {
        [Key]
        public int Id { get; private set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public int Year { get; set; }

        [Required]
        public PublicationType PublicationType { get; set; }

        // Foreign key
        public int SourceId { get; set; }
        public virtual Source Source { get; set; } = new();

        public string? Doi { get; set; }

        [Required]
        public int CitationCount { get; set; }

        // Навигационные свойства
        public virtual List<PublicationAuthor> PublicationAuthors { get; set; } = new();

        // ---------------- Methods ----------------

        public static Publication? Create(PublicationBindingModel model)
        {
            if (model == null) return null;
            return new Publication
            {
                Id = model.Id,
                Title = model.Title,
                Year = model.Year,
                PublicationType = model.PublicationType,
                SourceId = model.SourceId,
                Doi = model.Doi,
                CitationCount = model.CitationCount
            };
        }

        public void Update(PublicationBindingModel model)
        {
            if (model == null) return;
            Title = model.Title;
            Year = model.Year;
            PublicationType = model.PublicationType;
            SourceId = model.SourceId;
            Doi = model.Doi;
            CitationCount = model.CitationCount;
        }

        public PublicationViewModel GetViewModel => new()
        {
            Id = Id,
            Title = Title,
            Year = Year,
            PublicationType = PublicationType,
            SourceId = SourceId,
            Doi = Doi,
            CitationCount = CitationCount
        };
    }
}
