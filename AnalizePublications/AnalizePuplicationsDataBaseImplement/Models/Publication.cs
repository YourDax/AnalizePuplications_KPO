using AnalyzePublicationsContracts.BindingModels;
using AnalyzePublicationsContracts.ViewModels;
using AnalyzePublicationsDataModels.Models;
using AnalyzePublicationsDataModels.Enums;
using System.ComponentModel.DataAnnotations;

namespace AnalyzePublicationsDatabaseImplement.Models
{
    public class Publication : IPublicationModel
    {
        [Key]
        public int Id { get; private set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        public DateTime? PublicationDate { get; set; }

        [Required]
        public PublicationType PublicationType { get; set; }

        // Foreign key
        public int SourceId { get; set; }
        public virtual Source? Source { get; set; }

        public string? Doi { get; set; }

        public int? Pages { get; set; }

        public string? Abstract { get; set; }

        public string? Keywords { get; set; }

        [Required]
        public int CitationCount { get; set; }

        // Навигация для many-to-many
        public virtual List<PublicationAuthor> PublicationAuthors { get; set; } = new();

        // ---------------- Methods ----------------

        public static Publication? Create(PublicationBindingModel model)
        {
            if (model == null) return null;

            return new Publication
            {
                Title = model.Title,
                PublicationDate = model.PublicationDate,
                PublicationType = model.PublicationType,
                SourceId = model.SourceId,
                Doi = model.Doi,
                Pages = model.Pages,
                Abstract = model.Abstract,
                Keywords = model.Keywords,
                CitationCount = model.CitationCount
            };
        }

        public void Update(PublicationBindingModel model)
        {
            if (model == null) return;

            Title = model.Title;
            PublicationDate = model.PublicationDate;
            PublicationType = model.PublicationType;
            SourceId = model.SourceId;
            Doi = model.Doi;
            Pages = model.Pages;
            Abstract = model.Abstract;
            Keywords = model.Keywords;
            CitationCount = model.CitationCount;
        }

        public PublicationViewModel GetViewModel => new()
        {
            Id = Id,
            Title = Title,
            PublicationDate = PublicationDate,
            PublicationType = PublicationType,
            SourceId = SourceId,
            Doi = Doi,
            Pages = Pages,
            Abstract = Abstract,
            Keywords = Keywords,
            CitationCount = CitationCount
        };
    }
}
