using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Models;
using TaskModel = TaskManagementSystem.Models.Task;
namespace TaskManagementSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<TaskModel> Tasks => Set<TaskModel>();
    }
}
