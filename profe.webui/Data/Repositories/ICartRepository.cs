using System;
using profe.webui.Entities;

namespace profe.webui.Data.Repositories
{
	public interface ICartRepository : IGenericRepository<Cart>
    {
        void DeleteFromCart(int cartId, int productId);
        Cart GetByUserId(string userId);
    }
}

