<<<<<<< HEAD
﻿using Domain.Models.JsonTemplates;

namespace Persistance.Data
=======
﻿namespace Persistance.Data
>>>>>>> parent of 699593b (modify_userController)
{
    public interface IRepository<T> where T : class
    {
        Task<T?> CreateAsync(T entity);
        Task<T?> GetByIdAsync(Guid id);
<<<<<<< HEAD
        Task<T?> GetBySlugAsync(string id);
        Task<IEnumerable<T>> GetAllAsync(SearchModel model);
=======
        Task<IEnumerable<T>> GetAllAsync();
>>>>>>> parent of 699593b (modify_userController)
        Task UpdateAsync(T entity);
        Task<bool> DeleteAsync(Guid id);
        void Detach(T entity);
    }
}
