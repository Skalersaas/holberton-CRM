namespace Domain.Models.Entities
{
    public class AdmissionNote : IModel
    {
        public Guid Guid { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Content { get; set; } = string.Empty;


        public Guid AdmissionGuid { get; set; }
        public Admission Admission { get; set; } = new();
    }
}
