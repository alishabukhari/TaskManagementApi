using System.ComponentModel.DataAnnotations;

namespace TaskManagementApi.Dtos;

public class CreateTaskDto
{
    [Required]
    [MinLength(3)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;
}