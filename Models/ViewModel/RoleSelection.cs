using StudentAndEmployeeAppsMVC.Models.DataAccess;

namespace StudentAndEmployeeAppsMVC.Models.ViewModel
{
    public class RoleSelection
    {
        public Role role { get; set; }
        public bool Selected { get; set; }

        public RoleSelection()
        {
            Selected = false;
            role = null;
        }
        public RoleSelection(Role role, bool Selected = false)
        {
            this.Selected = Selected; // not checked at the beginning
            this.role = role;
        }
    }
}
