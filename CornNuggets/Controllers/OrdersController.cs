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
    public class OrdersController : Controller
    {
        private readonly CornNuggetsContext _context;

        public OrdersController(CornNuggetsContext context)
        {
            _context = context;
            var repo = new CornNuggetsRepository();

        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var cornNuggetsContext = _context.Orders.Include(o => o.Customer).Include(o => o.Store).Include(o=> o.OrderLog);
            return View(await cornNuggetsContext.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Form(int? orderId)
        {
            if (orderId==null)
            {
                return NotFound();
            }
            var orders = await _context.Orders
                .Include(s => s.StoreId)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.CustomerId == orderId);

            if (orders ==null)
            {
                return NotFound();
            }    
            //var byId = new FormViewModel();
            //var result = byId.Repo.GetOrdersDetails(orderId);
            return View(orders);
        }
        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orders = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Store)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (orders == null)
            {
                return NotFound();
            }

            return View(orders);
        }
        [HttpPost]
        public IActionResult Search(int custId)
        {
            var listing = from Orders in _context.Orders
                          where Orders.CustomerId == custId
                          select Orders;
            return View(listing);
        }

        public IActionResult Stores(int store)
        {
            var listing = from Orders in _context.Orders
                          where Orders.StoreId == store
                          select Orders;

            ViewData["orderId"] = new SelectList(_context.Orders, "orderId", "Order ID:");
            return View(listing);
        }


        // GET: Orders/Create
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Orders, "CustomerId", "Customer ID:");
            ViewData["StoreId"] = new SelectList(_context.NuggetStores, "StoreId", "StoreId");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,DateTimeStamp,StoreId,CustomerId,Total")] Orders orders)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orders);
                
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Orders, "CustomerId", "CustomerId", orders.CustomerId);
            ViewData["StoreId"] = new SelectList(_context.Orders, "StoreId", "StoreId", orders.StoreId);
            return View(orders);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orders = await _context.Orders.FindAsync(id);
            if (orders == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "PreferredStore", orders.CustomerId);
            ViewData["StoreId"] = new SelectList(_context.NuggetStores, "StoreId", "StoreName", orders.StoreId);
            return View(orders);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,DateTimeStamp,StoreId,CustomerId,Total")] Orders orders)
        {
            if (id != orders.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orders);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrdersExists(orders.OrderId))
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
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "PreferredStore", orders.CustomerId);
            ViewData["StoreId"] = new SelectList(_context.NuggetStores, "StoreId", "StoreName", orders.StoreId);
            return View(orders);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orders = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Store)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (orders == null)
            {
                return NotFound();
            }

            return View(orders);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orders = await _context.Orders.FindAsync(id);
            _context.Orders.Remove(orders);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrdersExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }
    }
}
