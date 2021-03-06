﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CornNuggets.DataAccess;
using CornNuggets.DataAccess.Models;
using CornNuggets.DataAccess.Repositories;

namespace CornNuggets.WebUI.Controllers
{
    public class CustomersController : Controller
    {
        private readonly CornNuggetsContext _context;
        readonly CornNuggetsRepository repository = new CornNuggetsRepository(); 
        public CustomersController(CornNuggetsContext context)
        {
            _context = context;
            repository = new CornNuggetsRepository(_context);
        }

        // GET: Customers
        public async Task<IActionResult> Index()
        {
            
            return View(await _context.Customers.ToListAsync());
        }
        public IActionResult Search(string fname, string lname)
        {
            var listing = from Customers in _context.Customers
                          where Customers.FirstName == fname
                          where Customers.LastName == lname
                          select Customers;
            return View(listing);
        }
        // GET: Customers/Details/5

        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            var customers = await _context.Customers
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customers == null)
            {
                return NotFound();
            }
            
            return View(customers);
        }

        // GET: Customers/Create
        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]        
        public async Task<IActionResult> Create([Bind("FirstName,LastName,PreferredStore")] Customers customer)
        {
            if (ModelState.IsValid)
            {
                
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return Redirect("/");
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customers = await _context.Customers.FindAsync(id);
            if (customers == null)
            {
                return NotFound();
            }
            ViewData["StoreName"] = new SelectList(_context.NuggetStores, "PreferredStore", "StoreName", customers.PreferredStore);
            return View(customers);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerId,FirstName,LastName,PreferredStore")] Customers customers)
        {
            if (id != customers.CustomerId)
            {
                return NotFound();
            }
            
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customers);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomersExists(customers.CustomerId))
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
            ViewData["StoreName"] = new SelectList(_context.NuggetStores, "PreferredStore", "StoreName", customers.PreferredStore);
            return View(customers);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customers = await _context.Customers
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customers == null)
            {
                return NotFound();
            }

            return View(customers);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customers = await _context.Customers.FindAsync(id);
            _context.Customers.Remove(customers);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomersExists(int id)
        {
            return _context.Customers.Any(e => e.CustomerId == id);
        }
    }
}
