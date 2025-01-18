namespace holberton_CRM.Data
{
    public interface IRepository<T> where T : class
    {
        Task<T> CreateAsync(T entity);
        Task<T?> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        Task UpdateAsync(T entity);
        Task UpdateAsync(T prev, T next);
        Task<bool> DeleteAsync(Guid id);
        void Detach(T entity);
    }
}
