﻿using System.ComponentModel.DataAnnotations;

namespace ProjectManagementToolAPI.Models.DTO
{
    public class RegistrationRequestDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
