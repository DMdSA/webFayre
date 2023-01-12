using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using WebFayre.Common;
using WebFayre.Models;

namespace WebFayre.Controllers
{
    public class FeirasController : Controller
    {
        private readonly WebFayreContext _context;
        private readonly FairsConcurrencyController _fairsCC;

        public FeirasController(WebFayreContext context, FairsConcurrencyController fairsCC)
        {
            _context = context;
            _fairsCC = fairsCC;

            // <feira_id, {<id_utilizador,email_utilizador>, maxConcurrentUsers}>
            // {1 : { {1: d@gmail.com, 2: j@gmail.com} , 10 }, 2 : { {13 : @gmail.com, 144 : @gmail.com} , 100}, ...}
        }

        private Boolean userHasSession()
        {
            return (HttpContext.Session.GetInt32("utilizadorId") != null);
        }

        private int getUserId()
        {
            return (int)HttpContext.Session.GetInt32("utilizadorId");
        }
        private int getUserType()
        {
            return (int)HttpContext.Session.GetInt32("isFuncionario");
        }

        private string getUserEmail()
        {
            return HttpContext.Session.GetString("utilizadorEmail");
        }

        private string getFuncFuncao()
        {
            return HttpContext.Session.GetString("Funcao");
        }

        private int VerifyAdmin()
        {
            if (HttpContext.Session.GetInt32("utilizadorId") == null || getFuncFuncao() != "Admin")
                return 0;
            else
                return 1;
        }

        public IActionResult RedirectIndex(string nameFeira)
        {
            if (!userHasSession())
            {
                return RedirectToAction("Login", "Home");
            }

            if (getFuncFuncao() == "Admin")
            {
                return RedirectToAction("Index", "Feiras", nameFeira);
            }
            else if(getUserType() == 0)
            {
                return RedirectToAction("IndexUser", "Feiras", nameFeira);
            }
            else
            {
                return RedirectToAction("IndexFunc", "Feiras", nameFeira);
            }
        }

        // GET: Feiras
        public async Task<IActionResult> Index(string nameFeira)
        {
            if (VerifyAdmin() == 0)
            {
                return RedirectToAction("index", "home");
            }
            else
            {
                if (String.IsNullOrEmpty(nameFeira))
                {
                    return _context.Feiras != null ?
                            View(await _context.Feiras.Include(f => f.FeiraCategoria1s).ToListAsync()) :
                            Problem("Entity set 'WebFayreContext.Feiras'  is null.");
                }
                else
                {
                    var searchItems = await _context.Feiras.Include(f => f.FeiraCategoria1s).Where(s => s.Nome.Contains(nameFeira)).ToListAsync();
                    return View(searchItems);
                }
            }
        }

        public async Task<IActionResult> IndexUser(string nameFeira)
        {
            if (getUserType() != 0)
            {
                return RedirectToAction("index", "home");
            }
            else
            {
                if (String.IsNullOrEmpty(nameFeira))
                {
                    return _context.Feiras != null ?
                            View(await _context.Feiras.Include(f => f.FeiraCategoria1s).Where(f => f.DataFim >= DateTime.Today).ToListAsync()) :
                            Problem("Entity set 'WebFayreContext.Feiras'  is null.");
                }
                else
                {
                    var searchItems = await _context.Feiras.Include(f => f.FeiraCategoria1s).Where(f => f.DataFim >= DateTime.Today).Where(s => s.Nome.Contains(nameFeira)).ToListAsync();
                    return View(searchItems);
                }
            }
        }

        public async Task<IActionResult> IndexFunc(string nameFeira)
        {
            if (getUserType() != 1)
            {
                return RedirectToAction("index", "home");
            }
            else
            {
                if (String.IsNullOrEmpty(nameFeira))
                {
                    return _context.Feiras != null ?
                            View(await _context.Feiras.Include(f => f.FeiraCategoria1s).Where(f => f.DataFim >= DateTime.Today).ToListAsync()) :
                            Problem("Entity set 'WebFayreContext.Feiras'  is null.");
                }
                else
                {
                    var searchItems = await _context.Feiras.Include(f => f.FeiraCategoria1s).Where(f => f.DataFim >= DateTime.Today).Where(s => s.Nome.Contains(nameFeira)).ToListAsync();
                    return View(searchItems);
                }
            }
        }

        // POST: Feiras/SearchResults
        public async Task<IActionResult> SearchResults(String searchTerm)
        {
            if (!userHasSession())
            {
                return RedirectToAction("Login", "Home");
            }
            if (getFuncFuncao() == "Admin")
            {
                return View("Index", await _context.Feiras.Include(f => f.FeiraCategoria1s).Where(f => f.Nome.Contains(searchTerm)).ToListAsync());
            }
            else if(getUserType() == 0)
            {
                return View("IndexUser", await _context.Feiras.Include(f => f.FeiraCategoria1s).Where(f => f.Nome.Contains(searchTerm)).ToListAsync());
            }
            else
            {
                return View("IndexFunc", await _context.Feiras.Include(f => f.FeiraCategoria1s).Where(f => f.Nome.Contains(searchTerm)).ToListAsync());
            }
        }

        // GET: Feiras/Details/5
        public async Task<IActionResult> Details(int? id)
        {
                if (id == null || _context.Feiras == null)
                {
                    return NotFound();
                }

                //var feira = await _context.Feiras.Include(x => x.FeiraCategoria1s)
                //    .ThenInclude(y => y.IdCategoriaFeira)  //Devia ser o nome da table
                //    .SingleOrDefaultAsync(m => m.IdFeira == id);

                var feira = await _context.Feiras
                    .SingleOrDefaultAsync(m => m.IdFeira == id);
                if (feira == null)
                {
                    return NotFound();
                }
                return View(feira);
        }

        // GET: Feiras/Create
        public IActionResult Create()
        {
            if (VerifyAdmin() == 0)
            {
                return RedirectToAction("index", "home");
            }
            else
            {

                ViewData["Categorias"] = new MultiSelectList(_context.Categoriafeiras, "IdCategoriaFeira", "Descricao");
                ViewData["Patrocinadores"] = new MultiSelectList(_context.Patrocinadors, "IdPatrocinador", "Nome");
                return View();
            }
        }

        // POST: Feiras/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdFeira,Descricao,Nome,DataInicio,DataFim,CapacidadeClientes,NStands,Email,Telefone,Morada,FeiraPath,FeiraCategoria1s,Patrocinadors")] Feira feira)
        {
                await _context.Feiras.Include(x => x.FeiraCategoria1s).Include(x => x.Patrocinadors).LoadAsync();

                if (ModelState.IsValid)
                {
                    var category_values = ModelState.Values.ToList()[9];
                    var category_ids = category_values.AttemptedValue.Split(",");
                    var category_idsList = category_ids.Select(int.Parse).ToList();

                    var patroc_values = ModelState.Values.ToList()[10];
                    var patroc_ids = patroc_values.AttemptedValue.Split(",");
                    var patroc_idsList = patroc_ids.Select(int.Parse).ToList();

                    var category_entities = _context.Categoriafeiras.Where(x => category_idsList.Contains(x.IdCategoriaFeira));
                    var patroc_entities = _context.Patrocinadors.Where(x => patroc_idsList.Contains(x.IdPatrocinador));
                    feira.FeiraCategoria1s.AddRange(category_entities);
                    feira.Patrocinadors.AddRange(patroc_entities);

                    _context.Add(feira).Collection(c => c.FeiraCategoria1s);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }


                ViewData["Categorias"] = new MultiSelectList(_context.Categoriafeiras, "IdCategoriaFeira", "Descricao", feira.FeiraCategoria1s);
                ViewData["Patrocinadores"] = new MultiSelectList(_context.Patrocinadors, "IdPatrocinador", "Nome", feira.Patrocinadors);


                return View(feira);
        }

        // GET: Feiras/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (VerifyAdmin() == 0)
            {
                return RedirectToAction("index", "home");
            }
            else
            {
                if (id == null || _context.Feiras == null)
                {
                    return NotFound();
                }

                var feira = await _context.Feiras.FindAsync(id);
                if (feira == null)
                {
                    return NotFound();
                }
                ViewData["Categorias"] = new MultiSelectList(_context.Categoriafeiras, "IdCategoriaFeira", "Descricao", feira.FeiraCategoria1s);
                ViewData["Patrocinadores"] = new MultiSelectList(_context.Patrocinadors, "IdPatrocinador", "Nome", feira.Patrocinadors);
                return View(feira);
            }
        }

        // POST: Feiras/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdFeira,Descricao,Nome,DataInicio,DataFim,CapacidadeClientes,NStands,Email,Telefone,Morada,FeiraPath,FeiraCategoria1s,Patrocinadors")] Feira feira)
        {
                if (id != feira.IdFeira)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        var category_values = ModelState.Values.ToList()[12];
                        var category_ids = category_values.AttemptedValue.Split(",");
                        var category_idsList = category_ids.Select(int.Parse).ToList();

                        var patroc_values = ModelState.Values.ToList()[11];
                        var patroc_ids = patroc_values.AttemptedValue.Split(",");
                        var patroc_idsList = patroc_ids.Select(int.Parse).ToList();

                        foreach (var category in feira.FeiraCategoria1s.ToList())
                        {
                            feira.FeiraCategoria1s.Remove(category);
                        }
                        foreach (var patrocinador in feira.Patrocinadors.ToList())
                        {
                            feira.Patrocinadors.Remove(patrocinador);
                        }

                        var category_entities = _context.Categoriafeiras.Where(x => category_idsList.Contains(x.IdCategoriaFeira));
                        var patroc_entities = _context.Patrocinadors.Where(x => patroc_idsList.Contains(x.IdPatrocinador));
                        feira.FeiraCategoria1s.AddRange(category_entities);
                        feira.Patrocinadors.AddRange(patroc_entities);
                        _context.Update(feira);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!FeiraExists(feira.IdFeira))
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
                ViewData["FeiraCategoria1s"] = new MultiSelectList(_context.Categoriafeiras, "IdCategoriaFeira", "IdCategoriaFeira", feira.FeiraCategoria1s);
                ViewData["Patrocinadores"] = new MultiSelectList(_context.Patrocinadors, "IdPatrocinador", "Nome", feira.Patrocinadors);
                return View(feira);
        }

        // GET: Feiras/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (VerifyAdmin() == 0)
            {
                return RedirectToAction("index", "home");
            }
            else
            {
                if (id == null || _context.Feiras == null)
                {
                    return NotFound();
                }

                var feira = await _context.Feiras.Include(x => x.Stands).Include(x => x.Tickets).Include(x => x.FeiraCategoria1s)
                    .FirstOrDefaultAsync(m => m.IdFeira == id);
                if (feira == null)
                {
                    return NotFound();
                }

                return View(feira);
            }
        }

        // POST: Feiras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
                if (_context.Feiras == null)
                {
                    return Problem("Entity set 'WebFayreContext.Feiras'  is null.");
                }
                var feira = await _context.Feiras.Include(f => f.FeiraCategoria1s).FirstOrDefaultAsync(f => f.IdFeira == id);
                if (feira != null)
                {
                    CategoriafeirasController categoriaController = new CategoriafeirasController(_context);
                    foreach (var category in feira.FeiraCategoria1s.ToList())
                    {
                        feira.FeiraCategoria1s.Remove(category);
                        await categoriaController.RemoveFeiraAsync(feira, category.IdCategoriaFeira);
                    }

                    PatrocinadoresController patrocinadoresController = new PatrocinadoresController(_context);
                    foreach (var patrocinador in feira.Patrocinadors.ToList())
                    {
                        feira.Patrocinadors.Remove(patrocinador);
                        await patrocinadoresController.RemoveFeiraAsync(feira, patrocinador.IdPatrocinador);
                    }

                    StandsController standsController = new StandsController(_context);
                    foreach (var stand in feira.Stands.ToList())
                    {
                        await standsController.RemoveStand(stand.IdStand, stand.FeiraId);
                    }
                    TicketsController tc = new TicketsController(_context);
                    foreach (var ticket in feira.Tickets.ToList())
                    {
                        await tc.RemoveTicket(ticket.Id);
                    }
                    _context.Feiras.Remove(feira);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
        }

        private bool FeiraExists(int id)
        {
            return (_context.Feiras?.Any(e => e.IdFeira == id)).GetValueOrDefault();
        }

        public async Task<IActionResult> Enter(int id)
        {
            // se não houver nenhuma session ativa, redirecionar para o login
            if (!userHasSession())
            {
                return RedirectToAction("Login", "Home");
            }

            // get current user id
            var userid = getUserId();
            var useremail = getUserEmail();
            var isFunc = getUserType();
            // se a feira existe na bd
            if (FeiraExists(id))
            {
                if(isFunc == 1)
                {
                    return RedirectToAction("RedirectIndex", "stands", new { idFeira = id });
                }
                // verificar se um ticket já foi emitido para o utilizador em questão
                bool ticket_exists = _context.Tickets.Any(t => t.UtilizadorId == userid && t.FeiraId == id);

                // se o ticket não tiver sido gerado
                if (!ticket_exists)
                {
                    // criar um ticket manualmente com valores default
                    Ticket t = new Ticket() { Id = 0, Data = DateTime.Now, UtilizadorId = (int)userid, FeiraId = id };
                    
                    // utilizar o controller dos tickets para o adicionar corretamente à db
                    TicketsController tc = new TicketsController(_context);
                    await tc.Create(t);
                    
                    TempData["feiraticket"] = "Ticket gerado com sucesso!";
                    //return RedirectToAction("standsByFeira", "stands", new { id });
                    //return await addUserToFair(id, userid, useremail);
                }
                // se o ticket já tinha sido gerado

                TempData["feiraticket"] = "Já tinhas um ticket! podes entrar! :)";
                return RedirectToAction("RedirectIndex", "stands", new { idFeira = id });
                
                // entrar nos stands associados à feira em questão
                //return await addUserToFair(id, userid, useremail);
                //return RedirectToAction("standsByFeira", "stands", new { id });
            }

            return NotFound();
        }

        
        public async Task<IActionResult> Leave(int id)
        {
            if (this._fairsCC.FairsCC.ContainsKey(id))
            {
                var userid = getUserId();
                int result = (this._fairsCC.FairsCC[id]).removeUser(userid);
                if (this._fairsCC.FairsCC[id].onlineUsers() == 0)
                {
                    this._fairsCC.FairsCC.TryRemove(id, out _);
                }
                return RedirectToAction("index", "feiras");
            }
            return RedirectToAction("index", "home");
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


        public async Task<IActionResult> addUserToFair(int feiraid, int userid, string email)
        {

            // se a feira já estiver a ser controlada
            if (this._fairsCC.FairsCC.ContainsKey(feiraid))
            {
                // get do controlador da feira em questão
                FairCurrentUsers fcu = this._fairsCC.FairsCC[feiraid];

                // tentar entrar na feira
                int result = fcu.addUser(userid, email);

                // se foi possível entrar na feira
                if (result == 0)
                    
                    // entrar nos stands associados à feira em questão
                    return RedirectToAction("RedirectIndex", "stands", new { idFeira = feiraid });

                // se não foi possível entrar na feira
                else
                {
                    TempData["enter"] = "full";
                    // alterar o redirect para onde for necessário
                    return RedirectToAction("index", "home");
                }
            }

            // se a feira ainda não estiver a ser controlada
            // get da feira
            var feira = await _context.Feiras.FindAsync(feiraid);
                
            // criar o controlador de utilizadores em simultaneo
            FairCurrentUsers nfcu = new FairCurrentUsers(feira.CapacidadeClientes);
            // adicionar utilizador
            nfcu.addUser(userid, email);
            // atualizar o controlador de feiras
            this._fairsCC.FairsCC.TryAdd(feiraid, nfcu);

            // entrar na feira
            TempData["userid"] = userid;
            TempData["feiraid"] = feiraid;
            return RedirectToAction("RedirectIndex", "stands", new { idFeira = feiraid });
        }

        /*
        public async Task<int> removeUserFromFair(int feiraid, int userid)
        {
            // se a feira estiver a ser controlada
            if (this.fairsCurrentUsers.ContainsKey(feiraid))
            {
                // remover o utilizador, caso exista
                return (this.fairsCurrentUsers[feiraid]).removeUser(userid);
            }

            // se a feira não estiver a ser controlada
            return 3;
        }
        */
        public async Task<IActionResult> AddFavorite(int id)
        {

            // se não houver nenhuma session ativa, redirecionar para o login
            if (!userHasSession())
            {
                return RedirectToAction("Login", "Home");
            }

            // get current user id
            var userid = getUserId();

            await _context.Feiras.LoadAsync();
            await _context.Utilizadors.Include(u => u.IdFeiras).LoadAsync();

            var currentuser = await _context.Utilizadors.FindAsync(userid);
            var feira = await _context.Feiras.FindAsync(id);
            if (!currentuser.IdFeiras.Contains(feira))
            {

                currentuser.IdFeiras.Add(feira);
                _context.Update(currentuser).Collection(u => u.IdFeiras);
                await _context.SaveChangesAsync();
                ViewBag.favorite = "NewFavorite";
            }
            else
            {
                ViewBag.favorite = "OldFavorite";
                currentuser.IdFeiras.Remove(feira);
                _context.Update(currentuser).Collection(u => u.IdFeiras);
                await _context.SaveChangesAsync();
            }

            // change if needed
            return RedirectToAction("RedirectIndex", "feiras");
        }

        public async Task<IActionResult> Favorites()
        {
            if (!userHasSession())
            {
                return RedirectToAction("Login", "Home");
            }

            // get current user id
            var userid = getUserId();
            var currentuser = await _context.Utilizadors.FindAsync(userid);
            var feiraids = currentuser.IdFeiras.Select(f => f.IdFeira).ToList();

            var feiras = await _context.Feiras.Where(f => feiraids.Contains(f.IdFeira)).Include(f => f.FeiraCategoria1s).ToListAsync();
            return _context.Feiras != null ?
                        View(feiras) :
                        Problem("Entity set 'WebFayreContext.Feiras'  is null.");
        }

        public IActionResult RedirectToForm()
        {
            if (!userHasSession())
            {
                return RedirectToAction("Login", "Home");
            }
            else if(getUserType() == 1){
                return RedirectToAction("IndexFuncionario", "PromocaoFeiras");
            }
            else
            {
                return RedirectToAction("IndexByUser", "PromocaoFeiras");
            }

        }

    }
}
