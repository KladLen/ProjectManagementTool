using System.ComponentModel.DataAnnotations;

namespace ProjectManagementToolAPI.Models.DTO
{
    public class ProjectDTO
    {
        [Required]
        public string Name {  get; set; }
        [Required]
        [StringLength(3)]
        public string Key { get; set; }
        public string? Description { get; set; }
        public string Status { get; set; }
        public string? ManagerFullName { get; set; }
    }
}
