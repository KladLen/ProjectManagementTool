using System.ComponentModel.DataAnnotations;

namespace ProjectManagementToolAPI.Models.DTO
{
    public class LoginRequestDTO
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
