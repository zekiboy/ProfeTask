using System;
using profe.webui.Entities.Common;

namespace profe.webui.Entities
{
	public class ProductCategory 
	{
        public int ProductId { get; set; }

        public int CategoryId { get; set; }

        public Product Product { get; set; }

        public Category Category { get; set; }
    }
}

