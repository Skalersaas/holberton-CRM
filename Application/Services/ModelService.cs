using Application.Interfaces;
using Domain.Models.Interfaces;
using Persistance.Data.Interfaces;
using Utilities.DataManipulation;

namespace Application.Services
{
    public class ModelService<TModel, TCreate, TUpdate, TResponse>(IRepository<TModel> context) : IModelService<TModel, TCreate, TUpdate, TResponse>
        where TModel : class, IModel, new()
        where TResponse: class, new()
    {
        protected readonly IRepository<TModel> context = context;
        public virtual async Task<(bool, TResponse?)> CreateAsync(TCreate entity)
        {
            var model = Mapper.FromDTO<TModel, TCreate>(entity);
            var created = await context.CreateAsync(model);
            
            return created == null
                ? (false, null)
                : (true, Mapper.FromDTO<TResponse, TModel>(created));
        }

        public virtual async Task<bool> DeleteAsync(Guid guid) => await context.DeleteAsync(guid);
        public virtual async Task<(bool, TResponse?)> GetByIdAsync(Guid guid)
        {
            var model = await context.GetByIdAsync(guid);

            return model == null
                ? (false, null)
                : (true, Mapper.FromDTO<TResponse, TModel>(model));
        }
        public virtual (bool, TResponse?) GetByField(string fieldName, object value)
        {
            var model = context.GetByField(fieldName, value);

            return model == null
                ? (false, null)
                : (true, Mapper.FromDTO<TResponse,TModel>(model));
        }
        public virtual async Task<(TResponse[], int)> GetAllAsync(SearchModel model)
        {
            var (data, fullCount) = await context.GetAllAsync(model);

            var responseList = data.Select(Mapper.FromDTO<TResponse, TModel>).ToArray();

            return (responseList, fullCount);
        }

        public virtual async Task<(bool, TResponse?)> UpdateAsync(TUpdate entity)
        {
            var model = Mapper.FromDTO<TModel, TUpdate>(entity);
            var updated = await context.UpdateAsync(model);

            return updated == null
                ? (false, null)
                : (true, Mapper.FromDTO<TResponse, TModel>(updated));

        }
    }
}
