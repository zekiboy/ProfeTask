using System;
using profe.webui.Entities;

namespace profe.webui.Models
{
	public class ProductDetailModel
	{
        public Product Product { get; set; }
        public List<Category>? Categories { get; set; }
    }
}

