namespace AnalyzePublicationsContracts.SearchModels
{
    public class PublicationSearchModel
    {
        public int? Id { get; set; }
        public string? Title { get; set; }
        public DateTime? PublicationDateFrom { get; set; }
        public DateTime? PublicationDateTo { get; set; }
        public int? SourceId { get; set; }
    }
}
