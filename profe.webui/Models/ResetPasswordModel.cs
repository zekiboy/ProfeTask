﻿using System;
using System.ComponentModel.DataAnnotations;

namespace profe.webui.Models
{
	public class ResetPasswordModel
	{
        [Required]
        public string Token { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}

