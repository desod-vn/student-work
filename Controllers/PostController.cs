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
    public class PostController : Controller
    {
        private readonly StudentWorkContext _context;
        //private readonly IHostingEnvironment hostingEnvironment;
        private readonly IWebHostEnvironment _hostingEnvironment;
        
        public PostController(StudentWorkContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _hostingEnvironment = environment;
        }
        public async Task<IActionResult> Index()
        {
            //var products = _context.categories.Include(p => p.Name).Include(p => p.Name);
            return View(await _context.posts.ToListAsync());
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var post = await _context.posts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }
        public IActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(_context.categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,ImageFile,Name,Content,PublishDate,CategoryId,UserId")] Post postModel)
        public async Task<IActionResult> Create([Bind("Id,ImageFile,Name,Content,PublishDate,CategoryId,UserId")] Post postModel)
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

                string fileName = Path.GetFileNameWithoutExtension(postModel.ImageFile.FileName);
                string extension = Path.GetExtension(postModel.ImageFile.FileName);
                string fileNameFull = fileName + extension;
                postModel.Image = fileName + extension;
                if (fileName != null)
                {
                    using (FileStream stream = new FileStream(Path.Combine(path, fileNameFull), FileMode.Create))
                    {
                        await postModel.ImageFile.CopyToAsync(stream);
                    }
                }
                
                //var date = postModel.PublishDate;
                //var aaa = postModel.CategoryId;
                //int user1 = postModel.UserId;
                //postModel.User = Int32.Parse();


                _context.Add(postModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            

            //ViewBag.CategoryId = new SelectList(_context.categories, "CategoryId", "Name", postModel.CategoryId);
            ViewBag.CategoryId = new SelectList(_context.categories, "Id", "Name");
            return View(postModel);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ImageFile,Name,Content,PublishDate,CategoryId,UserId")] Post postModel)
        {
            if (id != postModel.Id)
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

                    string fileName = Path.GetFileNameWithoutExtension(postModel.ImageFile.FileName);
                    string extension = Path.GetExtension(postModel.ImageFile.FileName);
                    string fileNameFull = fileName + extension;
                    postModel.Image = fileName + extension;
                    if (fileName != null)
                    {
                        using (FileStream stream = new FileStream(Path.Combine(path, fileNameFull), FileMode.Create))
                        {
                            await postModel.ImageFile.CopyToAsync(stream);
                        }
                    }


                    _context.Update(postModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(postModel.Id))
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
            return View(postModel);
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

        private bool PostExists(int id)
        {
            return _context.posts.Any(e => e.Id == id);
        }
    }
}