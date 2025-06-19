using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Models
{
    public enum TaskStatus
    {
        Pending,
        InProgress,
        Completed
    }

    public class Task
    {
        [Key]
        public int TaskId { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public DateTime DueDate { get; set; }

        [Required]
        public TaskStatus Status { get; set; }
    }
}