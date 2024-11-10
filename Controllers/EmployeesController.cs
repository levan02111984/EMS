using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EMS.Data;
using EMS.Models;
using EMS.Repositories;
using System.Net;
using EMS.ViewModel;

namespace EMS.Controllers
{
    public class EmployeesController : Controller
    {

        //private UnitOfWork unitOfWork = new UnitOfWork();
        private readonly IGenericRepository<Employee> _repository;
        private readonly IGenericRepository<Country> _repositoryCountry;
        private ApplicationDbContext _dbcontext;

        public EmployeesController(IGenericRepository<Employee> repository,ApplicationDbContext dbContext)
        {
            _repository = repository;
            _dbcontext = dbContext;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
           
            var employeeList = await _repository.GetAllAsync();

            //Apply UnitOfWork
            //var employeeList =await unitOfWork.EmployeeRepository.GetAllAsync
            //
            //var designationList = await _dbcontext.Designations.ToListAsync();
            //var coutryList = await _dbcontext.Countries.ToListAsync();
            //var departmentList = await _dbcontext.Departments.ToListAsync();


            //var employeeViewModel = new EmployeeViewModel
            //{
            //    Employees = employeeList,
            //    CountryName = this.GetStringName<Country>(coutryList, employee.Id),
            //    DesignationName = this.GetStringName<Designation>(designationList, employee.Id),
            //    DepartmentName = this.GetStringName<Department>(departmentList, employee.Id),
            //};

            
            //Get Result from 4 table and Map the result to EmployeeViewModel
            var resultEmployeeView = await (from emp in _dbcontext.Employees
                       from cty in _dbcontext.Countries
                       from dep in _dbcontext.Departments
                       from des in _dbcontext.Designations
                       where emp.CountryID == cty.Id &&
                       emp.DepartmentID == dep.Id &&
                       emp.DesignationID == des.Id
                       select new EmployeeViewModel
                       {
                           Employee = emp,
                           CountryName = cty.Name,
                           DepartmentName = dep.Name,
                           DesignationName = des.Name
                       }).ToListAsync();





            return View(resultEmployeeView);
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
         
            }

            var employee = await _repository.GetByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        public async Task<IActionResult> Create()
        {
            //Binding Data to Dropdownlist
            var designationList = await _dbcontext.Designations.ToListAsync();
            var coutryList = await _dbcontext.Countries.ToListAsync();
            var departmentList = await _dbcontext.Departments.ToListAsync();

            ViewData["viewDataDesignation"] = new SelectList(designationList, "Id", "Name");
            ViewData["viewDataCountry"] = new SelectList(coutryList, "Id", "Name");
            ViewData["viewDataDepartment"] = new SelectList(departmentList, "Id", "Name");

            return View();
        }

        // POST: Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                employee.CreatedByID = "Van";
                employee.CreatedOn = DateTime.Now;
               // employee.Department = 

                await _repository.InsertAsync(employee);
                await _repository.SaveAsync(); // Commit to Database
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var designationList = await _dbcontext.Designations.ToListAsync();
            var coutryList = await _dbcontext.Countries.ToListAsync();
            var departmentList = await _dbcontext.Departments.ToListAsync();

            ViewData["viewDataDesignation"] = new SelectList(designationList, "Id", "Name");
            ViewData["viewDataCountry"] = new SelectList(coutryList, "Id", "Name");
            ViewData["viewDataDepartment"] = new SelectList(departmentList, "Id", "Name");


            if (id == null)
            {
                return NotFound();
            }

            var employee = await _repository.GetByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        public string GetStringName<T>(List<T> objectList, int id)
        {
            string valueReturn;
            if (objectList == null || objectList.Count == 0)
            {
                return string.Empty;
            }
            switch (objectList)
            {
                case Country:
                    valueReturn =  objectList.OfType<Country>().FirstOrDefault(p => p.Id == id)?.Name ?? string.Empty;
                    break;
                case Department:
                    valueReturn = objectList.OfType<Department>().FirstOrDefault(p => p.Id == id)?.Name ?? string.Empty;
                    break;
                case Designation:
                    valueReturn  = objectList.OfType<Designation>().FirstOrDefault(p => p.Id == id)?.Name ?? string.Empty;
                    break;
                default: valueReturn = string.Empty; break;
            }
            return valueReturn;
        }

        // POST: Employees/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,Employee employee)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _repository.UpdateAsync(employee);
                    await _repository.SaveAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    var emp = await _repository.GetByIdAsync(employee.Id);
                    if (emp==null)
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
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _repository.GetByIdAsync(id);
                
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
            var employee = await _repository.GetByIdAsync(id);
            if (employee != null)
            {
                await _repository.DeleteAsync(employee);
                await _repository.SaveAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        
    }
}
