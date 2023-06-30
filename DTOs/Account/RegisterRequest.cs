using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_Api.DTOs.Account
{
    public class RegisterRequest /// For validation
    {
        [Required]
        [EmailAddress]
        public string Username { get; set; }

        [Required]
        [MinLength(8)]
        public string Password { get; set; }

        public int RoleId { get; set; }
    }
}
/*
per@gmail.com",
  "password": "486912345
*/