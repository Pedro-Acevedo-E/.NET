using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Login.Models;
using LoginContacts.Data;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace LoginContacts.Controllers {
    public class UserController : Controller {
        private readonly LoginContactsContext _context;

        public UserController(LoginContactsContext context)
        {
            _context = context;
        }

        // GET: User
        public async Task<IActionResult> Index()
        {
            Console.WriteLine("current session: " + HttpContext.Session.GetString("_UserSession"));
            if(UserIsLoggedIn()) {
                return View(await _context.User.ToListAsync());
            }

            return RedirectToAction(nameof(Login));
        }

        public async Task<IActionResult> Login()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Remove("_UserSession");
            return RedirectToAction(nameof(Login));
        }

        [HttpPost]
        public async Task<IActionResult> Login([Bind("Id,Email,Password")] User user)
        {
            var usr = await _context.User
                .FirstOrDefaultAsync(m => 
                    m.Email == user.Email && m.Password == user.Password
                );

            if (usr == null) {
                Console.WriteLine("No user found");
            } else {
                HttpContext.Session.SetString("_UserSession", usr.Id.ToString());
                return RedirectToAction("Details", new {id = usr.Id});
            }
            
            return View();
        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if(UserIsLoggedIn()) {
                if (id == null)
                {
                    return NotFound();
                }

                var user = await _context.User
                    .Include(u => u.Contacts) 
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (user == null)
                {
                    return NotFound();
                }

                return View(user);
            }

            return RedirectToAction(nameof(Login));
        }

        // GET: User/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Email,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();

                var usr = await _context.User
                    .FirstOrDefaultAsync(m => m.Email == user.Email);

                for (int i = 0; Request.Form.ContainsKey($"NewContacts.{i}.Name"); i++) {
                    Contact contact = new Contact();
                    contact.Name = Request.Form[$"NewContacts.{i}.Name"];
                    contact.LastName = Request.Form[$"NewContacts.{i}.LastName"];
                    contact.PhoneNumber = Request.Form[$"NewContacts.{i}.PhoneNumber"];
                    contact.UserId = usr.Id;
                     _context.Add(contact);
                    await _context.SaveChangesAsync();
                }
                
                return RedirectToAction(nameof(Login));
            }
            return View(user);
        }

        // GET: User/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if(UserIsLoggedIn()) {
                if (id == null)
                {
                    return NotFound();
                }

                var user = await _context.User
                    .Include(u => u.Contacts) 
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (user == null)
                {
                    return NotFound();
                }
                
                return View(user);
            }
            
            return RedirectToAction(nameof(Login));
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Email,Password")] User user)
        {
            if(UserIsLoggedIn()) {
                if (id != user.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(user);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!UserExists(user.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction("Details", new {id = id});
                }
                return View(user);
            }
            return RedirectToAction(nameof(Login));
        }

        //GET
        public async Task<IActionResult> EditContact(int? id) {
            if(UserIsLoggedIn()) {
                if (id == null)
                {
                    return NotFound();
                }

                var contact = await _context.Contact
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (contact == null)
                {
                    return NotFound();
                }
                
                return View(contact);
            }

            return RedirectToAction(nameof(Login));
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditContact(int id, [Bind("Id,Name,LastName,PhoneNumber,UserId")] Contact contact) {
            if(UserIsLoggedIn()) {
                if (id != contact.Id)
                {
                    return NotFound();
                }

                try
                {
                    _context.Update(contact);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactExists(contact.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction("Edit", new {id = contact.UserId});
            }

            return RedirectToAction(nameof(Login));
        }

        // GET
        public async Task<IActionResult> DeleteContact(int? id)
        {
            if(UserIsLoggedIn()) {
                if (id == null)
                {
                    return NotFound();
                }

                var contact = await _context.Contact
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (contact == null)
                {
                    return NotFound();
                }

                return View(contact);
            }

            return RedirectToAction(nameof(Login));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteContact(int id) {
            if(UserIsLoggedIn()) {
                var contact = await _context.Contact
                    .FindAsync(id);
                
                if (contact != null)
                {
                    _context.Contact.Remove(contact);
                }

                await _context.SaveChangesAsync();

                return RedirectToAction("Edit", new {id = contact.UserId});
            }

            return RedirectToAction(nameof(Login));
        }
        
        public IActionResult CreateContact(int? id)
        {
            if(UserIsLoggedIn()) {
                if (id == null)
                {
                    return NotFound();
                }

                Console.WriteLine("Create contact for: " + id);

                ViewData["CurrentUser"] = id;
                return View();
            }

            return RedirectToAction(nameof(Login));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateContact([Bind("Id,Name,LastName,PhoneNumber,UserId")] Contact contact)
        {
            if(UserIsLoggedIn()) {
                Contact ct = new Contact();
                ct.Name = contact.Name;
                ct.LastName = contact.LastName;
                ct.PhoneNumber = contact.PhoneNumber;
                ct.UserId = contact.Id;
                
                _context.Add(ct);
                await _context.SaveChangesAsync();
                
                return RedirectToAction("Edit", new {id = ct.UserId});
            }

            return RedirectToAction(nameof(Login));
        }

        
        
        // GET: User/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if(UserIsLoggedIn()) {
                if (id == null)
                {
                    return NotFound();
                }

                var user = await _context.User
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (user == null)
                {
                    return NotFound();
                }

                return View(user);
            }

            return RedirectToAction(nameof(Login));
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if(UserIsLoggedIn()) {
                var user = await _context.User
                    .FindAsync(id);
                if (user != null)
                {
                    _context.User.Remove(user);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Login));
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }

        private bool ContactExists(int id)
        {
            return _context.Contact.Any(e => e.Id == id);
        }

        private bool UserIsLoggedIn() {
            return (HttpContext.Session.GetString("_UserSession") != null);
        }
    }
}
