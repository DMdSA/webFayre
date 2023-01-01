using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebFayre.Models;
using Microsoft.EntityFrameworkCore;

namespace WebFayre.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly FairsConcurrencyController _fairsCC;


        public HomeController(ILogger<HomeController> logger, FairsConcurrencyController fairsCC)
        {
            _logger = logger;
            _fairsCC = fairsCC;
        }

        public async Task<IActionResult> IndexAsync()
        {
            WebFayreContext wfc = new WebFayreContext();

            return wfc.Feiras != null ?
                    View(await wfc.Feiras.ToListAsync()) :
                    Problem("Entity set 'WebFayreContext.Feiras'  is null.");
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

        /*
        [HttpPost]
        public async Task<JsonResult> HasSession()
        {

            if (HttpContext.Session.GetInt32("utilizadorId") == null)
            {
                return new JsonResult(new { data = 0 });
            }
            var userid = (int)HttpContext.Session.GetInt32("utilizadorId");
            return new JsonResult(new { data = userid });
        }
        */

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
            var funcionarioLoggedIn = wfc.Funcionarios
                .SingleOrDefault(u => u.Email == email && u.Password == password);

            if (userLoggedIn != null)
            {
                ViewBag.message = "Logged in!";
                ViewBag.triedOnce = "yes";

                HttpContext.Session.SetInt32("utilizadorId", userLoggedIn.Id);
                HttpContext.Session.SetString("utilizadorNome", userLoggedIn.Nome);
                HttpContext.Session.SetString("utilizadorEmail", userLoggedIn.Email);
                HttpContext.Session.SetInt32("isFuncionario", 0);

                // remove current user from all possible fairs that are being tracked
                LeaveAll(userLoggedIn.Id);


                return RedirectToAction("index", "home");
            }
            else if (funcionarioLoggedIn != null)
            {
                ViewBag.message = "Logged in!";
                ViewBag.triedOnce = "yes";

                HttpContext.Session.SetInt32("utilizadorId", funcionarioLoggedIn.IdFuncionario);
                HttpContext.Session.SetString("utilizadorNome", funcionarioLoggedIn.Nome);
                HttpContext.Session.SetString("utilizadorEmail", funcionarioLoggedIn.Email);
                HttpContext.Session.SetInt32("isFuncionario", 1);

                // remove current user from all possible fairs that are being tracked
                LeaveAll(funcionarioLoggedIn.IdFuncionario);


                return RedirectToAction("index", "home");
            }
            else
            {
                ViewBag.triedOnce = "yes";
                return View();
            }
        }


        public ActionResult Logout()
        {
            //< a asp - action = "Details" asp - route - id = "@item.IdFuncionario" > Details </ a > |
            if (HttpContext.Session.GetInt32("utilizadorId") != null)
            {

                // remove current user from all possible fairs that are being tracked
                var userid = (int)HttpContext.Session.GetInt32("utilizadorId");
                LeaveAll(userid);

                // logout
                HttpContext.Session.Clear();
                HttpContext.Session.Remove("utilizadorId");
                HttpContext.Session.Remove("utilizadorNome");
                HttpContext.Session.Remove("StandShoppingCart");
            }

            return RedirectToAction("login", "home");
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

        public void LeaveAll(int userid)
        {
            foreach (var users in _fairsCC.FairsCC.Values)
            {
                if (users.userExists(userid))
                {
                    users.removeUser(userid);
                }
            }
        }

    }
}