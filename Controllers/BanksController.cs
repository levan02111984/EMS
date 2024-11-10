using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EMS.Models;
using EMS.Data;
using EMS.Repositories;

namespace EMS.Controllers
{
    public class BanksController : Controller
    {
        private readonly IGenericRepository<Bank> _respository;

        public BanksController(IGenericRepository<Bank> responsitory)
        {
            _respository = responsitory;
        }

        // GET: Banks
        public async Task<IActionResult> Index()
        {
            var bankList = await _respository.GetAllAsync();
            return View(bankList);
        }

        // GET: Banks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bank = await _respository.GetByIdAsync(id);
            if (bank == null)
            {
                return NotFound();
            }

            return View(bank);
        }

        // GET: Banks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Banks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Code,Name,AccountNo,CreatedByID,CreatedOn,ModifiedBy,ModifiedOn")] Bank bank)
        {
            if (ModelState.IsValid)
            {
                await _respository.InsertAsync(bank);
                await _respository.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bank);
        }

        // GET: Banks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bank = await _respository.GetByIdAsync(id);
            if (bank == null)
            {
                return NotFound();
            }
            return View(bank);
        }

        // POST: Banks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Code,Name,AccountNo,CreatedByID,CreatedOn,ModifiedBy,ModifiedOn")] Bank bank)
        {
            if (id != bank.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _respository.UpdateAsync(bank);
                    await _respository.SaveAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    var objBank = await _respository.GetByIdAsync(bank.Id);
                    if (objBank==null)
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
            return View(bank);
        }

        // GET: Banks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bank = await _respository.GetByIdAsync(id);
          
            if (bank == null)
            {
                return NotFound();
            }

            return View(bank);
        }

        // POST: Banks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bank = await _respository.GetByIdAsync(id);
            if (bank != null)
            {
                await _respository.DeleteAsync(bank);
                await _respository.SaveAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
