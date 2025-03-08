namespace Domain.Models.JsonTemplates
{
    public class ChangeTemplate
    {
        public string Field { get; set; } = string.Empty;
        public string? Prev { get; set; }
        public string? Next { get; set; }
    }
}
