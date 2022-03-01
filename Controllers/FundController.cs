using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentWork.Models;
using System.Threading.Tasks;

namespace StudentWork.Controllers
{
    public class FundController : Controller
    {
        //private StudentWorkContext db = new StudentWorkContext();
        private readonly StudentWorkContext db;
        public FundController(StudentWorkContext context)
        {
            db = context;
        }
        // db context null


        public async Task<ActionResult> Index()
        {
            return View(await db.funds.ToListAsync());
        }
        public async Task<ActionResult> Details(int? id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            Fund FundModel = await db.funds.FindAsync(id);
            //if (FundModel == null)
            //{
            //    return HttpNotFound();
            //}
            return View(FundModel);
        }
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(include: "Id,Name,From,Price")] Fund fundModel) 
        {
            if (ModelState.IsValid) 
            {
                db.funds.Add(fundModel);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(fundModel);
        }

        public async Task<ActionResult> Edit(int? id) 
        {
            Fund fundModel = await db.funds.FindAsync(id);
            return View(fundModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(include: "Id,Name,From,Price")] Fund fundModel) 
        {
            if (ModelState.IsValid) 
            {
                db.Entry(fundModel).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(fundModel);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            Fund fundModel = await db.funds.FindAsync(id);
            return View(fundModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int? id) 
        {
            Fund fundModel = await db.funds.FindAsync(id);
            db.funds.Remove(fundModel);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}