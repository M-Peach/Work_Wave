using System.ComponentModel.DataAnnotations;

namespace Work_Wave.Models
{
    public class Comment
    {
        public int Id { get; set; }

        [Display(Name = "Ticket Id")]
        public int TicketId { get; set; }

        [Display(Name = "Comment")]
        public string Note { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Comment Date")]
        public DateTimeOffset Created { get; set; }

        [Display(Name = "User")]
        public string UserId { get; set; }


        //Nav props

        public virtual Ticket Ticket { get; set; }

        public virtual WaveUser User { get; set; }
    }
}
