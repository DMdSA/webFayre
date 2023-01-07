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
    public class StandstaffsController : Controller
    {
        private readonly WebFayreContext _context;

        public StandstaffsController(WebFayreContext context)
        {
            _context = context;
        }

        // GET: Standstaffs
        public async Task<IActionResult> Index()
        {
            var webFayreContext = _context.Standstaffs.Include(s => s.IdStandNavigation);
            return View(await webFayreContext.ToListAsync());
        }

        // GET: Standstaffs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Standstaffs == null)
            {
                return NotFound();
            }

            var standstaff = await _context.Standstaffs
                .Include(s => s.IdStandNavigation)
                .FirstOrDefaultAsync(m => m.IdStand == id);
            if (standstaff == null)
            {
                return NotFound();
            }

            return View(standstaff);
        }

        // GET: Standstaffs/Create
        public IActionResult Create()
        {
            ViewData["IdStand"] = new SelectList(_context.Stands, "IdStand", "Nome");
            ViewData["Emails"] = new SelectList(_context.Utilizadors, "Email", "Email");
            return View();
        }

        // POST: Standstaffs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdStand,StaffEmail")] Standstaff standstaff)
        {
            if (ModelState.IsValid)
            {
                _context.Add(standstaff);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdStand"] = new SelectList(_context.Stands, "IdStand", "Nome", standstaff.IdStand);
            ViewData["Emails"] = new SelectList(_context.Utilizadors, "Email", "Email");
            return View(standstaff);
        }

        // GET: Standstaffs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Standstaffs == null)
            {
                return NotFound();
            }

            var standstaff = await _context.Standstaffs.FindAsync(id);
            if (standstaff == null)
            {
                return NotFound();
            }
            ViewData["IdStand"] = new SelectList(_context.Stands, "IdStand", "IdStand", standstaff.IdStand);
            return View(standstaff);
        }

        // POST: Standstaffs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdStand,StaffEmail")] Standstaff standstaff)
        {
            if (id != standstaff.IdStand)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(standstaff);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StandstaffExists(standstaff.IdStand))
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
            ViewData["IdStand"] = new SelectList(_context.Stands, "IdStand", "IdStand", standstaff.IdStand);
            return View(standstaff);
        }

        // GET: Standstaffs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Standstaffs == null)
            {
                return NotFound();
            }

            var standstaff = await _context.Standstaffs
                .Include(s => s.IdStandNavigation)
                .FirstOrDefaultAsync(m => m.IdStand == id);
            if (standstaff == null)
            {
                return NotFound();
            }

            return View(standstaff);
        }

        // POST: Standstaffs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Standstaffs == null)
            {
                return Problem("Entity set 'WebFayreContext.Standstaffs'  is null.");
            }
            var standstaff = await _context.Standstaffs.FindAsync(id);
            if (standstaff != null)
            {
                _context.Standstaffs.Remove(standstaff);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StandstaffExists(int id)
        {
          return (_context.Standstaffs?.Any(e => e.IdStand == id)).GetValueOrDefault();
        }
    }
}
