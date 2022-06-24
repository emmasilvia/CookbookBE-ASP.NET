﻿using System.ComponentModel.DataAnnotations;

namespace CookbookBackEnd.Controllers.Account
{
    public class LoginRequestDTO
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
