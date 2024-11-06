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
        private readonly IGenericRepository<Employee> _repository;

        public EmployeesController(IGenericRepository<Employee> repository)
        {
            _repository = repository;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
           
            var employeeList = await _repository.GetAllAsync();
            
            return View(employeeList);
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
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EmpNo,FirstName,MiddleName,LastName,PhoneNUmber,EmailAddress,Country,DateOfBirth,Address,Department,Designation,CreatedByID,CreatedOn,ModifiedBy,ModifiedOn")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                employee.CreatedByID = "Van";
                employee.CreatedOn = DateTime.Now;

                await _repository.InsertAsync(employee);
                await _repository.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
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

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EmpNo,FirstName,MiddleName,LastName,PhoneNUmber,EmailAddress,Country,DateOfBirth,Address,Department,Designation,CreatedByID,CreatedOn,ModifiedBy,ModifiedOn")] Employee employee)
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
