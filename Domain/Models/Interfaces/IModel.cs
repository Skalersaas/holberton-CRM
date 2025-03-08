namespace Domain.Models.Interfaces
{
    public interface IModel
    {
        Guid Guid { get; set; }
        string Slug { get; set; }
    }
}
