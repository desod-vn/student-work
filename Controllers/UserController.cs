using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentWork;
using StudentWork.Models;
using System.Security.Cryptography;
using System.Text;

namespace StudentWork.Controllers
{
    public class UserController : Controller
    {
        private readonly StudentWorkContext _context;

        public UserController(StudentWorkContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.users.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Email,Name,Password")] User user)
        {
            if (UserEmailExists(user.Email)) {
                ViewBag.Message = "Email is existed.";
            } else {
                user.Password = MD5Hash(user.Password);
                if (ModelState.IsValid)
                {
                    _context.Add(user);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(user);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Email,Name,Password")] User user)
        {
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
                return RedirectToAction(nameof(Index));
            }
            
            return View(user);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.users.FindAsync(id);
            _context.users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.users.Any(e => e.Id == id);
        }

        private bool UserEmailExists(string email)
        {
            return _context.users.Any(e => e.Email == email);
        }

        public static string MD5Hash(string text)  
        {  
            MD5 md5 = new MD5CryptoServiceProvider();  

            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));
        
            byte[] result = md5.Hash;  

            StringBuilder strBuilder = new StringBuilder();  
            for (int i = 0; i < result.Length; i++)  
            {
                strBuilder.Append(result[i].ToString("x2"));  
            }  

            return strBuilder.ToString();  
        }
    }
}
