using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace StudentAndEmployeeAppsMVC.Models.DataAccess
{
    public class AcademicRecordsMetaData
    {
        [Display(Name = "Course")]
        public virtual Course CourseCodeNavigation { get; set; } = null!;

        [Required(ErrorMessage ="You must enter a grade")]
        [Range(0,100,ErrorMessage ="You must enter between 0-100")]
        public int? Grade { get; set; }
    }

}
