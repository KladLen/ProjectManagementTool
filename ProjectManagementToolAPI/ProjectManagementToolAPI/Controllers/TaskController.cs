using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementToolAPI.Data;
using ProjectManagementToolAPI.Models;
using ProjectManagementToolAPI.Models.DTO;
using System.Security.Claims;

namespace ProjectManagementToolAPI.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public TaskController(ApplicationDbContext db, UserManager<ApplicationUser> userManager) 
        {
            _db = db;
            _userManager = userManager;
        }

        [HttpPost]
        [Route("create")]
        public IActionResult Create([FromBody] TaskDTO task)
        {
            if (task == null)
            {
                return BadRequest();
            }

            var mail = _userManager.GetUserId(User);
            
            string[] assignee = task.AsigneeFullName.Split(' ');
            TaskModel model = new TaskModel
            {
                Title = task.Title,
                Description = task.Description,
                Status = "ToDo",
                CreationDate = DateTime.Now.Date.ToString("yyyy-MM-dd"),
                DueDate = task.DueDate,
                Priority = task.Priority,
                ProjectId = _db.Projects.Where(p => p.Name == task.ProjectTitle).Select(p => p.Id).FirstOrDefault(),
                CreatorId = _db.Users.Where(u => u.Email == mail).Select(u => u.Id).FirstOrDefault(),
                AssigneeId = _db.Users.Where(u => u.UserName == assignee[0] && u.LastName == assignee[1])
                                      .Select(p => p.Id).FirstOrDefault()
            };

            _db.Tasks.Add(model);
            _db.SaveChanges();
            return Ok();
        }
    }
}
