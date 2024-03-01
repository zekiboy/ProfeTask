using System;
using profe.webui.Entities;

namespace profe.webui.Data.Repositories
{
	public interface ICategoryRepository : IGenericRepository<Category>
	{
        Category GetByIdwithProducts(int categoryId);
        void DeleteProductFromCategory(int productId, int categoryId);
    }
}

