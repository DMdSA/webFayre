using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebFayre.Models;
using Microsoft.EntityFrameworkCore;

namespace WebFayre.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        //private readonly FairsConcurrencyController _fairsCC;


        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            //_fairsCC = fairsCC;
        }

        public async Task<IActionResult> IndexAsync()
        {
            WebFayreContext wfc = new WebFayreContext();
            if (HttpContext.Session.GetInt32("utilizadorId") == null)
            {
                return RedirectToAction("login", "home");
            }
            var userid = (int)HttpContext.Session.GetInt32("utilizadorId");

            if (HttpContext.Session.GetInt32("isFuncionario") == 0)
            {
                var user = await wfc.Utilizadors.Include(u => u.IdFeiras).FirstOrDefaultAsync(m => m.Id == userid);
                
                if (user != null)
                    ViewData["favorite"] = user.IdFeiras;
                else ViewData["favorite"] = new List<Feira>();
                ViewBag.message = "";
            }

            else
            {
                ViewData["favorite"] = new List<Feira>();
                ViewBag.message = "";
            }

            return wfc.Feiras != null ?
                View(await wfc.Feiras.Where(f => f.DataFim >= DateTime.Today).OrderBy(f => f.DataInicio).ToListAsync()) :
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

        // POST: Feiras/SearchResults
        [HttpPost]
        public async Task<IActionResult> SearchResult(String nameFeira)
        {
            WebFayreContext wfc = new WebFayreContext();
            if (HttpContext.Session.GetInt32("isFuncionario") == 0)
            {
                var userid = (int)HttpContext.Session.GetInt32("utilizadorId");
                var a = new List<Feira>();
                var user = await wfc.Utilizadors.Include(u => u.IdFeiras).FirstOrDefaultAsync(m => m.Id == userid);
                ViewData["favorite"] = a.Concat(user.IdFeiras);
                ViewBag.message = "";
            }
            var result = await wfc.Feiras.Where(f => f.DataFim >= DateTime.Today).OrderBy(f => f.DataInicio)
                .Where(f => f.Nome.Contains(nameFeira) || f.FeiraCategoria1s.ToList().Select(e => e.Descricao).Contains(nameFeira))
                .ToListAsync();
            return View("Index", result);
        }

        [HttpPost]
        public async Task<IActionResult> SearchDate(DateTime dateInicio)
        {
            WebFayreContext wfc = new WebFayreContext();
            if (HttpContext.Session.GetInt32("isFuncionario") == 0)
            {
                var userid = (int)HttpContext.Session.GetInt32("utilizadorId");
                var a = new List<Feira>();
                var user = await wfc.Utilizadors.Include(u => u.IdFeiras).FirstOrDefaultAsync(m => m.Id == userid);
                ViewData["favorite"] = a.Concat(user.IdFeiras);
                ViewBag.message = "";
            }
            var result = await wfc.Feiras.Where(f => f.DataInicio >= dateInicio).OrderBy(f => f.DataInicio).ToListAsync();
            return View("Index", result);
        }


        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Utilizador u)
        {
            WebFayreContext wfc = new WebFayreContext();
            var userlist = wfc.Utilizadors.ToList().Select(e => e.Email);

            if (userlist.Contains(u.Email))
            {
                return RedirectToAction("Advise", "home");
                //return RedirectToAction("index", "home");
            }

            var funcList = wfc.Funcionarios.ToList().Select(e => e.Email);
            if (funcList.Contains(u.Email))
            {
                return RedirectToAction("Advise", "home");
                //return RedirectToAction("index", "home");
            }
            wfc.Utilizadors.Add(u);
            wfc.SaveChanges();

            ViewBag.message = "You are successfully registered!!";

            return RedirectToAction("login", "home");
        }

        public async Task<IActionResult> Advise()
        {
            ViewBag.Message = "There is someone with that email";
            return View();
        }
        
        public ActionResult Login()
        {   
            if (HttpContext.Session.GetInt32("utilizadorId") != null)
            {
                return RedirectToAction("index", "home");
            }
            else
            {
                ViewBag.message = "empty";
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
                HttpContext.Session.SetString("utilizadorEmail", userLoggedIn.Email);
                HttpContext.Session.SetInt32("isFuncionario", 0);


                return RedirectToAction("index", "home");
            }
            var funcionarioLoggedIn = wfc.Funcionarios
                .SingleOrDefault(u => u.Email == email && u.Password == password);
            if (funcionarioLoggedIn != null)
            {
                ViewBag.message = "Logged in!";
                ViewBag.triedOnce = "yes";

                HttpContext.Session.SetInt32("utilizadorId", funcionarioLoggedIn.IdFuncionario);
                HttpContext.Session.SetString("utilizadorNome", funcionarioLoggedIn.Nome);
                HttpContext.Session.SetString("utilizadorEmail", funcionarioLoggedIn.Email);
                HttpContext.Session.SetInt32("isFuncionario", 1);
                var func = funcionarioLoggedIn.Funcao;
                var funcName = wfc.Funcaos.Where(u => u.IdFuncao == func).FirstOrDefault();
                HttpContext.Session.SetString("Funcao", funcName.Descricao);


                return RedirectToAction("index", "home");
            }
            else
            {
                ViewBag.message = "false";
                ViewBag.triedOnce = "yes";
                return View();
            }
        }

        public IActionResult RedirectProfile()
        {
            if (HttpContext.Session.GetInt32("utilizadorId") == null)
            {
                return RedirectToAction("login", "home");
            }
            else if(HttpContext.Session.GetInt32("isFuncionario") == 0)
            {
                return RedirectToAction("ViewProfile", "utilizadores");
            }
            else
            {
                return RedirectToAction("ViewProfile", "funcionarios");
            }
        }


        public ActionResult Logout()
        {
            //< a asp - action = "Details" asp - route - id = "@item.IdFuncionario" > Details </ a > |
            if (HttpContext.Session.GetInt32("utilizadorId") != null)
            {

                // remove current user from all possible fairs that are being tracked
                var userid = (int)HttpContext.Session.GetInt32("utilizadorId");

                // logout
                HttpContext.Session.Clear();
                HttpContext.Session.Remove("utilizadorId");
                HttpContext.Session.Remove("utilizadorNome");
                HttpContext.Session.Remove("StandShoppingCart");
                HttpContext.Session.Remove("isFuncionario");
                HttpContext.Session.Remove("Funcao");
                
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

        

    }
}