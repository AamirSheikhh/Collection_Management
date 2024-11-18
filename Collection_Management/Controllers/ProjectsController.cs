using Collection_Management.DataBaseContext;
using Collection_Management.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Collection_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly ProjectTaskContext _context;

        public ProjectsController(ProjectTaskContext context)
        {
            _context = context;
        }

        
        [HttpPost]
        public async Task<ActionResult<Project>> CreateProject([FromBody] Project project)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return Ok(new { id = project.Id });
        }

        [HttpGet]
        public async Task<ActionResult> GetProjects()
        {
            var projectsWithTasks = await (from project in _context.Projects
                                           join task in _context.Tasks on project.Id equals task.ProjectId into taskGroup
                                           from task in taskGroup.DefaultIfEmpty() 
                                           select new
                                           {
                                               Project = project,
                                               Task = task 
                                           }).ToListAsync();

            var result = projectsWithTasks
                .GroupBy(p => new
                {
                    p.Project.Id,
                    p.Project.Title,
                    p.Project.Description,
                    p.Project.Status
                })
                .Select(g => new ProjectWithTasksDto
                {
                    Id = g.Key.Id,
                    Title = g.Key.Title,
                    Description = g.Key.Description,
                    Status = g.Key.Status,
                    Tasks = g.Select(t => t.Task != null ? new TaskDto
                    {
                        Id = t.Task.Id,
                        Title = t.Task.Title,
                        Description = t.Task.Description,
                        DueDate = t.Task.DueDate,
                        IsCompleted = t.Task.IsCompleted
                    } : null).ToList() 
                }).ToList();

            return Ok(result);
        }
    }
    }

    
