namespace AnalyzePublicationsContracts.SearchModels
{
    public class PublicationSearchModel
    {
        public int? Id { get; set; }
        public string? Title { get; set; }
        public int? Year { get; set; }
        public int? SourceId { get; set; }
    }
}
