namespace Domain.Models
{
    public class ChangeTemplate
    {
        public Guid Id { get; set; }
        public string FieldName { get; set; } = string.Empty;
        public string PreValue { get; set; } = string.Empty;
        public string PostValue { get; set; } = string.Empty;
    }
}
