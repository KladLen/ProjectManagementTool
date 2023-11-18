using System.ComponentModel.DataAnnotations;

namespace ProjectManagementToolAPI.Models.DTO
{
    public class TaskDTO
    {
        [Required]
        public string Title { get; set; }
        public string? Description { get; set; }
        [Required]
        public string Status { get; set; }
        public string DueDate { get; set; }
        public string Priority { get; set; }
        public string ProjectTitle { get; set; }
        public string? AsigneeFullName { get; set; }
    }
}
