using TaskManagementSystem.DTOs;
using TaskModel = TaskManagementSystem.Models.Task;
using TaskManagementSystem.Models;
using TaskManagementSystem.Repositories;
using TaskStatus = TaskManagementSystem.Models.TaskStatus;

namespace TaskManagementSystem.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _repo;

        public TaskService(ITaskRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<TaskModel>> GetAllTasksAsync() => await _repo.GetAllTasksAsync();

        public async Task<TaskModel?> GetTaskByIdAsync(int id) => await _repo.GetTaskByIdAsync(id);

        public async Task<TaskModel> CreateTaskAsync(TaskDto dto)
        {
            var task = new TaskModel
            {
                Title = dto.Title,
                Description = dto.Description,
                DueDate = dto.DueDate,
                Status = Enum.Parse<TaskStatus>(dto.Status)
            };
            return await _repo.AddTaskAsync(task);
        }

        public async Task<TaskModel?> UpdateTaskAsync(int id, TaskDto dto)
        {
            var existing = await _repo.GetTaskByIdAsync(id);
            if (existing == null) return null;

            existing.Title = dto.Title;
            existing.Description = dto.Description;
            existing.DueDate = dto.DueDate;
            existing.Status = Enum.Parse<TaskStatus>(dto.Status);

            return await _repo.UpdateTaskAsync(existing);
        }
        //public async Task<TaskModel?> UpdateTaskAsync(int id, TaskDto dto)
        //{
        //    var existing = await _repo.GetTaskByIdAsync(id);
        //    if (existing == null) return null;

        //    existing.Title = dto.Title;
        //    existing.Description = dto.Description;
        //    existing.DueDate = DateTime.SpecifyKind(dto.DueDate, DateTimeKind.Utc);
        //    existing.Status = Enum.Parse<TaskStatus>(dto.Status);

        //    return await _repo.UpdateTaskAsync(existing);
        //}
        public async Task<bool> DeleteTaskAsync(int id) => await _repo.DeleteTaskAsync(id);
    }
}