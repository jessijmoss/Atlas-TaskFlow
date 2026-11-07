using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace J_Myers_Atlast_TaskFlow.Models
{
    public class AuthorizationTask
    {
        [Key]
        public int TaskId { get; set; }

        // Alias property to support views looking for 'Id'
        [NotMapped]
        public int Id => TaskId;

        [Required(ErrorMessage = "Task title is required.")]
        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters.")]
        public string Title { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Patient case number is required.")]
        [Display(Name = "Patient Case Number")]
        public string PatientCaseNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "CPT code is required.")]
        [Display(Name = "CPT Code")]
        public string CptCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Authorization stage is required.")]
        public string Stage { get; set; } = "Pending";

        public string Notes { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [Display(Name = "Due Date")]
        public DateTime DueDate { get; set; } = DateTime.Now.AddDays(7);

        [Range(1, 5, ErrorMessage = "Priority must be between 1 and 5.")]
        public int Priority { get; set; } = 3;

        [Display(Name = "Completed")]
        public bool IsCompleted { get; set; } = false;

        // Foreign Key to Project
        [Display(Name = "Project")]
        public int ProjectId { get; set; }

        [ForeignKey("ProjectId")]
        public Project? Project { get; set; }
    }
}