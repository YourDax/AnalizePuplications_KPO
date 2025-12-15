using System.ComponentModel;
using AnalyzePublicationsDataModels.Models;

namespace AnalyzePublicationsContracts.ViewModels
{
    public class AuthorViewModel : IAuthorModel
    {
        public int Id { get; set; }

        [DisplayName("Полное имя")]
        public string FullName { get; set; } = string.Empty;

        [DisplayName("Организация/Кафедра")]
        public string Affiliation { get; set; } = string.Empty;

        [DisplayName("Электронная почта")]
        public string Email { get; set; } = string.Empty;

        // Метрики
        [DisplayName("Количество публикаций")]
        public int PublicationsCount { get; set; }

        [DisplayName("Количество соавторов")]
        public int CoauthorsCount { get; set; }

        [DisplayName("Количество цитирований")]
        public int CitationCount { get; set; }

        [DisplayName("H-индекс")]
        public int HIndex { get; set; }

        [DisplayName("Среднее число страниц")]
        public double AveragePages { get; set; }

        // Дополнительные метрики
        [DisplayName("i10-индекс")]
        public int I10Index { get; set; }

        [DisplayName("Средние цитирования на публикацию")]
        public double AverageCitationsPerPublication { get; set; }

        [DisplayName("Общее количество страниц")]
        public int TotalPages { get; set; }

        [DisplayName("Индекс продуктивности")]
        public double ProductivityIndex { get; set; }

        [DisplayName("Максимальное количество цитирований")]
        public int MaxCitations { get; set; }

        [DisplayName("Минимальное количество цитирований")]
        public int MinCitations { get; set; }
    }
}
