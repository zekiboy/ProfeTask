using System;
using profe.webui.Entities.Common;

namespace profe.webui.Entities
{
    public class Product : BaseEntity
    {
        public string productName { get; set; }
        public string Description { get; set; }
        public int Stock { get; set; }
        public int Price { get; set; }
        public string? ImgUrl { get; set; }
        public bool IsApproved { get; set; }
        public List<ProductCategory> ProductCategories { get; set; }
    }
}

