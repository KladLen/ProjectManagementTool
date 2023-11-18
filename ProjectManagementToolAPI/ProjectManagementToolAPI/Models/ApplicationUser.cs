using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManagementToolAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string LastName { get; set; }
        [InverseProperty("Creator")]
        public ICollection<TaskModel> CreatedTasks { get; set; }
        [InverseProperty("Assignee")]
        public ICollection<TaskModel> AssignedTasks { get; set; }
        public ICollection<ProjectModel> Projects { get; set; }
    }
}
