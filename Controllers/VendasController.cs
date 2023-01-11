using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebFayre.Models;

namespace WebFayre.Controllers
{
    public class VendasController : Controller
    {
        private readonly WebFayreContext _context;

        public VendasController(WebFayreContext context)
        {
            _context = context;
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

        // GET: Vendas
        public async Task<IActionResult> Index()
        {
            if (VerifyAdmin() == 0)
            {
                return RedirectToAction("index", "home");
            }
            else
            {
                var webFayreContext = _context.Venda.Include(v => v.Stand).Include(v => v.Utilizador);
                return View(await webFayreContext.ToListAsync());
            }
        }

        // GET: Vendas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Venda == null)
            {
                return NotFound();
            }

            var vendum = await _context.Venda
                .Include(v => v.Stand)
                .Include(v => v.Utilizador)
                .FirstOrDefaultAsync(m => m.IdVenda == id);
            if (vendum == null)
            {
                return NotFound();
            }

            return View(vendum);
        }

        // GET: Vendas/Create
        public IActionResult Create()
        {
            if (VerifyAdmin() == 0)
            {
                return RedirectToAction("index", "home");
            }
            else
            {
                ViewData["StandId"] = new SelectList(_context.Stands, "IdStand", "IdStand");
                ViewData["UtilizadorId"] = new SelectList(_context.Utilizadors, "Id", "Id");
                return View();
            }
        }

        // POST: Vendas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdVenda,Data,Total,ValorRegateio,UtilizadorId,StandId")] Vendum vendum)
        {
            await _context.Venda.Include(v => v.Stand).Include(v => v.Utilizador).LoadAsync();
            if (ModelState.IsValid)
            {
                _context.Add(vendum);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StandId"] = new SelectList(_context.Stands, "IdStand", "IdStand", vendum.StandId);
            ViewData["UtilizadorId"] = new SelectList(_context.Utilizadors, "Id", "Id", vendum.UtilizadorId);
            return View(vendum);
        }

        // GET: Vendas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (VerifyAdmin() == 0)
            {
                return RedirectToAction("index", "home");
            }
            else
            {
                if (id == null || _context.Venda == null)
                {
                    return NotFound();
                }

                var vendum = await _context.Venda.FindAsync(id);
                if (vendum == null)
                {
                    return NotFound();
                }
                ViewData["StandId"] = new SelectList(_context.Stands, "IdStand", "IdStand", vendum.StandId);
                ViewData["UtilizadorId"] = new SelectList(_context.Utilizadors, "Id", "Id", vendum.UtilizadorId);
                return View(vendum);
            }
        }

        // POST: Vendas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdVenda,Data,Total,ValorRegateio,UtilizadorId,StandId")] Vendum vendum)
        {
            if (id != vendum.IdVenda)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vendum);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VendumExists(vendum.IdVenda))
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
            ViewData["StandId"] = new SelectList(_context.Stands, "IdStand", "IdStand", vendum.StandId);
            ViewData["UtilizadorId"] = new SelectList(_context.Utilizadors, "Id", "Id", vendum.UtilizadorId);
            return View(vendum);
        }

        // GET: Vendas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (VerifyAdmin() == 0)
            {
                return RedirectToAction("index", "home");
            }
            else
            {
                if (id == null || _context.Venda == null)
                {
                    return NotFound();
                }

                var vendum = await _context.Venda
                    .Include(v => v.Stand)
                    .Include(v => v.Utilizador)
                    .FirstOrDefaultAsync(m => m.IdVenda == id);
                if (vendum == null)
                {
                    return NotFound();
                }

                return View(vendum);
            }
        }

        // POST: Vendas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Venda == null)
            {
                return Problem("Entity set 'WebFayreContext.Venda'  is null.");
            }
            var vendum = await _context.Venda.FindAsync(id);
            if (vendum != null)
            {
                _context.Venda.Remove(vendum);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VendumExists(int id)
        {
          return (_context.Venda?.Any(e => e.IdVenda == id)).GetValueOrDefault();
        }
    }
}
