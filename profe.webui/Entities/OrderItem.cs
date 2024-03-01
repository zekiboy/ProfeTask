using System;
using profe.webui.Entities.Common;

namespace profe.webui.Entities
{
	public class OrderItem : BaseEntity
	{
        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public double Price { get; set; }
        public int Quantity { get; set; }
    }
}

