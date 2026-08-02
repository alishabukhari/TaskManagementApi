using Microsoft.AspNetCore.Mvc;
using TaskManagementApi.Dtos;
using TaskManagementApi.Model;

namespace TaskManagementApi.Controllers;

[ApiController]
[Route("api/tasks")]
public class TasksController : ControllerBase
{
    private static readonly List<TaskItem> Tasks = new();

    private static int _nextId = 1;


    [HttpGet("{id:int}")]
    public ActionResult<TaskItem> GetTaskById(int id)
    {
        TaskItem? task = Tasks.FirstOrDefault(task => task.Id == id);

        if (task is null)
        {
            return NotFound(new
            {
                message = $"Task with ID {id} was not found."
            });
        }

        return Ok(task);
    }

    [HttpGet("completed")]
    public ActionResult<List<TaskItem>> GetCompletedTasks()
    {
        List<TaskItem> completedTasks = Tasks
            .Where(task => task.IsCompleted)
            .ToList();

        return Ok(completedTasks);
    }

    [HttpGet("pending")]
    public ActionResult<List<TaskItem>> GetPendingTasks()
    {
        List<TaskItem> pendingTasks = Tasks
            .Where(task => !task.IsCompleted)
            .ToList();

        return Ok(pendingTasks);
    }

    [HttpPost]
    public ActionResult<TaskItem> CreateTask(CreateTaskDto request)
    {
        TaskItem task = new()
        {
            Id = _nextId++,
            Title = request.Title,
            Description = request.Description,
            IsCompleted = false,
            CreatedAt = DateTime.UtcNow
        };

        Tasks.Add(task);

        return CreatedAtAction(
            nameof(GetTaskById),
            new { id = task.Id },
            task
        );
    }

    [HttpPatch("{id:int}/complete")]
    public ActionResult<TaskItem> CompleteTask(int id)
    {
        TaskItem? task = Tasks.FirstOrDefault(task => task.Id == id);

        if (task is null)
        {
            return NotFound(new
            {
                message = $"Task with ID {id} was not found."
            });
        }

        task.IsCompleted = true;

        return Ok(task);
    }

    [HttpGet]
    public ActionResult<List<TaskItem>> GetAllTasks(
        [FromQuery] string? search
    )
    {
        if (string.IsNullOrWhiteSpace(search))
        {
            return Ok(Tasks);
        }

        List<TaskItem> matchingTasks = Tasks
            .Where(task =>
                task.Title.Contains(
                    search,
                    StringComparison.OrdinalIgnoreCase
                ))
            .ToList();

        return Ok(matchingTasks);
    }

    [HttpPut("{id:int}")]
    public ActionResult<TaskItem> UpdateTask(
        int id,
        UpdateTaskDto request
    )
    {
        TaskItem? task = Tasks.FirstOrDefault(task => task.Id == id);

        if (task is null)
        {
            return NotFound(new
            {
                message = $"Task with ID {id} was not found."
            });
        }

        task.Title = request.Title;
        task.Description = request.Description;
        task.IsCompleted = request.IsCompleted;

        return Ok(task);
    }

    [HttpDelete("{id:int}")]
    public IActionResult DeleteTask(int id)
    {
        TaskItem? task = Tasks.FirstOrDefault(task => task.Id == id);

        if (task is null)
        {
            return NotFound(new
            {
                message = $"Task with ID {id} was not found."
            });
        }

        Tasks.Remove(task);

        return NoContent();
    }
}