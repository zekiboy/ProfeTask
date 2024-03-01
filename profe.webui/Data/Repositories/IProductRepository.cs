using System;
using profe.webui.Entities;

namespace profe.webui.Data.Repositories
{
	public interface IProductRepository : IGenericRepository<Product>
    {
        List<Product> GetProductsByQSearch(string search);
        List<Product> GetProductByCategoryId(int id);
        Product GetProductDetails(int id);
        void Update(Product entity, int[] categoryIds);
    }
}

				