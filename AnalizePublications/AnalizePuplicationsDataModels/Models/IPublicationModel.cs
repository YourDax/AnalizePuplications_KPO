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
        /// Дата публикации
        /// </summary>
        DateTime? PublicationDate { get; }

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
        /// Количество страниц
        /// </summary>
        int? Pages { get; }

        /// <summary>
        /// Аннотация/Abstract публикации
        /// </summary>
        string? Abstract { get; }

        /// <summary>
        /// Ключевые слова
        /// </summary>
        string? Keywords { get; }

        /// <summary>
        /// Число цитирований данной публикации.
        /// Вводится вручную пользователем.
        /// </summary>
        int CitationCount { get; }
    }
}
