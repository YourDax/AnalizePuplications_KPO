using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnalyzePublicationsDatabaseImplement.Models
{
    public class PublicationAuthor
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Publication")]
        public int PublicationId { get; set; }

        public virtual Publication Publication { get; set; } = null!;

        [ForeignKey("Author")]
        public int AuthorId { get; set; }

        public virtual Author Author { get; set; } = null!;
    }
}
