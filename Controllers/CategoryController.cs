using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentWork;
using StudentWork.Models;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http;


namespace StudentWork.Controllers
{
    public class CategoryController : Controller
    {
        private readonly StudentWorkContext _context;
        //private readonly IHostingEnvironment hostingEnvironment;
        private readonly IWebHostEnvironment _hostingEnvironment;
        //public CategoryController(IWebHostEnvironment environment)
        //{
        //    hostingEnvironment = environment;
        //}
        public CategoryController(StudentWorkContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _hostingEnvironment = environment;
        }

        // GET: Category
        public async Task<IActionResult> Index()
        {
            return View(await _context.categories.ToListAsync());
        }

        // GET: Category/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Category/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ImageFile,Name,Description")] Category category)
        {
            if (ModelState.IsValid)
            {
                string wwwPath = this._hostingEnvironment.WebRootPath;
                string contentPath = this._hostingEnvironment.ContentRootPath;

                string path = Path.Combine(this._hostingEnvironment.WebRootPath, "Uploads");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string fileName = Path.GetFileNameWithoutExtension(category.ImageFile.FileName);
                string extension = Path.GetExtension(category.ImageFile.FileName);
                string fileNameFull = fileName + extension;
                category.Image = fileName + extension;
                if (fileName != null)
                {
                    using (FileStream stream = new FileStream(Path.Combine(path, fileNameFull), FileMode.Create))
                    {
                        await category.ImageFile.CopyToAsync(stream);
                    }
                }

                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Category/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Category/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ImageFile,Name,Description")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    string wwwPath = this._hostingEnvironment.WebRootPath;
                    string contentPath = this._hostingEnvironment.ContentRootPath;

                    string path = Path.Combine(this._hostingEnvironment.WebRootPath, "Uploads");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    string fileName = Path.GetFileNameWithoutExtension(category.ImageFile.FileName);
                    string extension = Path.GetExtension(category.ImageFile.FileName);
                    string fileNameFull = fileName + extension;
                    category.Image = fileName + extension;
                    if (fileName != null)
                    {
                        using (FileStream stream = new FileStream(Path.Combine(path, fileNameFull), FileMode.Create))
                        {
                            await category.ImageFile.CopyToAsync(stream);
                        }
                    }


                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
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
            return View(category);
        }

        // GET: Category/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.categories.FindAsync(id);
            var imagePath = Path.Combine(_hostingEnvironment.WebRootPath, "Uploads", category.Image);
            if (System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);

            _context.categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.categories.Any(e => e.Id == id);
        }
    }
}
