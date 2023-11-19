using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManagementToolAPI.Models
{
    public class TaskModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string Status { get; set; }
        public string CreationDate { get; set; }
        public string? DueDate { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string Priority { get; set; }
        public int ProjectId { get; set; }
        public ProjectModel Project { get; set; }
        public string CreatorId { get; set; }
        
        [ForeignKey("CreatorId")]
        public ApplicationUser Creator { get; set; }
        public string? AssigneeId { get; set; }
        
        [ForeignKey("AssigneeId")]
        public ApplicationUser? Assignee { get; set; }
    }
}
