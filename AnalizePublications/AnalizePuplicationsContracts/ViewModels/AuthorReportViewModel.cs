using AnalyzePublicationsDataModels.Enums;

namespace AnalyzePublicationsContracts.ViewModels
{
    public class AuthorReportViewModel
    {
        public AuthorViewModel Author { get; set; } = null!;
        public List<PublicationViewModel> Publications { get; set; } = new();
        
        // Статистика по типам публикаций
        public Dictionary<PublicationType, int> PublicationsByType { get; set; } = new();
        
        // Статистика по годам
        public Dictionary<int, int> PublicationsByYear { get; set; } = new();
        
        // Топ публикаций по цитированиям
        public List<PublicationViewModel> TopCitedPublications { get; set; } = new();
        
        // Дата формирования отчета
        public DateTime ReportDate { get; set; } = DateTime.Now;
    }
}


