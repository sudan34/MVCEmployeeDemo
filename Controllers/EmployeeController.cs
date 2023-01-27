using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCEmployeeDemo.Data;
using MVCEmployeeDemo.Models;

namespace MVCEmployeeDemo.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext applicationDbContext;

        public EmployeeController(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var employee = await applicationDbContext.employees.ToListAsync();
            return View(employee);
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddEmployeeViewModel addEmployeeRequest)
        {
            var employee = new Employee()
            {
                Id = Guid.NewGuid(),
                Name = addEmployeeRequest.Name,
                Email = addEmployeeRequest.Email,
                Salary = addEmployeeRequest.Salary,
                DateOfBirth = addEmployeeRequest.DateOfBirth,
                Department = addEmployeeRequest.Department,
            };
            await applicationDbContext.employees.AddAsync(employee);
            await applicationDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> View(Guid id)
        {
            var employee = await applicationDbContext.employees.FirstOrDefaultAsync(x => x.Id == id);
            if (employee != null)
            {
                var viewModel = new UpdateEmployeeViewModel()
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Email = employee.Email,
                    Salary = employee.Salary,
                    DateOfBirth = employee.DateOfBirth,
                    Department = employee.Department,
                };
                return await Task.Run(() => View("View", viewModel));
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> View(UpdateEmployeeViewModel model)
        {
            var employee = await applicationDbContext.employees.FindAsync(model.Id);
            if (employee != null)
            {
                employee.Name = model.Name;
                employee.Email = model.Email;
                employee.Salary = model.Salary;
                employee.DateOfBirth= model.DateOfBirth;
                employee.Department = model.Department;

                await applicationDbContext.SaveChangesAsync();
                return RedirectToAction("Index");   
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UpdateEmployeeViewModel model)
        {
            var employee =await applicationDbContext.employees.FindAsync(model.Id);
            if (employee != null)
            {
                applicationDbContext.employees.Remove(employee);
                await applicationDbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

    }
}
