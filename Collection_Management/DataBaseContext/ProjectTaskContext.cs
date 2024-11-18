namespace Collection_Management.DataBaseContext
{
    using Collection_Management.Models;
    using Microsoft.EntityFrameworkCore;

    public class ProjectTaskContext : DbContext
    {
        public DbSet<Project> Projects { get; set; }
        public DbSet<Tasks> Tasks { get; set; }

        public ProjectTaskContext(DbContextOptions<ProjectTaskContext> options) : base(options) { }

       
    }

}
