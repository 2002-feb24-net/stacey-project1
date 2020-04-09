using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CornNuggets.DataAccess;
using CornNuggets.DataAccess.Models;

namespace CornNuggets.WebUI.Controllers
{
    public class OrderLogsController : Controller
    {
        private readonly CornNuggetsContext _context;

        public OrderLogsController(CornNuggetsContext context)
        {
            _context = context;
        }

        // GET: OrderLogs
        public async Task<IActionResult> Index()
        {
            var cornNuggetsContext = _context.OrderLog.Include(o => o.Order).Include(o => o.Product);
            return View(await cornNuggetsContext.ToListAsync());
        }

        // GET: OrderLogs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderLog = await _context.OrderLog
                .Include(o => o.Order)
                .Include(o => o.Product)
                .FirstOrDefaultAsync(m => m.LogId == id);
            if (orderLog == null)
            {
                return NotFound();
            }
            //orderLog.SubTotal = orderLog.ProductId *orderLog.ProductQty;
            return View(orderLog);
        }

        // GET: OrderLogs/Create
        public IActionResult Create()
        {
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId");
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId");
            return View();
        }

        // POST: OrderLogs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LogId,OrderId,ProductId,ProductQty,SubTotal")] OrderLog orderLog)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orderLog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId", orderLog.OrderId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", orderLog.ProductId);
            return View(orderLog);
        }
        [HttpPost]
        public IActionResult Search(int custId)
        {
            var listing = from Orders in _context.Orders
                          where Orders.CustomerId == custId
                          select Orders;
            
            return View(listing);
        }
        // GET: OrderLogs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderLog = await _context.OrderLog.FindAsync(id);
            if (orderLog == null)
            {
                return NotFound();
            }
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId", orderLog.OrderId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", orderLog.ProductId);
            
            return View(orderLog);
        }

        // POST: OrderLogs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LogId,OrderId,ProductId,ProductQty,SubTotal")] OrderLog orderLog)
        {
            if (id != orderLog.LogId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderLog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderLogExists(orderLog.LogId))
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
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId", orderLog.OrderId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", orderLog.ProductId);
            return View(orderLog);
        }

        // GET: OrderLogs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderLog = await _context.OrderLog
                .Include(o => o.Order)
                .Include(o => o.Product)
                .FirstOrDefaultAsync(m => m.LogId == id);
            if (orderLog == null)
            {
                return NotFound();
            }

            return View(orderLog);
        }

        // POST: OrderLogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orderLog = await _context.OrderLog.FindAsync(id);
            _context.OrderLog.Remove(orderLog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderLogExists(int id)
        {
            return _context.OrderLog.Any(e => e.LogId == id);
        }
    }
}
