using System;
using System.Collections.Generic;

namespace StudentAndEmployeeAppsMVC.Models.DataAccess
{
    public partial class Student
    {
        public Student()
        {
            AcademicRecords = new HashSet<AcademicRecord>();
            CourseCourses = new HashSet<Course>();
        }

        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;

        public string NameWithId { get { return Id + " - " + Name ; } }

        public virtual ICollection<AcademicRecord> AcademicRecords { get; set; }

        public virtual ICollection<Course> CourseCourses { get; set; }
    }
}
