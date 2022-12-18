namespace StudentAndEmployeeAppsMVC.Models.DataAccess
{
    public partial class Course
    {
        public string codeWithName { get { return Code + " - " + Title; } }
    }
}
