using System;
using profe.webui.Entities;

namespace profe.webui.Data.Services
{
	public interface ICartService
	{
        void InitializeCart(string userId);
        Cart GetCartByUserId(string userId);
        void AddToCart(string userId, int productId, int quantity);
        void DeleteFromCart(string userId, int productId);
    }
}

