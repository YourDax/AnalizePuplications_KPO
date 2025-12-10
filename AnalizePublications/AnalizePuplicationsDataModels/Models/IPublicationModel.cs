using AnalizePuplicationsDataModels;
using AnalyzePublicationsDataModels.Enums;

namespace AnalyzePublicationsDataModels.Models
{
    /// <summary>
    /// Научная публикация
    /// </summary>
    public interface IPublicationModel : IId
    {
        /// <summary>
        /// Заголовок публикации
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Год публикации
        /// </summary>
        int Year { get; }

        /// <summary>
        /// Тип публикации (журнал, конференция, патент...)
        /// </summary>
        PublicationType PublicationType { get; }

        /// <summary>
        /// Идентификатор источника публикации
        /// </summary>
        int SourceId { get; }

        /// <summary>
        /// DOI уникальный идентификатор публикации
        /// </summary>
        string? Doi { get; }

        /// <summary>
        /// Число цитирований данной публикации.
        /// Вводится вручную пользователем.
        /// </summary>
        int CitationCount { get; }
    }
}
