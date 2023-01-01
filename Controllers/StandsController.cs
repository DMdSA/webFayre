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

        // GET: Stands
        public async Task<IActionResult> Index()
        {
            var webFayreContext = _context.Stands.Include(s => s.Feira).Include(s => s.StandTipo);
            return View(await webFayreContext.ToListAsync());
        }

        public async Task<IActionResult> StandsByFeira(int feiraid)
        {
            var standlist = _context.Stands.Include(s => s.Feira).Include(s => s.StandTipo).Where(s => s.FeiraId == feiraid);
            var feira = await _context.Feiras.Where(s => s.IdFeira == feiraid).FirstOrDefaultAsync();
            ViewBag.NomeFeira = feira.Nome;
            return View(await standlist.ToListAsync());
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
            ViewData["FeiraId"] = new SelectList(_context.Feiras, "IdFeira", "Nome");
            ViewData["StandTipoId"] = new SelectList(_context.TipoStands, "Id", "Descricao");
            return View();
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
            if (id == null || _context.Stands == null)
            {
                return NotFound();
            }

            var stand = await _context.Stands.FindAsync(id);
            if (stand == null)
            {
                return NotFound();
            }
            ViewData["FeiraId"] = new SelectList(_context.Feiras, "IdFeira", "IdFeira", stand.FeiraId);
            ViewData["StandTipoId"] = new SelectList(_context.TipoStands, "Id", "Id", stand.StandTipoId);
            return View(stand);
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
                _context.Stands.Remove(stand);
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

                

                //HttpContext.Session.SetObject("StandShoppingCart", ssc);

                return RedirectToAction("produtosByStand", "produtos", new { feiraId, id }); //Redirect para um href com o id do stand
            }

            return NotFound();
        }

        public IActionResult Leave()
        {
            return RedirectToAction("index", "feiras");
        }
    }
}
