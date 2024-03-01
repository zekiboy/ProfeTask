using System;
using profe.webui.Entities.Common;

namespace profe.webui.Data.Repositories
{
	public interface IGenericRepository<T> 
    {

        Task<T> GetById(int id);

        List<T> GetAll();

        Task<T> AddAsync(T entity);

        Task AddRangeAsync(IList<T> entities);

        Task<T> UpdateAsync(T entity);

        Task Delete(T entity);

        Task HardDeleteRangeAsync(IList<T> entity);
    }
}

