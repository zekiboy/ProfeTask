using System;
using Microsoft.AspNetCore.Identity;

namespace profe.webui.Entities
{
	public class User :IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}

