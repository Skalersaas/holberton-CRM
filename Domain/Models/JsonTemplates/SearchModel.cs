namespace Domain.Models.JsonTemplates
{
    public class SearchModel
    {
        public int Size { get; set; }
        public int Page {  get; set; }
        public string? SortedField { get; set; }
        public bool IsAscending {  get; set; }
        public string FilteredField { get; set; } = string.Empty;
        public Dictionary<string, string> Filters { get; set; } = new Dictionary<string, string>();
        public bool PaginationValid() => Size > 0 && Page > 0; 
    }
}
