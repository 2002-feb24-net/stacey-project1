using System;
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
    public class NuggetStoresController : Controller
    {
        private readonly CornNuggetsContext _context;

        public NuggetStoresController(CornNuggetsContext context)
        {
            _context = context;
        }

        // GET: NuggetStores
        public async Task<IActionResult> Index()
        {
            
             
            return View(await _context.NuggetStores.ToListAsync());
        }

        // GET: NuggetStores/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id < 0)
            {
                return NotFound();
            }

            var nuggetStores = await _context.NuggetStores
                .FirstOrDefaultAsync(m => m.StoreId == id);
            if (nuggetStores == null)
            {
                return NotFound();
            }
            return View(nuggetStores);
        }

        // GET: NuggetStores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: NuggetStores/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StoreId,StoreName,StoreLocation")] NuggetStores nuggetStores)
        {
            if (ModelState.IsValid)
            {
                _context.Add(nuggetStores);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(nuggetStores);
        }

        // GET: NuggetStores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nuggetStores = await _context.NuggetStores.FindAsync(id);
            if (nuggetStores == null)
            {
                return NotFound();
            }
            return View(nuggetStores);
        }

        // POST: NuggetStores/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StoreId,StoreName,StoreLocation")] NuggetStores nuggetStores)
        {
            if (id != nuggetStores.StoreId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nuggetStores);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NuggetStoresExists(nuggetStores.StoreId))
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
            return View(nuggetStores);
        }

        // GET: NuggetStores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nuggetStores = await _context.NuggetStores
                .FirstOrDefaultAsync(m => m.StoreId == id);
            if (nuggetStores == null)
            {
                return NotFound();
            }

            return View(nuggetStores);
        }

        // POST: NuggetStores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var nuggetStores = await _context.NuggetStores.FindAsync(id);
            _context.NuggetStores.Remove(nuggetStores);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NuggetStoresExists(int id)
        {
            return _context.NuggetStores.Any(e => e.StoreId == id);
        }
    }
}
