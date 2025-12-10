using AnalizePuplicationsDataModels;

namespace AnalyzePublicationsDataModels.Models
{
    /// <summary>
    /// Связь публикации и автора (многие-ко-многим)
    /// </summary>
    public interface IPublicationAuthorModel : IId
    {
        /// <summary>
        /// ID автора
        /// </summary>
        int AuthorId { get; }

        /// <summary>
        /// ID публикации
        /// </summary>
        int PublicationId { get; }
    }
}
