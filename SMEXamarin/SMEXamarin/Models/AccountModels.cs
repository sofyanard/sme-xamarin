using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SMEXamarin.Models
{
    public class LoginRequest
    {
        [Required]
        public string username { get; set; }

        [Required]
        public string password { get; set; }

        [Required]
        public string grant_type { get; set; }

        public LoginRequest(string username, string password)
        {
            this.username = username;
            this.password = password;
            this.grant_type = "password";
        }

    }

    public class LoginResponse
    {
        public string access_token { get; set; }

        public string token_type { get; set; }

        public int expires_in { get; set; }

        public string userName { get; set; }

        public DateTime issued { get; set; }

        public DateTime expires { get; set; }
    }

    public class RegisterRequest
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }
    }
}
