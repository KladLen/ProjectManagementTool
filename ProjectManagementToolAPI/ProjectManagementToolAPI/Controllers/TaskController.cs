using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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

        [HttpGet]
        [Route("index")]
        public IActionResult Index()
        {
            var mail = _userManager.GetUserId(User);
            var userId = _db.Users.Where(u => u.Email == mail).Select(u => u.Id).FirstOrDefault();
            var tasks = _db.Tasks.Where(t => t.AssigneeId == userId).Include(t => t.Assignee).Include(t => t.Project).ToList();

            List<TaskDTO> tasksDTO = new List<TaskDTO>();
            foreach (var task in tasks)
            {
                tasksDTO.Add(new TaskDTO
                {
                    Title = task.Title,
                    Description = task.Description,
                    Status = task.Status,
                    DueDate = task.DueDate,
                    Priority = task.Priority,
                    ProjectTitle = task.Project.Name,
                    AsigneeFullName = task.Assignee.UserName + " " + task.Assignee.LastName
                });
            }
            return Ok(tasksDTO);
        }

        [HttpGet]
        [Route("details/{id}")]
        public IActionResult Details(int id)
        {
            TaskModel task = _db.Tasks.Where(t => t.Id == id).Include(t =>  t.Project).FirstOrDefault();   
            
            if (task == null)
            {
                return NotFound();   
            }

            TaskDTO taskDTO = new TaskDTO
            {
                Title = task.Title,
                Description = task.Description,
                Status = task.Status,
                DueDate = task.DueDate,
                Priority = task.Priority,
                ProjectTitle = task.Project.Name,
            };

            if (!string.IsNullOrEmpty(task.AssigneeId))
            {
                _db.Entry(task).Reference(t => t.Assignee).Load();
                taskDTO.AsigneeFullName = task.Assignee.UserName + " " + task.Assignee.LastName;
            }
            return Ok(taskDTO);
        }

        [HttpPost]
        [Route("create")]
        public IActionResult Create([FromBody] TaskDTO taskDTO)
        {
            if (taskDTO == null)
            {
                return BadRequest();
            }

            var mail = _userManager.GetUserId(User);
                        
            TaskModel model = new TaskModel
            {
                Title = taskDTO.Title,
                Description = taskDTO.Description,
                Status = "ToDo",
                CreationDate = DateTime.Now.Date.ToString("yyyy-MM-dd"),
                DueDate = taskDTO.DueDate,
                Priority = taskDTO.Priority,
                ProjectId = _db.Projects.Where(p => p.Name == taskDTO.ProjectTitle).Select(p => p.Id).FirstOrDefault(),
                CreatorId = _db.Users.Where(u => u.Email == mail).Select(u => u.Id).FirstOrDefault()
            };

            if (!taskDTO.AsigneeFullName.IsNullOrEmpty())
            {
                string[] assignee = taskDTO.AsigneeFullName.Split(' ');
                model.AssigneeId = _db.Users.Where(u => u.UserName == assignee[0] && u.LastName == assignee[1])
                                            .Select(p => p.Id).FirstOrDefault();
            }

            _db.Tasks.Add(model);
            _db.SaveChanges();
            return Ok();
        }
    }
}
