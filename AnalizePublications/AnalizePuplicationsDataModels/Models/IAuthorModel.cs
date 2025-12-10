using AnalizePuplicationsDataModels;

namespace AnalyzePublicationsDataModels.Models
{
    /// <summary>
    /// Автор / исследователь
    /// </summary>
    public interface IAuthorModel : IId
    {
        /// <summary>
        /// Полное имя автора
        /// </summary>
        string FullName { get; }

        /// <summary>
        /// Организация или кафедра
        /// </summary>
        string Affiliation { get; }

        /// <summary>
        /// Электронная почта
        /// </summary>
        string Email { get; }

        // ---------------------
        // Метрики автора
        // ---------------------

        /// <summary>
        /// Количество публикаций
        /// </summary>
        int PublicationsCount { get; }

        /// <summary>
        /// Количество соавторов
        /// </summary>
        int CoauthorsCount { get; }

        /// <summary>
        /// Количество цитирований всех публикаций автора
        /// </summary>
        int CitationCount { get; }

        /// <summary>
        /// Индекс Хирша (H-индекс)
        /// </summary>
        int HIndex { get; }

        /// <summary>
        /// Среднее число страниц в публикации.
        /// </summary>
        double AveragePages { get; }
    }
}
