using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StudentWork.Models;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using System.Linq;
using System;

namespace StudentWork.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly StudentWorkContext _context;

        public HomeController(ILogger<HomeController> logger, StudentWorkContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index(int? pageNumber)
        {
            int pageSize = 10;

            ViewBag.Funds = await _context.funds.ToListAsync();

            ViewBag.HotPosts = await _context.settings
                .Where(s => s.Key.Contains("ST_HotPosts"))
                .ToDictionaryAsync(s => s.Key, s => s.Value);

            ViewBag.HotImage = _context.settings
                .Where(s => s.Key == "ST_ImageLink_Image")
                .FirstOrDefault();
            
            ViewBag.HotImageLink = _context.settings
                .Where(s => s.Key == "ST_ImageLink_Link")
                .FirstOrDefault();
            
            var posts = _context.posts
                .Include(p => p.Category)
                .Include(p => p.User);

            var hotCategories = await _context.settings
                .Where(s => s.Key == "ST_HotCategories")
                .Select(s => int.Parse(s.Value))
                .ToListAsync();
            
            var hotWorks = await _context.settings
                .Where(s => s.Key == "ST_HotWorks")
                .Select(s => int.Parse(s.Value))
                .ToListAsync();
            
            string[] badges = {"primary", "secondary", "success", "danger", "info", "dark"};

            ViewBag.HotCategories = await _context.categories
                .Where(c => hotCategories.Contains(c.Id))
                .ToListAsync();

            ViewBag.HotWorks = await _context.categories
                .Where(c => hotWorks.Contains(c.Id))
                .Include(c => c.Posts)
                .ToListAsync();

             ViewBag.HotWorkPosts = _context.posts
                .Where(p => hotWorks.Contains(p.CategoryId))
                .Include(p => p.Category)
                .Take(5);
            
            ViewBag.Bagdes = badges;
            
            return View(await PaginatedList<Post>.CreateAsync(posts.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        public async Task<IActionResult> Dashboard()
        {
            ViewBag.TotalUsers = await _context.users.CountAsync();

            ViewBag.TotalPosts = await _context.posts.CountAsync();

            ViewBag.TotalCategories = await _context.categories.CountAsync();

            ViewBag.TotalFunds = await _context.funds.CountAsync();

            ViewBag.TotalMoney = await _context.funds.SumAsync(f => f.Price);

            ViewBag.TopFunds = _context.funds.OrderByDescending(f => f.Price).Take(5);

            ViewBag.TopCategories = _context.categories
                .Include(c => c.Posts)
                .OrderByDescending(c => c.Posts.Count)
                .Take(5);

            ViewBag.TopUsers = _context.users
                .Include(u => u.Posts)
                .OrderByDescending(c => c.Posts.Count)
                .Take(5);

            return View();
        }

        public IActionResult Login()
        {
            string UserExist = HttpContext.Session.GetString("Name");
            
            if(UserExist != null)
                return RedirectToAction("Index");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login([Bind(include: "Email,Password")] User userModel) 
        {
            string HashPassword = MD5Hash(userModel.Password);

            var data = await _context.users.Where(s => 
                s.Email.Equals(userModel.Email) && s.Password.Equals(HashPassword)).ToListAsync();

            if (data.Count() > 0)
            {
                HttpContext.Session.SetInt32("UserIdLogin", data.FirstOrDefault().Id);
                HttpContext.Session.SetString("Name", data.FirstOrDefault().Name);
                HttpContext.Session.SetString("Email", data.FirstOrDefault().Email);

                return RedirectToAction("Dashboard");
            }
            else
            {
                ViewBag.Error = "Thông tin tài khoản hoặc mật khẩu không chính xác.";
                return View();
            }
        }

        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
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
