using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Data;
using TaskModel = TaskManagementSystem.Models.Task;

namespace TaskManagementSystem.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly AppDbContext _context;

        public TaskRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaskModel>> GetAllTasksAsync() =>
            await _context.Tasks.ToListAsync();

        public async Task<TaskModel?> GetTaskByIdAsync(int id) =>
            await _context.Tasks.FindAsync(id);

        public async Task<TaskModel> AddTaskAsync(TaskModel task)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<TaskModel?> UpdateTaskAsync(TaskModel task)
        {
            var existing = await _context.Tasks.FindAsync(task.TaskId);
            if (existing == null) return null;

            existing.Title = task.Title;
            existing.Description = task.Description;
            existing.DueDate = task.DueDate;
            existing.Status = task.Status;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null) return false;

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}