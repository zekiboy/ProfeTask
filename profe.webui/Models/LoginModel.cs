using System;
using System.ComponentModel.DataAnnotations;

namespace profe.webui.Models
{
	public class LoginModel
	{
        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string? ReturnUrl { get; set; }
    }
}

