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
    public class TipoStandsController : Controller
    {
        private readonly WebFayreContext _context;

        public TipoStandsController(WebFayreContext context)
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


        // GET: TipoStands
        public async Task<IActionResult> Index()
        {
            if (VerifyAdmin() == 0)
            {
                return RedirectToAction("index", "home");
            }
            else
            {
                return _context.TipoStands != null ?
                          View(await _context.TipoStands.ToListAsync()) :
                          Problem("Entity set 'WebFayreContext.TipoStands'  is null.");
            }
        }

        // GET: TipoStands/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (VerifyAdmin() == 0)
            {
                return RedirectToAction("index", "home");
            }
            else
            {
                if (id == null || _context.TipoStands == null)
                {
                    return NotFound();
                }

                var tipoStand = await _context.TipoStands
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (tipoStand == null)
                {
                    return NotFound();
                }

                return View(tipoStand);
            }
        }

        // GET: TipoStands/Create
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

        // POST: TipoStands/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Descricao")] TipoStand tipoStand)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tipoStand);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tipoStand);
        }

        // GET: TipoStands/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (VerifyAdmin() == 0)
            {
                return RedirectToAction("index", "home");
            }
            else
            {
                if (id == null || _context.TipoStands == null)
                {
                    return NotFound();
                }

                var tipoStand = await _context.TipoStands.FindAsync(id);
                if (tipoStand == null)
                {
                    return NotFound();
                }
                return View(tipoStand);
            }
        }

        // POST: TipoStands/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Descricao")] TipoStand tipoStand)
        {
            if (id != tipoStand.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tipoStand);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipoStandExists(tipoStand.Id))
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
            return View(tipoStand);
        }

        // GET: TipoStands/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (VerifyAdmin() == 0)
            {
                return RedirectToAction("index", "home");
            }
            else
            {
                if (id == null || _context.TipoStands == null)
                {
                    return NotFound();
                }

                var tipoStand = await _context.TipoStands
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (tipoStand == null)
                {
                    return NotFound();
                }

                return View(tipoStand);
            }
        }

        // POST: TipoStands/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TipoStands == null)
            {
                return Problem("Entity set 'WebFayreContext.TipoStands'  is null.");
            }
            var tipoStand = await _context.TipoStands.FindAsync(id);
            if (tipoStand != null)
            {
                _context.TipoStands.Remove(tipoStand);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TipoStandExists(int id)
        {
          return (_context.TipoStands?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
