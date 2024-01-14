using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProjectManagementToolAPI.Data;
using ProjectManagementToolAPI.Data.Interfaces;
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
        private readonly ITaskService _taskService;
        public TaskController(ApplicationDbContext db, UserManager<ApplicationUser> userManager, ITaskService taskService) 
        {
            _db = db;
            _userManager = userManager;
            _taskService = taskService;
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
                tasksDTO.Add(_taskService.MapTaskToTaskDTO(task));
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

            TaskDTO taskDTO = _taskService.MapTaskToTaskDTOWithoutUser(task);

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
                        
            TaskModel model = _taskService.MapTaskDTOToTaskModel(taskDTO, mail);

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
