using Collection_Management.DataBaseContext;
using Collection_Management.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Collection_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ProjectTaskContext _context;

        public TasksController(ProjectTaskContext context)
        {
            _context = context;
        }

        
        [HttpPost]
        public async Task<ActionResult<Task>> AddTask([FromBody] Tasks task)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var project = await _context.Projects.FindAsync(task.ProjectId);
            if (project == null) return NotFound("Project not found.");

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return Ok(new { id = task.Id });
        }

        
        [HttpPut]
        public async Task<IActionResult> UpdateTask([FromBody] Tasks updatedTask)
        {
            if (updatedTask == null || updatedTask.Id == 0)
                return BadRequest("Invalid task data.");

            var task = await _context.Tasks.FindAsync(updatedTask.Id);
            if (task == null)
                return NotFound("Task not found.");

            
            task.Title = updatedTask.Title;
            task.Description = updatedTask.Description;
            task.DueDate = updatedTask.DueDate;
            task.IsCompleted = updatedTask.IsCompleted;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null) return NotFound();

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }

}
