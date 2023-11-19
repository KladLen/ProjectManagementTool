using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementToolAPI.Models.DTO;
using ProjectManagementToolAPI.Models;
using ProjectManagementToolAPI.Data;
using Microsoft.IdentityModel.Tokens;

namespace ProjectManagementToolAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public ProjectController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpPost]
        [Route("create")]
        public IActionResult Create([FromBody] ProjectDTO project)
        {
            if (project == null)
            {
                return BadRequest();
            }
          
            ProjectModel model = new ProjectModel
            {
                Name = project.Name,
                Key = project.Key,
                Description = project.Description,
                Status = "ToDo"
            };

            if (!project.ManagerFullName.IsNullOrEmpty())
            {
                string[] manager = project.ManagerFullName.Split(' ');
                model.ManagerId = _db.Users.Where(u => u.UserName == manager[0] && u.LastName == manager[1])
                                           .Select(p => p.Id).FirstOrDefault();
            }

            _db.Projects.Add(model);
            _db.SaveChanges();

            return Ok();
        }
    }
}
