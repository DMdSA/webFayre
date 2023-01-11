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


        // GET: Standstaffs
        public async Task<IActionResult> Index()
        {
            if (VerifyAdmin() == 0)
            {
                return RedirectToAction("index", "home");
            }
            else
            {
                var webFayreContext = _context.Standstaffs.Include(s => s.IdStandNavigation);
                return View(await webFayreContext.ToListAsync());
            }
        }
        // GET: Standstaffs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (VerifyAdmin() == 0)
            {
                return RedirectToAction("index", "home");
            }
            else
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
        }

        public IActionResult CreateStaff()
        {
            if (VerifyAdmin() == 0)
            {
                return RedirectToAction("index", "home");
            }
            else
            {
                ViewData["IdStand"] = new SelectList(_context.Stands, "IdStand", "Nome");
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateStaff([Bind("IdStand,StaffEmail")] Standstaff standstaff)
        {
            if (VerifyAdmin() == 0)
            {
                return RedirectToAction("index", "home");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    //Emails de todos os utilizadores para verificar se o email introduzido é válido
                    var emails = _context.Utilizadors.ToList().Select(s => s.Email);
                    //Emails de todos os staffs do stand pretendido para verificar se o email introduzido já faz parte do staff desse stand
                    var jobs = _context.Standstaffs.Where(s => s.IdStand == standstaff.IdStand).ToList().Select(s => s.StaffEmail);
                    if (emails.Contains(standstaff.StaffEmail) && !jobs.Contains(standstaff.StaffEmail))
                    {
                        _context.Add(standstaff);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }
                ViewData["IdStand"] = new SelectList(_context.Stands, "IdStand", "Nome", standstaff.IdStand);
                return View(standstaff);
            }
        }

        // GET: Standstaffs/Create
        public IActionResult Create()
        {
            if (VerifyAdmin() == 0)
            {
                return RedirectToAction("index", "home");
            }
            else
            {
                ViewData["IdStand"] = new SelectList(_context.Stands, "IdStand", "Nome");
                ViewData["Emails"] = new SelectList(_context.Utilizadors, "Email", "Email");
                return View();
            }
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
            if (VerifyAdmin() == 0)
            {
                return RedirectToAction("index", "home");
            }
            else
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
            if (VerifyAdmin() == 0)
            {
                return RedirectToAction("index", "home");
            }
            else
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
