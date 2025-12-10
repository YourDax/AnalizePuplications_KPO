namespace AnalizePublicationsDataModels.Models
{
    /// <summary>
    /// Модель метрик по публикациям автора.
    /// </summary>
    public interface IMetricsModel
    {
        /// <summary>
        /// Общее число публикаций автора.
        /// </summary>
        int PublicationsCount { get; }

        /// <summary>
        /// Число уникальных соавторов.
        /// </summary>
        int CoauthorsCount { get; }

        /// <summary>
        /// Общее число цитирований всех публикаций автора (вводится вручную).
        /// </summary>
        int CitationCount { get; }

        /// <summary>
        /// Среднее число страниц в публикации.
        /// </summary>
        double AveragePages { get; }

        /// <summary>
        /// Среднее число соавторов на одну публикацию.
        /// </summary>
        double AverageCoauthorsPerPublication { get; }

        /// <summary>
        /// Среднее число цитирований на одну публикацию.
        /// </summary>
        double AverageCitationsPerPublication { get; }
    }
}
