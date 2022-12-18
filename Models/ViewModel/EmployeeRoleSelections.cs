using StudentAndEmployeeAppsMVC.Models.DataAccess;

namespace StudentAndEmployeeAppsMVC.Models.ViewModel
{
    public class EmployeeRoleSelections
    {
        public Employee employee { get; set; }
        public List<RoleSelection> roleSelections { get; set; }

        public EmployeeRoleSelections()
        {
            employee = new Employee();
            roleSelections = new List<RoleSelection>();
        }
        public EmployeeRoleSelections(Employee employee, List<Role> roles)
        {
            this.employee = employee;
            roleSelections = new List<RoleSelection>();
            foreach (Role role in roles)
            {
                RoleSelection roleSelection = new RoleSelection(role);
                if (employee.Roles.SingleOrDefault(r => r.Id == role.Id) != null)
                {
                    roleSelection.Selected = true;
                }
                roleSelections.Add(roleSelection);
            }
        }
    }
}