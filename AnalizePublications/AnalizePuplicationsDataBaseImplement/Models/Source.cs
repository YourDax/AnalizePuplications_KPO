using AnalyzePublicationsContracts.BindingModels;
using AnalyzePublicationsContracts.ViewModels;
using AnalyzePublicationsDataModels.Models;
using AnalyzePublicationsDataModels.Enums;
using System.ComponentModel.DataAnnotations;

namespace AnalyzePublicationsDatabaseImplement.Models
{
    public class Source : ISourceModel
    {
        [Key]
        public int Id { get; private set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Url { get; set; } = string.Empty;

        [Required]
        public SourceType SourceType { get; set; }

        // Навигационные свойства
        public virtual List<Publication> Publications { get; set; } = new();

        // ---------------- Methods ----------------

        public static Source? Create(SourceBindingModel model)
        {
            if (model == null) return null;
            return new Source
            {
                Id = model.Id,
                Name = model.Name,
                Url = model.Url,
                SourceType = model.SourceType
            };
        }

        public void Update(SourceBindingModel model)
        {
            if (model == null) return;
            Name = model.Name;
            Url = model.Url;
            SourceType = model.SourceType;
        }

        public SourceViewModel GetViewModel => new()
        {
            Id = Id,
            Name = Name,
            Url = Url,
            SourceType = SourceType
        };
    }
}
