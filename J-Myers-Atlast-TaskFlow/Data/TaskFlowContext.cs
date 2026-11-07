using Microsoft.EntityFrameworkCore;
using J_Myers_Atlast_TaskFlow.Models;

namespace J_Myers_Atlast_TaskFlow.Data
{
    public class TaskFlowContext : DbContext
    {
        public TaskFlowContext(DbContextOptions<TaskFlowContext> options)
            : base(options)
        {
        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<AuthorizationTask> AuthorizationTasks { get; set; }
    }
}