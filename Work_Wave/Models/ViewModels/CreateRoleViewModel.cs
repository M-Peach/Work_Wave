using System.ComponentModel.DataAnnotations;

namespace Work_Wave.Models.ViewModels
{
    public class CreateRoleViewModel
    {
        [Required]
        public string RoleName { get; set;}
    }
}
