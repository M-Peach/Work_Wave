using System.ComponentModel.DataAnnotations;

namespace Work_Wave.Models
{
    public class Ticket
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Customer First Name")]
        public string CFirstName { get; set; }

        [Required]
        [Display(Name = "Customer Last Name")]
        public string CLastName { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Required]
        [Display(Name = "Customer Phone")]
        public string CPhone { get; set; }

        [Required]
        [Display(Name = "Customer Address")]
        public string CAddress { get; set; }

        [Required]
        [Display(Name = "Customer City")]
        public string CCity { get; set; }

        [Required]
        [Display(Name = "Customer State")]
        public string CState { get; set; }

        [Required]
        [Display(Name = "Customer Zip")]
        public string CZip { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Created")]
        public DateTimeOffset Created { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Sheduled")]
        public DateTimeOffset Schedule { get; set; }

        [Display(Name = "Archived")]
        public bool IsArchived { get; set; }


        // Below Referances Priority, Status, Comment

        [Display(Name = "Priority")]
        public int PriorityId { get; set; }

        [Display(Name = "Technician")]
        public string TechnicianId { get; set; }

        [Display(Name = "Technical Support")]
        public string SupportId { get; set; }

        // Navigation Properties

        public virtual Priority Priority { get; set; }

        public virtual WaveUser Technician { get; set; }

        public virtual WaveUser Support { get; set; }

        public virtual ICollection<Comment> Notes { get; set; }
    }
}
