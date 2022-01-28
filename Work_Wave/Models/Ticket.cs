using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [Display(Name = "First Name")]
        public string CFirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string CLastName { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Required]
        [Display(Name = "Phone")]
        public string CPhone { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string CAddress { get; set; }

        [Required]
        [Display(Name = "City")]
        public string CCity { get; set; }

        [Required]
        [Display(Name = "State")]
        public string CState { get; set; }

        [Required]
        [Display(Name = "Zip")]
        public string CZip { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Created")]
        public DateTimeOffset Created { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Sheduled")]
        public DateTimeOffset Schedule { get; set; }

        [Display(Name = "Archived")]
        public bool IsArchived { get; set; }

        [NotMapped]
        [Display(Name = "Phone Number")]
        public string FormatedPhone { get { return String.Format("{0:(###) ###-####}", Convert.ToInt64(CPhone)); } }
        [NotMapped]
        public string NumberStreet { get { return CAddress.Replace(" ", "+"); } }

        [NotMapped]
        [Display(Name = "Address Link")]
        public string AddressLink { get { return $"https://google.com/maps/search/{NumberStreet}+{CCity}+{CState}+{CZip}"; } }


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

        public virtual ICollection<Comment> Notes { get; set; } = new HashSet<Comment>();
    }
}
