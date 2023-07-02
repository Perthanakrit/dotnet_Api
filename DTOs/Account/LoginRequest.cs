using System.ComponentModel.DataAnnotations;

namespace dotnet_Api.DTOs.Account
{
    public class LoginRequest /// For validation
    {
        [Required]
        [EmailAddress]
        public string Username { get; set; }

        [Required]
        [MinLength(8)]
        public string Password { get; set; }
    }
}
/*
per@gmail.com",
  "password": "486912345
*/