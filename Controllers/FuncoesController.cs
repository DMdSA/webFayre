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
    public class FuncoesController : Controller
    {
        private readonly WebFayreContext _context;

        public FuncoesController(WebFayreContext context)
        {
            _context = context;
        }

        // GET: Funcoes
        public async Task<IActionResult> Index()
        {
              return _context.Funcaos != null ? 
                          View(await _context.Funcaos.ToListAsync()) :
                          Problem("Entity set 'WebFayreContext.Funcaos'  is null.");
        }

        // GET: Funcoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Funcaos == null)
            {
                return NotFound();
            }

            var funcao = await _context.Funcaos
                .FirstOrDefaultAsync(m => m.IdFuncao == id);
            if (funcao == null)
            {
                return NotFound();
            }

            return View(funcao);
        }

        // GET: Funcoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Funcoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdFuncao,Descricao")] Funcao funcao)
        {
            if (ModelState.IsValid)
            {
                _context.Add(funcao);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(funcao);
        }

        // GET: Funcoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Funcaos == null)
            {
                return NotFound();
            }

            var funcao = await _context.Funcaos.FindAsync(id);
            if (funcao == null)
            {
                return NotFound();
            }
            return View(funcao);
        }

        // POST: Funcoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdFuncao,Descricao")] Funcao funcao)
        {
            if (id != funcao.IdFuncao)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(funcao);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FuncaoExists(funcao.IdFuncao))
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
            return View(funcao);
        }

        // GET: Funcoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Funcaos == null)
            {
                return NotFound();
            }

            var funcao = await _context.Funcaos
                .FirstOrDefaultAsync(m => m.IdFuncao == id);
            if (funcao == null)
            {
                return NotFound();
            }

            return View(funcao);
        }

        // POST: Funcoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Funcaos == null)
            {
                return Problem("Entity set 'WebFayreContext.Funcaos'  is null.");
            }
            var funcao = await _context.Funcaos.FindAsync(id);
            if (funcao != null)
            {
                _context.Funcaos.Remove(funcao);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FuncaoExists(int id)
        {
          return (_context.Funcaos?.Any(e => e.IdFuncao == id)).GetValueOrDefault();
        }
    }
}
