﻿namespace Doodle.Api.Services.Users.Models
{
    public class UserRegisterInput
    {
        public string Name { get; set; }

        public string Username { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string PhoneNumber { get; set; }
    }
}