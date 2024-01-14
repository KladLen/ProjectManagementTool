using ProjectManagementToolAPI.Data.Interfaces;
using ProjectManagementToolAPI.Models;
using ProjectManagementToolAPI.Models.DTO;

namespace ProjectManagementToolAPI.Data.Implementation
{
    public class TaskService : ITaskService
    {
        private readonly ApplicationDbContext _db;

        public TaskService(ApplicationDbContext db)
        {
            _db = db;
        }

        public TaskDTO MapTaskToTaskDTO(TaskModel task)
        {
            TaskDTO taskDTO = MapTaskToTaskDTOWithoutUser(task);
            taskDTO.AsigneeFullName = task.Assignee.UserName + " " + task.Assignee.LastName;

            return taskDTO;
        }

        public TaskDTO MapTaskToTaskDTOWithoutUser(TaskModel task)
        {
            TaskDTO taskDTO = new TaskDTO
            {
                Title = task.Title,
                Description = task.Description,
                Status = task.Status,
                DueDate = task.DueDate,
                Priority = task.Priority,
                ProjectTitle = task.Project.Name
            };

            return taskDTO;
        }

        public TaskModel MapTaskDTOToTaskModel(TaskDTO taskDTO, string userMail)
        {
            TaskModel model = new TaskModel
            {
                Title = taskDTO.Title,
                Description = taskDTO.Description,
                Status = "ToDo",
                CreationDate = DateTime.Now.Date.ToString("yyyy-MM-dd"),
                DueDate = taskDTO.DueDate,
                Priority = taskDTO.Priority,
                ProjectId = _db.Projects.Where(p => p.Name == taskDTO.ProjectTitle).Select(p => p.Id).FirstOrDefault(),
                CreatorId = _db.Users.Where(u => u.Email == userMail).Select(u => u.Id).FirstOrDefault()
            };
            return model;
        }
    }
}
