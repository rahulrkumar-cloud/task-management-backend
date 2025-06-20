using System;
using System.ComponentModel.DataAnnotations;

public class TaskDto
{
    public TaskDto()
    {
        DueDate = DateTime.UtcNow;
    }

    [Required(ErrorMessage = "Title is required.")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Description is required.")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "DueDate is required.")]
    public DateTime DueDate { get; set; }

    [Required(ErrorMessage = "Status is required.")]
    [RegularExpression("Pending|InProgress|Completed",
        ErrorMessage = "Status must be one of: Pending, InProgress, Completed.")]
    public string Status { get; set; } = "Pending";
}
