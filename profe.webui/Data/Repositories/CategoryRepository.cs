using System;
using Microsoft.EntityFrameworkCore;
using profe.webui.Data.Context;
using profe.webui.Entities;

namespace profe.webui.Data.Repositories
{
	public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void DeleteProductFromCategory(int productId, int categoryId)
        {

            var cmd = "delete from productcategory where ProductId=@p0 and CategoryId=@p1;";
            _context.Database.ExecuteSqlRaw(cmd, productId, categoryId);


        }

        public Category GetByIdwithProducts(int categoryId)
        {

            var rtn = _context.Categories
                            .Where(i => i.Id == categoryId)
                            .Include(i => i.ProductCategories)
                            .ThenInclude(i => i.Product)
                            .FirstOrDefault();

            return rtn;
        }
    }
}

