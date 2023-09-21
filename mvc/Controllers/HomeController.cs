using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using mvc.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly MasterContext _dbContext;
        public HomeController(ILogger<HomeController> logger, IHttpContextAccessor httpContextAccessor,MasterContext dbContext)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var algo = _httpContextAccessor.HttpContext.Request;
            List<User> users = _dbContext.Users.FromSqlRaw("exec GetUsers").ToList();
            var usernameParam = new SqlParameter("@Username","Juan");
            var user = _dbContext.Users.FromSqlRaw("exec GetUser 'Juan'").ToList().FirstOrDefault();
            return View(users);
        }
        [Authorize]
        public IActionResult Privacy()
        {
            var username = User.Identity.Name;

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Login(string username,string password)
        {
            var ctx = _httpContextAccessor.HttpContext;
            if (username == "develop" && password == "develop") 
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username),
                };
                var claimsIdentity = new ClaimsIdentity(claims,"Login");
                await ctx.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                return Redirect("/Home/Privacy");
            }
            return RedirectToAction("Login") ;

        }

        public IActionResult Login()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}