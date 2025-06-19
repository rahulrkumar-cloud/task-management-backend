using TaskManagementSystem.DTOs;
using TaskModel = TaskManagementSystem.Models.Task;

namespace TaskManagementSystem.Services
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskModel>> GetAllTasksAsync();
        Task<TaskModel?> GetTaskByIdAsync(int id);
        Task<TaskModel> CreateTaskAsync(TaskDto dto);
        Task<TaskModel?> UpdateTaskAsync(int id, TaskDto dto);
        Task<bool> DeleteTaskAsync(int id);
    }
}