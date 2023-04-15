#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookShopAsp.Data;
using BookShopAsp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace BookShopAsp.Controllers
{
    public class OrdersController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Orders
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Orders.Include(o => o.Book).Include(o => o.User);
            return View(await applicationDbContext.ToListAsync());
        }
        [Authorize]
        public async Task<IActionResult> MyOrders()
        {
            var user = await _userManager.GetUserAsync(User);
           List<Order> orders= await _context.Orders
                .Include(o => o.Book).Include(o => o.User)
                .Where(o=>o.UserId == Convert.ToString(user.Id)).ToListAsync();
            return View(orders);
        }
        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Book)
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["BookName"] = new SelectList(_context.Books, "Id", "Name");
            ViewData["UserName"] = new SelectList(_context.Users, "Id", "UserName");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,UserId,Address,BookId,Date")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookName"] = new SelectList(_context.Books, "Id", "Name");
            ViewData["UserName"] = new SelectList(_context.Users, "Id", "UserName");
            return View(order);
        }

        // GET: Orders/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["BookName"] = new SelectList(_context.Books, "Id", "Name");
            ViewData["UserName"] = new SelectList(_context.Users, "Id", "UserName");
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,Address,BookId,Date")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
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
            ViewData["BookName"] = new SelectList(_context.Books, "Id", "Name");
            ViewData["UserName"] = new SelectList(_context.Users, "Id", "UserName");
            return View(order);
        }

        // GET: Orders/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Book)
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
        public string UserName { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return View();
        }
        private async Task LoadAsync(IdentityUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);

            UserName = userName;
        }
        
        [Authorize]
        public async Task<IActionResult> MakeOrder(int bookId,  IdentityUser user)
        {
            
            var book = await _context.Books.FirstOrDefaultAsync(x=>x.Id == bookId);
            ViewData["bookName"] = book.Name + " " + book.Author;
            ViewData["userName"] = await _userManager.GetUserNameAsync(user);
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MakeOrder([Bind("Id,UserId,Address,BookId,Date")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookName"] = new SelectList(_context.Books, "Id", "Name");
            ViewData["UserName"] = new SelectList(_context.Users, "Id", "UserName");
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
