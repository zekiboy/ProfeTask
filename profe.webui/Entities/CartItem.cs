using System;
using profe.webui.Entities.Common;

namespace profe.webui.Entities
{
	public class CartItem : BaseEntity
	{
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int CartId { get; set; }
        public Cart Cart { get; set; }

        public int Quantity { get; set; }
    }
}

