using Domain.Models.Interfaces;
using Utilities.DataManipulation;
namespace Application.Interfaces
{
    public interface IModelService<TModel, TCreate, TUpdate, TResponse>
           where TModel : class, IModel
    {
        Task<(bool, TResponse)> CreateAsync(TCreate entity);
        Task<(bool, TResponse)> GetByIdAsync(Guid guid);
        Task<(TResponse[], int)> GetAllAsync(SearchModel model);
        Task<(bool, TResponse)> UpdateAsync(TUpdate entity);
        Task<bool> DeleteAsync(Guid guid);
    }
}
