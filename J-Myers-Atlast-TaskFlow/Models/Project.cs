using System.ComponentModel.DataAnnotations;

namespace J_Myers_Atlast_TaskFlow.Models
{
    public class Project
    {
        [Key]
        public int ProjectId { get; set; }

        [Required(ErrorMessage = "Project name is required.")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation property for related tasks
        public ICollection<AuthorizationTask> Tasks { get; set; } = new List<AuthorizationTask>();
    }
}