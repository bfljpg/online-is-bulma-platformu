﻿namespace online_is_bulma_platformu.Models
{
        public class User
        {
            public int Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string PasswordHash { get; set; }
            public string Role { get; set; }
            public DateTime CreatedAt { get; set; }
        }
    }

