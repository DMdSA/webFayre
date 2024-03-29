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
    public class TicketsController : Controller
    {
        private readonly WebFayreContext _context;

        public TicketsController(WebFayreContext context)
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

        // GET: Tickets
        public async Task<IActionResult> Index()
        {
            if (VerifyAdmin() == 0)
            {
                return RedirectToAction("index", "home");
            }
            else
            {
                var webFayreContext = _context.Tickets.Include(t => t.Feira).Include(t => t.Utilizador);
                return View(await webFayreContext.ToListAsync());
            }
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (VerifyAdmin() == 0)
            {
                return RedirectToAction("index", "home");
            }
            else
            {
                if (id == null || _context.Tickets == null)
                {
                    return NotFound();
                }

                var ticket = await _context.Tickets
                    .Include(t => t.Feira)
                    .Include(t => t.Utilizador)
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (ticket == null)
                {
                    return NotFound();
                }

                return View(ticket);
            }
        }

        // GET: Tickets/Create
        public IActionResult Create()
        {
            if (VerifyAdmin() == 0)
            {
                return RedirectToAction("index", "home");
            }
            else
            {
                ViewData["FeiraId"] = new SelectList(_context.Feiras, "IdFeira", "IdFeira");
                ViewData["UtilizadorId"] = new SelectList(_context.Utilizadors, "Id", "Id");
                return View();
            }
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Data,UtilizadorId,FeiraId")] Ticket ticket)
        {
            {
                await _context.Tickets.Include(t => t.Feira).Include(t => t.Utilizador).LoadAsync();
                if (ModelState.IsValid)
                {
                    _context.Add(ticket);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                ViewData["FeiraId"] = new SelectList(_context.Feiras, "IdFeira", "IdFeira", ticket.FeiraId);
                ViewData["UtilizadorId"] = new SelectList(_context.Utilizadors, "Id", "Id", ticket.UtilizadorId);
                return View(ticket);
            }
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (VerifyAdmin() == 0)
            {
                return RedirectToAction("index", "home");
            }
            else
            {
                if (id == null || _context.Tickets == null)
                {
                    return NotFound();
                }

                var ticket = await _context.Tickets.FindAsync(id);
                if (ticket == null)
                {
                    return NotFound();
                }
                ViewData["FeiraId"] = new SelectList(_context.Feiras, "IdFeira", "IdFeira", ticket.FeiraId);
                ViewData["UtilizadorId"] = new SelectList(_context.Utilizadors, "Id", "Id", ticket.UtilizadorId);
                return View(ticket);
            }
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Data,UtilizadorId,FeiraId")] Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.Id))
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
            ViewData["FeiraId"] = new SelectList(_context.Feiras, "IdFeira", "IdFeira", ticket.FeiraId);
            ViewData["UtilizadorId"] = new SelectList(_context.Utilizadors, "Id", "Id", ticket.UtilizadorId);
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (VerifyAdmin() == 0)
            {
                return RedirectToAction("index", "home");
            }
            else
            {
                if (id == null || _context.Tickets == null)
                {
                    return NotFound();
                }

                var ticket = await _context.Tickets
                    .Include(t => t.Feira)
                    .Include(t => t.Utilizador)
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (ticket == null)
                {
                    return NotFound();
                }

                return View(ticket);
            }
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Tickets == null)
            {
                return Problem("Entity set 'WebFayreContext.Tickets'  is null.");
            }
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket != null)
            {
                _context.Tickets.Remove(ticket);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(int id)
        {
          return (_context.Tickets?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public async Task RemoveTicket(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket != null)
            {
                _context.Tickets.Remove(ticket);
            }
        }
    }
}
