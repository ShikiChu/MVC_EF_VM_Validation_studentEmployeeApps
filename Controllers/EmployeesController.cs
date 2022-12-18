using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentAndEmployeeAppsMVC.Models.DataAccess;
using StudentAndEmployeeAppsMVC.Models.ViewModel;
using NuGet.Versioning;

namespace StudentAndEmployeeAppsMVC.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly StudentRecordContext _context;

        public EmployeesController(StudentRecordContext context)
        {
            _context = context;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            var employeeContext = await _context.Employees.Include(e=>e.Roles).ToListAsync();

            //foreach (var em in employeeContext)
            //{
            //    var rolesData = await _context.Roles.Where(e => e.Employees.Any(e => e.Id == em.Id)).ToListAsync();  // return the Role class obj with its em(employee id) equal to Role e ICollection Employee id
            //    // https://stackoverflow.com/questions/25728556/how-to-query-icollectiont

            //    foreach (Role role in rolesData)
            //    {
            //        em.Roles.Add(role);
            //    }
            //}

            return View(employeeContext); 
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.Include(e=>e.Roles).SingleOrDefaultAsync(e=>e.Id == id);

            // or below:
            //var employee = await _context.Employees
            //    .FirstOrDefaultAsync(m => m.Id == id);

            //var roleRecord = await _context.Roles.Where(e => e.Employees.Any(e => e.Id == id)).ToListAsync();
            //foreach (Role role in roleRecord)
            //{
            //    employee.Roles.Add(role);
            //}

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            EmployeeRoleSelections employeeRoleSelections = new EmployeeRoleSelections(); // constructor creates new Employee and List RoleSelection obj
            var allRocrd = _context.Roles.ToList(); // grab all the Role obj as a list
            foreach (Role role in allRocrd) 
            {
                RoleSelection roleSelection = new RoleSelection(role); // new RoleSelection obj, by default, not checked
                employeeRoleSelections.roleSelections.Add(roleSelection); // add the RoleSelection obj into the list
            }
            return View(employeeRoleSelections); 
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeRoleSelections employeeRoleSelections)
        {
            if (!employeeRoleSelections.roleSelections.Any(m=>m.Selected)) 
            {
                ModelState.AddModelError("roleSelections", "You must select at least one role!");
            }
            if (_context.Employees.Any(e=>e.UserName == employeeRoleSelections.employee.UserName && e.Id == employeeRoleSelections.employee.Id)) 
            {
                ModelState.AddModelError("employee.UserName","This id is already used.");
            }
            if (ModelState.IsValid)
            {
                foreach (RoleSelection roleSelection in employeeRoleSelections.roleSelections)
                {
                    if (roleSelection.Selected)
                    {
                        Role role = _context.Roles.SingleOrDefault(r => r.Id == roleSelection.role.Id);
                        employeeRoleSelections.employee.Roles.Add(role);
                    }
                }
                _context.Add(employeeRoleSelections.employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(employeeRoleSelections);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id) // asp-route-id from index page
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id); // particular employee 
            var AssignedRole = _context.Roles.Where(r => r.Employees.Any(r => r.Id == id)).ToList(); // particular role record from employee
            var allRoles = _context.Roles.ToList(); // all the role from db

            EmployeeRoleSelections employeeRoleSelections = new EmployeeRoleSelections(employee, allRoles);

            // pre-checked 
            foreach (RoleSelection roleSelection in employeeRoleSelections.roleSelections)
            {
                foreach (Role r in AssignedRole)
                {
                    if (roleSelection.role.Id == r.Id)
                    {
                        roleSelection.Selected = true;
                    }
                }
            }

            if (employee == null)
            {
                return NotFound();
            }
            return View(employeeRoleSelections);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EmployeeRoleSelections employeeRoleSelections)
        {
            if (employeeRoleSelections == null) 
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                Employee employee = await _context.Employees.Include(e=>e.Roles).SingleOrDefaultAsync(e=>e.Id == employeeRoleSelections.employee.Id);
                employee.Roles.Clear();
                foreach (RoleSelection roleSelection in employeeRoleSelections.roleSelections) 
                {
                    if (roleSelection.Selected) 
                    {
                        var role = _context.Roles.SingleOrDefault(r => r.Id == roleSelection.role.Id);
                        employee.Roles.Add(role);
                    }
                }
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(employeeRoleSelections);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.Include(e => e.Roles).SingleOrDefaultAsync(e=>e.Id.Equals(id));

            // Or below:
            //var employee = await _context.Employees
            //    .FirstOrDefaultAsync(m => m.Id == id);
            //var roleRecord = await _context.Roles.Where(e=>e.Employees.Any(e=>e.Id == id)).ToListAsync();

            //foreach (Role role in roleRecord) 
            //{
            //    employee.Roles.Add(role);
            //}
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Employees == null)
            {
                return Problem("Entity set 'StudentRecordContext.Employees'  is null.");
            }
            var employee = _context.Employees.Include(e=>e.Roles).SingleOrDefault(e=>e.Id == id);
            if (employee != null)
            {
                employee.Roles.Clear();
                _context.Employees.Remove(employee);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
          return _context.Employees.Any(e => e.Id == id);
        }
    }
}
