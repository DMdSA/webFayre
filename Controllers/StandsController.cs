using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebFayre.Common;
using WebFayre.Models;

namespace WebFayre.Controllers
{
    public class StandsController : Controller
    {
        private readonly WebFayreContext _context;

        public StandsController(WebFayreContext context)
        {
            _context = context;
        }

        private Boolean userHasSession()
        {
            return (HttpContext.Session.GetInt32("utilizadorId") != null);
        }

        private int getUserType()
        {
            return (int)HttpContext.Session.GetInt32("isFuncionario");
        }

        private int getUserId()
        {
            return (int)HttpContext.Session.GetInt32("utilizadorId");
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

        // GET: Stands
        public async Task<IActionResult> Index()
        {
            if (VerifyAdmin() == 0)
            {
                return RedirectToAction("index", "home");
            }
            else
            {
                HttpContext.Session.Remove("CartObject");
                var webFayreContext = _context.Stands.Include(s => s.Feira).Include(s => s.StandTipo);
                return View(await webFayreContext.ToListAsync());
            }
        }

        public IActionResult RedirectIndex(int idFeira)
        {
            if(getFuncFuncao() == "Admin")
            {
                return RedirectToAction("StandsByFeiraAdmin", "Stands", new { idFeira });
            }
            else
            {
                return RedirectToAction("StandsByFeira", "Stands", new { idFeira });
            }
        }

        public async Task<IActionResult> StandsByFeira(int idFeira)
        {
            if (HttpContext.Session.GetInt32("utilizadorId") == null)
                return RedirectToAction("login", "home");

            int userid = (int)HttpContext.Session.GetInt32("utilizadorId");
            HttpContext.Session.Remove("CartObject");
            var standlist = await _context.Stands.Include(s => s.Feira).Include(s => s.StandTipo).Where(s => s.FeiraId == idFeira).ToListAsync();
            var feira = await _context.Feiras.Where(s => s.IdFeira == idFeira).FirstOrDefaultAsync();
            ViewBag.FeiraNome = feira.Nome;
            /*
            if (standlist.Count == 0)
            {
                RedirectToAction("index", "feiras", userid);
                return NoContent();
            }*/

            return View(standlist);
        }

        public async Task<IActionResult> StandsByFeiraAdmin(int idFeira)
        {
            if (HttpContext.Session.GetInt32("utilizadorId") == null)
                return RedirectToAction("login", "home");
            if (VerifyAdmin() == 0)
            {
                return RedirectToAction("index", "home");
            }
            else
            {

                int userid = (int)HttpContext.Session.GetInt32("utilizadorId");
                HttpContext.Session.Remove("CartObject");
                var standlist = await _context.Stands.Include(s => s.Feira).Include(s => s.StandTipo).Where(s => s.FeiraId == idFeira).ToListAsync();
                var feira = await _context.Feiras.Where(s => s.IdFeira == idFeira).FirstOrDefaultAsync();
                ViewBag.FeiraNome = feira.Nome;
                /*
                if (standlist.Count == 0)
                {
                    RedirectToAction("index", "feiras", userid);
                    return NoContent();
                }*/

                return View(standlist);
            }
        }

        public async Task<IActionResult> Sales(int id)
        {
            if (HttpContext.Session.GetInt32("utilizadorId") == null)
            {
                return RedirectToAction("login", "home");
            }

            var users = _context.Utilizadors.Where(p => p.Id == getUserId()).ToList().Select(p => p.Email);
            var staff = _context.Standstaffs.Where(p => p.IdStand == id).ToList().Select(p => p.StaffEmail);
            foreach (var email in users)
            {
                if (staff.Contains(email))
                {

                    // get current user's id
                    var userid = (int)HttpContext.Session.GetInt32("utilizadorId");
 
                    return _context.Venda != null ?
                                View(await _context.Venda.Include(v => v.VendaProdutos).Where(v => v.StandId == id).ToListAsync()) :
                                Problem("Entity set 'WebFayreContext.Venda'  is null.");
                }
            }
            return RedirectToAction("index", "home");
        }



        // GET: Stands/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Stands == null)
            {
                return NotFound();
            }

            var stand = await _context.Stands
                .Include(s => s.Feira)
                .Include(s => s.StandTipo)
                .FirstOrDefaultAsync(m => m.IdStand == id);
            if (stand == null)
            {
                return NotFound();
            }

            return View(stand);
        }

        // GET: Stands/Create
        public IActionResult Create()
        {
            if (VerifyAdmin() == 0)
            {
                return RedirectToAction("index", "home");
            }
            else
            {
                var feiras = _context.Feiras.Where(f => f.DataFim >= DateTime.Now).ToList();
                ViewData["FeiraId"] = new SelectList(feiras, "IdFeira", "Nome");
                ViewData["StandTipoId"] = new SelectList(_context.TipoStands, "Id", "Descricao");
                return View();
            }
        }

        // POST: Stands/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdStand,Descricao,Nome,Email,Telefone,Disponibilidade,Morada,FeiraId,StandPath,StandTipoId")] Stand stand)
        {
                if (ModelState.IsValid)
                {
                    _context.Add(stand);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                ViewData["FeiraId"] = new SelectList(_context.Feiras, "IdFeira", "Nome", stand.FeiraId);
                ViewData["StandTipoId"] = new SelectList(_context.TipoStands, "Id", "Descricao", stand.StandTipoId);
                return View(stand);
        }

        // GET: Stands/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (VerifyAdmin() == 0)
            {
                return RedirectToAction("index", "home");
            }
            else
            {
                if (id == null || _context.Stands == null)
                {
                    return NotFound();
                }

                var stand = await _context.Stands.Where(s => s.IdStand == id).FirstOrDefaultAsync();
                if (stand == null)
                {
                    return NotFound();
                }
                ViewData["FeiraId"] = new SelectList(_context.Feiras, "IdFeira", "IdFeira", stand.FeiraId);
                ViewData["StandTipoId"] = new SelectList(_context.TipoStands, "Id", "Id", stand.StandTipoId);
                return View(stand);
            }
        }

        // POST: Stands/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdStand,Descricao,Nome,Email,Telefone,Disponibilidade,Morada,FeiraId,StandPath,StandTipoId")] Stand stand)
        {
                if (id != stand.IdStand)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(stand);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!StandExists(stand.IdStand))
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
                ViewData["FeiraId"] = new SelectList(_context.Feiras, "IdFeira", "IdFeira", stand.FeiraId);
                ViewData["StandTipoId"] = new SelectList(_context.TipoStands, "Id", "Id", stand.StandTipoId);
                return View(stand);
        }

        // GET: Stands/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (VerifyAdmin() == 0)
            {
                return RedirectToAction("index", "home");
            }
            else
            {
                if (id == null || _context.Stands == null)
                {
                    return NotFound();
                }

                var stand = await _context.Stands
                    .Include(s => s.Feira)
                    .Include(s => s.StandTipo)
                    .FirstOrDefaultAsync(m => m.IdStand == id);
                if (stand == null)
                {
                    return NotFound();
                }

                return View(stand);
            }
        }

        // POST: Stands/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int feiraId)
        {
                if (_context.Stands == null)
                {
                    return Problem("Entity set 'WebFayreContext.Stands'  is null.");
                }
                var stand = await _context.Stands.FindAsync(id, feiraId);

                if (stand != null)
                {
                    await RemoveStand(id, feiraId);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
        }

        public async Task RemoveStand(int idstand, int idFeira)
        {
            var stand = await _context.Stands.FindAsync(idstand, idFeira);
            ProdutosController pc = new ProdutosController(_context);
            foreach (var product in stand.Produtos)
            {
                pc.RemoveProduto(product.IdProduto);
            }
            _context.Stands.Remove(stand);
        }

        private bool StandExists(int id)
        {
          return (_context.Stands?.Any(e => e.IdStand == id)).GetValueOrDefault();
        }

        public async Task<IActionResult> Enter(int feiraId, int id)
        {

            if (StandExists(id))
            {
                if (!userHasSession())
                {
                    return RedirectToAction("Login", "Home");
                }
                if (getUserType() == 0)
                {
                    var user = await _context.Utilizadors.Where(u => u.Id == getUserId()).FirstOrDefaultAsync();
                    var email = user.Email;
                    if (email != null)
                    {
                        var staff = _context.Standstaffs.Where(u => u.StaffEmail == email).ToList().Select(s => s.IdStand);
                        if (staff.Contains(id))
                        {
                            return RedirectToAction("StaffIndex", "produtos", new { id });
                        }

                    }
                }
                //HttpContext.Session.SetObject("StandShoppingCart", ssc);

                return RedirectToAction("RedirectIndex", "produtos", new { feiraId, id }); //Redirect para um href com o id do stand
            }

            return NotFound();
        }

        public IActionResult Leave()
        {
            return RedirectToAction("RedirectIndex", "feiras");
        }
    }
}
