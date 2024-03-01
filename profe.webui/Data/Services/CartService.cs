using System;
using profe.webui.Data.Repositories;
using profe.webui.Entities;

namespace profe.webui.Data.Services
{
	public class CartService : ICartService
	{
        private readonly ICartRepository _cartRepository;
		public CartService(ICartRepository cartRepository)
		{
            _cartRepository = cartRepository;
		}

        public void AddToCart(string userId, int productId, int quantity)
        {
            var cart = GetCartByUserId(userId);
            if (cart != null)
            {
                //eklenmek isteyen ürün sepette var mı (güncelleme)
                //eklenmek isteyen ürün sepette var ve yeni kayıt oluştur(kayıt ekleme)

                var index = cart.CartItems.FindIndex(i => i.ProductId == productId);

                if (index < 0)
                {
                    cart.CartItems.Add(new CartItem()
                    {
                        ProductId = productId,
                        Quantity = quantity,
                        CartId = cart.Id
                    });
                }
                else
                {
                    cart.CartItems[index].Quantity += quantity;
                }

                _cartRepository.UpdateAsync(cart);

            }
        }

        public void DeleteFromCart(string userId, int productId)
        {
            var cart = GetCartByUserId(userId);
            if (cart != null)
            {
                _cartRepository.DeleteFromCart(cart.Id, productId);
            }
        }

        public Cart GetCartByUserId(string userId)
        {
            return _cartRepository.GetByUserId(userId);
        }

        public void InitializeCart(string userId)
        {
            _cartRepository.AddAsync(new Cart() { UserId = userId });
        }
    }
}

