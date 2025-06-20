using System.Globalization;
using TaskManagementSystem.Models;
using TaskManagementSystem.Repositories;
using TaskModel = TaskManagementSystem.Models.Task;
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
            var (dueDate, status) = ValidateAndParse(dto);

            var task = new TaskModel
            {
                Title = dto.Title,
                Description = dto.Description,
                DueDate = dueDate,
                Status = status
            };

            return await _repo.AddTaskAsync(task);
        }

        public async Task<TaskModel?> UpdateTaskAsync(int id, TaskDto dto)
        {
            var existing = await _repo.GetTaskByIdAsync(id);
            if (existing == null) return null;

            var (dueDate, status) = ValidateAndParse(dto);

            existing.Title = dto.Title;
            existing.Description = dto.Description;
            existing.DueDate = dueDate;
            existing.Status = status;

            return await _repo.UpdateTaskAsync(existing);
        }

        public async Task<bool> DeleteTaskAsync(int id) => await _repo.DeleteTaskAsync(id);

        private (DateTime DueDate, TaskStatus Status) ValidateAndParse(TaskDto dto)
        {
            var dueDate = dto.DueDate;

            if (!Enum.TryParse<TaskStatus>(dto.Status, true, out var parsedStatus))
                throw new ArgumentException("Invalid status. Valid values are: Pending, InProgress, Completed.");

            return (dueDate, parsedStatus);
        }


    }
}