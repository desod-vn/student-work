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

        public async Task<IActionResult> Index()
        {
            ViewBag.Funds = await _context.funds.ToListAsync();

            return View();
        }

        public IActionResult Privacy()
        {
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

                return RedirectToAction("Index", "User");
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
