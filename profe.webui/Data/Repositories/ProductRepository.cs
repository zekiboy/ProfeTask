using System;
using Microsoft.EntityFrameworkCore;
using profe.webui.Data.Context;
using profe.webui.Entities;

namespace profe.webui.Data.Repositories
{
    public class ProductRepository : GenericRepository<Product> , IProductRepository
    {
        public readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }



        public List<Product> GetProductByCategoryId(int id)
        {
            var products = _context.Products.AsQueryable();
            //AsQueryable biz sorguyu yazıyoruz ama veritabanına göndermeden ben üzerine bir linq sorgusu eklemek istiyorum demek
            if (id != null)
            {
                products = products
                                .Include(i => i.ProductCategories)
                                .ThenInclude(i => i.Category)
                                .Where(i => i.ProductCategories.Any(a => a.Category.Id == id));
            }
            return  products.ToList();
        }



        public Product GetProductDetails(int id)
        {
            var product =  _context.Products
                                    .Where(i => i.Id == id)
                                    .Include(i => i.ProductCategories)
                                    .ThenInclude(i => i.Category)
                                    .FirstOrDefault();
            return product;

        }



        public List<Product> GetProductsByQSearch(string search)
        {
            var products = _context.Products.AsQueryable();

            products = products
                        .Include(i => i.ProductCategories)
                        .ThenInclude(i => i.Category)
                        .Where(i => i.ProductCategories.Any(a =>
                            a.Product.productName.ToLower().Contains(search.ToLower())
                                || a.Product.Description.ToLower().Contains(search.ToLower())
                                || a.Category.categoryName.ToLower().Contains(search.ToLower())
                            ));
            return products.ToList();
        }


        public  void Update(Product entity, int[] categoryIds)
        {

                var product = _context.Products
                        .Include(i => i.ProductCategories)
                        .FirstOrDefault(i => i.Id == entity.Id);

                if (product != null)
                {
                    product.Id = entity.Id;
                    product.productName = entity.productName;
                    product.Price = entity.Price;
                    product.Description = entity.Description;
                    product.ImgUrl = entity.ImgUrl;
                    product.Stock = entity.Stock;
                    product.IsApproved = entity.IsApproved;

                    product.ProductCategories = categoryIds.Select(catId => new ProductCategory()
                    {
                        ProductId = entity.Id,
                        CategoryId = catId
                    }).ToList();

                    _context.SaveChanges();
                }
        }

    }
}

