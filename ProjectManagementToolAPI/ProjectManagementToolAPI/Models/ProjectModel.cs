using Microsoft.AspNetCore.Identity;

namespace ProjectManagementToolAPI.Models
{
    public class ProjectModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Key { get; set; }
        public string? Description { get; set; }
        public string Status { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? ManagerId { get; set; }
        public ApplicationUser Mananger {  get; set; }
    }
}