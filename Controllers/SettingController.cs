using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StudentWork.Models;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace StudentWork.Controllers
{
    public class SettingController : Controller
    {
        private readonly ILogger<SettingController> _logger;
        private readonly StudentWorkContext _context;

        public SettingController(ILogger<SettingController> logger, StudentWorkContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Categories = await _context.categories
                .ToListAsync();
            
            ViewBag.HotCategories = await _context.settings
                .Where(s => s.Key == "ST_HotCategories")
                .Select(s => s.Value)
                .ToListAsync();

            ViewBag.HotWorks = await _context.settings
                .Where(s => s.Key == "ST_HotWorks")
                .Select(s => s.Value)
                .ToListAsync();

            ViewBag.HotImage = _context.settings
                .Where(s => s.Key == "ST_ImageLink_Image")
                .FirstOrDefault();
            
            ViewBag.HotImageLink = _context.settings
                .Where(s => s.Key == "ST_ImageLink_Link")
                .FirstOrDefault();

            ViewBag.HotPosts = await _context.settings
                .Where(s => s.Key.Contains("ST_HotPosts"))
                .ToDictionaryAsync(s => s.Key, s => s.Value);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult HotCategories(int[] ids)
        {
            string key = "ST_HotCategories";
            var keyExist = _context.settings.Where(s => s.Key == key);
            _context.settings.RemoveRange(keyExist);
            foreach(int id in ids) {
                _context.settings.AddRange(
                    new Setting
                    {
                        Key = key,
                        Value = id.ToString(),
                    }
                );
            }
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult HotImage(string image, string link)
        {
            string key = "ST_ImageLink";
            var keyExist = _context.settings.Where(s => s.Key.Contains(key));
            _context.settings.RemoveRange(keyExist);
            foreach(int id in link) {
                _context.settings.AddRange(
                    new Setting
                    {
                        Key = key + "_Image",
                        Value = image,
                    },
                    new Setting
                    {
                        Key = key + "_Link",
                        Value = link,
                    }
                );
            }
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult HotPosts(string[] titles, string[] descriptions, string[] links)
        {
            string key = "ST_HotPosts";
            var keyExist = _context.settings.Where(s => s.Key.Contains(key));
            _context.settings.RemoveRange(keyExist);
            for(int i = 0; i < 3; i++) {
                _context.settings.AddRange(
                    new Setting
                    {
                        Key = key + "_Title_" + i.ToString(),
                        Value = titles[i],
                    },
                    new Setting
                    {
                        Key = key + "_Description_" + i.ToString(),
                        Value = descriptions[i],
                    },
                    new Setting
                    {
                        Key = key + "_Link_" + i.ToString(),
                        Value = links[i],
                    }
                );
            }
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult HotWorks(int[] ids)
        {
            string key = "ST_HotWorks";
            var keyExist = _context.settings.Where(s => s.Key == key);
            _context.settings.RemoveRange(keyExist);
            foreach(int id in ids) {
                _context.settings.AddRange(
                    new Setting
                    {
                        Key = key,
                        Value = id.ToString(),
                    }
                );
            }
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
