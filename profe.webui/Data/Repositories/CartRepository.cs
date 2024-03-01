using System;
using Microsoft.EntityFrameworkCore;
using profe.webui.Data.Context;
using profe.webui.Entities;
using SendGrid.Helpers.Mail;

namespace profe.webui.Data.Repositories
{
	public class CartRepository : GenericRepository<Cart>, ICartRepository
	{

        public readonly ApplicationDbContext _context;

        public CartRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void DeleteFromCart(int cartId, int productId)
        {
            var cmd = @"delete from CardItems where CartId=@p0 and ProductId=@p1";
            _context.Database.ExecuteSqlRaw(cmd, cartId, productId);
        }

        public Cart GetByUserId(string userId)
        {
            return _context.Cards
                             .Include(i => i.CartItems)
                             .ThenInclude(i => i.Product)
                             .FirstOrDefault(i => i.UserId == userId);
        }

        public void ClearCart(int cartId)
        {
            var cmd = @"delete from CartItems where CartId=@p0";
            _context.Database.ExecuteSqlRaw(cmd, cartId);
        }

        public void Update(Cart entity)
        {

            _context.Cards.Update(entity);
            _context.SaveChanges();
            
        }
    }
}

