using System.ComponentModel.DataAnnotations;

namespace Work_Wave.Models
{
    public class Priority
    {
        public int Id { get; set; }

        [Display(Name = "Priority Name")]
        public string Name { get; set; }
    }
}
