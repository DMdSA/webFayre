using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebFayre.Models;
using Microsoft.AspNetCore.Http;

namespace WebFayre.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Utilizador u)
        {
            WebFayreContext wfc = new WebFayreContext();
            wfc.Utilizadors.Add(u);
            wfc.SaveChanges();

            ViewBag.message = "You are successfully registered!!";

            return RedirectToAction("login", "home");
        }
        
        public ActionResult Login()
        {   
            if (HttpContext.Session.GetInt32("utilizadorId") != null)
            {
                return RedirectToAction("index", "home");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            WebFayreContext wfc = new WebFayreContext();
            var userLoggedIn = wfc.Utilizadors
                .SingleOrDefault(u => u.Email == email && u.Password == password);

            if (userLoggedIn != null)
            {
                ViewBag.message = "Logged in!";
                ViewBag.triedOnce = "yes";

                HttpContext.Session.SetInt32("utilizadorId", userLoggedIn.Id);
                HttpContext.Session.SetString("utilizadorNome", userLoggedIn.Nome);


                return RedirectToAction("index", "home");
            }
            else
            {
                ViewBag.triedOnce = "yes";
                return View();
            }
        }


        public ActionResult mainPage(string name)
        {

            if (HttpContext.Session.GetInt32("utilizadorId") == null)
            {
                return RedirectToAction("login", "home");
            }
            else
            {
                //ViewBag.name = HttpContext.Session.GetString("utilizadorNome");
                return View();
            }
        }

    }
}