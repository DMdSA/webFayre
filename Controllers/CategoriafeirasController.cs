﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebFayre.Models;

namespace WebFayre.Controllers
{
    public class CategoriafeirasController : Controller
    {
        private readonly WebFayreContext _context;

        public CategoriafeirasController(WebFayreContext context)
        {
            _context = context;
        }

        private Boolean userHasSession()
        {
            return (HttpContext.Session.GetInt32("utilizadorId") != null);
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

        // GET: Categoriafeiras
        public async Task<IActionResult> Index()
        {
            if (VerifyAdmin() == 0)
            {
                return RedirectToAction("index", "home");
            }
            else
            {
                return _context.Categoriafeiras != null ?
                          View(await _context.Categoriafeiras.ToListAsync()) :
                          Problem("Entity set 'WebFayreContext.Categoriafeiras'  is null.");
            }
        }

        // GET: Categoriafeiras/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (VerifyAdmin() == 0)
            {
                return RedirectToAction("index", "home");
            }
            else
            {
                if (id == null || _context.Categoriafeiras == null)
                {
                    return NotFound();
                }

                var categoriafeira = await _context.Categoriafeiras
                    .FirstOrDefaultAsync(m => m.IdCategoriaFeira == id);
                if (categoriafeira == null)
                {
                    return NotFound();
                }

                return View(categoriafeira);
            }
        }

        public async Task RemoveFeiraAsync(Feira feira, int idCateg)
        {
            var categoriafeira = await _context.Categoriafeiras
                .FirstOrDefaultAsync(m => m.IdCategoriaFeira == idCateg);
            categoriafeira.Feiras.Remove(feira);
        }

        // GET: Categoriafeiras/Create
        public IActionResult Create()
        {
            if (VerifyAdmin() == 0)
            {
                return RedirectToAction("index", "home");
            }
            else
            {
                return View();
            }
        }

        // POST: Categoriafeiras/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCategoriaFeira,Descricao")] Categoriafeira categoriafeira)
        {
                _context.Categoriafeiras.Include(f => f.Feiras);

                if (ModelState.IsValid)
                {
                    _context.Add(categoriafeira);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(categoriafeira);
        }

        // GET: Categoriafeiras/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (VerifyAdmin() == 0)
            {
                return RedirectToAction("index", "home");
            }
            else
            {
                if (id == null || _context.Categoriafeiras == null)
                {
                    return NotFound();
                }

                var categoriafeira = await _context.Categoriafeiras.FindAsync(id);
                if (categoriafeira == null)
                {
                    return NotFound();
                }
                return View(categoriafeira);
            }
        }

        // POST: Categoriafeiras/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCategoriaFeira,Descricao")] Categoriafeira categoriafeira)
        {
                if (id != categoriafeira.IdCategoriaFeira)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(categoriafeira);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!CategoriafeiraExists(categoriafeira.IdCategoriaFeira))
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
                return View(categoriafeira);
            }

        // GET: Categoriafeiras/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (VerifyAdmin() == 0)
            {
                return RedirectToAction("index", "home");
            }
            else
            {
                if (id == null || _context.Categoriafeiras == null)
                {
                    return NotFound();
                }

                var categoriafeira = await _context.Categoriafeiras
                    .FirstOrDefaultAsync(m => m.IdCategoriaFeira == id);
                if (categoriafeira == null)
                {
                    return NotFound();
                }

                return View(categoriafeira);
            }
        }

        // POST: Categoriafeiras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
                if (_context.Categoriafeiras == null)
                {
                    return Problem("Entity set 'WebFayreContext.Categoriafeiras'  is null.");
                }
                var categoriafeira = await _context.Categoriafeiras.FindAsync(id);
                if (categoriafeira != null)
                {
                    _context.Categoriafeiras.Remove(categoriafeira);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
        }

        private bool CategoriafeiraExists(int id)
        {
          return (_context.Categoriafeiras?.Any(e => e.IdCategoriaFeira == id)).GetValueOrDefault();
        }
    }
}
