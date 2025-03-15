namespace Domain.Interfaces
{

    public interface IModel
    {
        public Guid Id { get; set; }
        public string Slug { get; set; }
    }
}
