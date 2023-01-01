using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using WebFayre.Models;

namespace WebFayre.Controllers
{
    public class PromocaofeirasController : Controller
    {
        private readonly WebFayreContext _context;

        public PromocaofeirasController(WebFayreContext context)
        {
            _context = context;
        }
        private int getUserType()
        {
            return (int)HttpContext.Session.GetInt32("isFuncionario");
        }

        // GET: Promocaofeiras
        public async Task<IActionResult> Index()
        {
            var webFayreContext = _context.Promocaofeiras.Include(p => p.IdUtilizadorNavigation);
            return View(await webFayreContext.ToListAsync());
        }

        public async Task<IActionResult> IndexByUser()
        {
            var userid = (int)HttpContext.Session.GetInt32("utilizadorId");
            var webFayreContext = _context.Promocaofeiras.Include(p => p.IdUtilizadorNavigation).Where(p => p.IdUtilizador == userid);
            return View(await webFayreContext.ToListAsync());
        }

        // GET: Promocaofeiras/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Promocaofeiras == null)
            {
                return NotFound();
            }

            var promocaofeira = await _context.Promocaofeiras
                .Include(p => p.IdUtilizadorNavigation)
                .FirstOrDefaultAsync(m => m.IdPromocaoFeira == id);
            if (promocaofeira == null)
            {
                return NotFound();
            }

            return View(promocaofeira);
        }

        // GET: Promocaofeiras/Create
        public IActionResult Create()
        {
            ViewData["IdUtilizador"] = new SelectList(_context.Utilizadors, "Id", "Id");
            return View();
        }

        // POST: Promocaofeiras/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPromocaoFeira,CapacidadeUtilizadores,Descricao,Nome,NStands,IdUtilizador,IdFuncionario")] Promocaofeira promocaofeira)
        {
            if (ModelState.IsValid)
            {
                var userid = HttpContext.Session.GetInt32("utilizadorId");
                promocaofeira.IdUtilizador = (int)userid;
                promocaofeira.IdFuncionario = null;
                _context.Add(promocaofeira);
                await _context.SaveChangesAsync();
                if (getUserType() == 0)
                {
                    return RedirectToAction("IndexByUser");
                }
                else
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewData["IdUtilizador"] = new SelectList(_context.Utilizadors, "Id", "Id", promocaofeira.IdUtilizador);
            return View(promocaofeira);
        }

        // GET: Promocaofeiras/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Promocaofeiras == null)
            {
                return NotFound();
            }

            var promocaofeira = await _context.Promocaofeiras.FindAsync(id);
            if (promocaofeira == null)
            {
                return NotFound();
            }
            ViewData["IdUtilizador"] = new SelectList(_context.Utilizadors, "Id", "Id", promocaofeira.IdUtilizador);
            return View(promocaofeira);
        }

        // POST: Promocaofeiras/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPromocaoFeira,CapacidadeUtilizadores,Descricao,Nome,NStands,IdUtilizador,IdFuncionario")] Promocaofeira promocaofeira)
        {
            if (id != promocaofeira.IdPromocaoFeira)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(promocaofeira);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PromocaofeiraExists(promocaofeira.IdPromocaoFeira))
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
            ViewData["IdUtilizador"] = new SelectList(_context.Utilizadors, "Id", "Id", promocaofeira.IdUtilizador);
            return View(promocaofeira);
        }

        // GET: Promocaofeiras/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Promocaofeiras == null)
            {
                return NotFound();
            }

            var promocaofeira = await _context.Promocaofeiras
                .Include(p => p.IdUtilizadorNavigation)
                .FirstOrDefaultAsync(m => m.IdPromocaoFeira == id);
            if (promocaofeira == null)
            {
                return NotFound();
            }

            return View(promocaofeira);
        }

        // POST: Promocaofeiras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Promocaofeiras == null)
            {
                return Problem("Entity set 'WebFayreContext.Promocaofeiras'  is null.");
            }
            var promocaofeira = await _context.Promocaofeiras.FindAsync(id);
            if (promocaofeira != null)
            {
                _context.Promocaofeiras.Remove(promocaofeira);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PromocaofeiraExists(int id)
        {
          return (_context.Promocaofeiras?.Any(e => e.IdPromocaoFeira == id)).GetValueOrDefault();
        }
    }
}
