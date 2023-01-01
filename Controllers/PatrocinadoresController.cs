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
    public class PatrocinadoresController : Controller
    {
        private readonly WebFayreContext _context;

        public PatrocinadoresController(WebFayreContext context)
        {
            _context = context;
        }

        // GET: Patrocinadores
        public async Task<IActionResult> Index()
        {
              return _context.Patrocinadors != null ? 
                          View(await _context.Patrocinadors.ToListAsync()) :
                          Problem("Entity set 'WebFayreContext.Patrocinadors'  is null.");
        }

        public async Task RemoveFeiraAsync(Feira feira, int idPatroc)
        {
            var patrocinadores = await _context.Patrocinadors
                .FirstOrDefaultAsync(m => m.IdPatrocinador == idPatroc);
            patrocinadores.Feiras.Remove(feira);
        }

        // GET: Patrocinadores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Patrocinadors == null)
            {
                return NotFound();
            }

            var patrocinador = await _context.Patrocinadors
                .FirstOrDefaultAsync(m => m.IdPatrocinador == id);
            if (patrocinador == null)
            {
                return NotFound();
            }

            return View(patrocinador);
        }

        // GET: Patrocinadores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Patrocinadores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPatrocinador,Nome,Email,Descricao,Telefone")] Patrocinador patrocinador)
        {
            if (ModelState.IsValid)
            {
                _context.Add(patrocinador);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(patrocinador);
        }

        // GET: Patrocinadores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Patrocinadors == null)
            {
                return NotFound();
            }

            var patrocinador = await _context.Patrocinadors.FindAsync(id);
            if (patrocinador == null)
            {
                return NotFound();
            }
            return View(patrocinador);
        }

        // POST: Patrocinadores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPatrocinador,Nome,Email,Descricao,Telefone")] Patrocinador patrocinador)
        {
            if (id != patrocinador.IdPatrocinador)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(patrocinador);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatrocinadorExists(patrocinador.IdPatrocinador))
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
            return View(patrocinador);
        }

        // GET: Patrocinadores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Patrocinadors == null)
            {
                return NotFound();
            }

            var patrocinador = await _context.Patrocinadors
                .FirstOrDefaultAsync(m => m.IdPatrocinador == id);
            if (patrocinador == null)
            {
                return NotFound();
            }

            return View(patrocinador);
        }

        // POST: Patrocinadores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Patrocinadors == null)
            {
                return Problem("Entity set 'WebFayreContext.Patrocinadors'  is null.");
            }
            var patrocinador = await _context.Patrocinadors.FindAsync(id);
            if (patrocinador != null)
            {
                _context.Patrocinadors.Remove(patrocinador);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PatrocinadorExists(int id)
        {
          return (_context.Patrocinadors?.Any(e => e.IdPatrocinador == id)).GetValueOrDefault();
        }
    }
}
