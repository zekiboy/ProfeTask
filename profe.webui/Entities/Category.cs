using System;
using profe.webui.Entities.Common;

namespace profe.webui.Entities
{
	public class Category : BaseEntity
	{
        public string categoryName { get; set; }

        public List<ProductCategory> ProductCategories { get; set; }
    }
}

