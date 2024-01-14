using ProjectManagementToolAPI.Models;
using ProjectManagementToolAPI.Models.DTO;

namespace ProjectManagementToolAPI.Data.Interfaces
{
    public interface ITaskService
    {
        public TaskDTO MapTaskToTaskDTO(TaskModel task);
        public TaskDTO MapTaskToTaskDTOWithoutUser(TaskModel task);
        public TaskModel MapTaskDTOToTaskModel(TaskDTO taskDTO, string userMail);
    }
}
