using System;
using profe.webui.Entities.Common;

namespace profe.webui.Entities
{
	public class Cart : BaseEntity
	{

        public string UserId { get; set; }
        public List<CartItem> CartItems { get; set; }
    }
}

