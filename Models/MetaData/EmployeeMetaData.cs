using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace StudentAndEmployeeAppsMVC.Models.DataAccess
{
    public class EmployeeMetaData
    {
        [Display(Name = "Employee ID")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Employee name is required.")]
        [RegularExpression(@"[a-zA-Z]+\s+[a-zA-Z]+", ErrorMessage = "Must be in the form of first name followed by last name")]
        [Display(Name = "Employee Name")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Network ID is required.")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Network ID need to be more than 3 characters.")]
        [Display(Name = "Network ID")]
        public string UserName { get; set; } = null!;

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "Password need to be more than 5 characters.")]
        public string Password { get; set; } = null!;
        [Display(Name = "Job Title(s)")]
        public virtual ICollection<Role> Roles { get; set; }
    }
}
