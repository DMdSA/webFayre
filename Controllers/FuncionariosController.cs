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
    public class FuncionariosController : Controller
    {
        private readonly WebFayreContext _context;

        public FuncionariosController(WebFayreContext context)
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

        private int getUserType()
        {
            return (int)HttpContext.Session.GetInt32("isFuncionario");
        }

        private int VerifyAdmin()
        {
            if (HttpContext.Session.GetInt32("utilizadorId") == null || getFuncFuncao() != "Admin")
                return 0;
            else
                return 1;
        }

        // GET: Funcionarios
        public async Task<IActionResult> Index()
        {
            if (VerifyAdmin() == 0)
            {
                return RedirectToAction("index", "home");
            }
            else
            {
                var webFayreContext = _context.Funcionarios.Include(f => f.FuncaoNavigation);
                return View(await webFayreContext.ToListAsync());
            }
        }

        // GET: Funcionarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (VerifyAdmin() == 0)
            {
                return RedirectToAction("index", "home");
            }
            else
            {
                if (id == null || _context.Funcionarios == null)
                {
                    return NotFound();
                }

                var funcionario = await _context.Funcionarios
                    .Include(f => f.FuncaoNavigation)
                    .FirstOrDefaultAsync(m => m.IdFuncionario == id);
                if (funcionario == null)
                {
                    return NotFound();
                }

                return View(funcionario);
            }
        }

        // GET: Funcionarios/Create
        public IActionResult Create()
        {
            if (VerifyAdmin() == 0)
            {
                return RedirectToAction("index", "home");
            }
            else
            {
                if (HttpContext.Session.GetInt32("utilizadorId") == null || getFuncFuncao() != "Admin")
                    return RedirectToAction("login", "home");
                ViewData["Funcao"] = new SelectList(_context.Funcaos, "IdFuncao", "IdFuncao");
                return View();
            }
        }

        // POST: Funcionarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdFuncionario,Nome,Email,Password,Telemovel,CreationDate,FuncionarioPath,Funcao")] Funcionario funcionario)
        {
                //await _context.Entry(funcionario).Reference(f => f.FuncaoNavigation).LoadAsync();
                await _context.Funcionarios.Include(f => f.FuncaoNavigation).LoadAsync();

                if (ModelState.IsValid)
                {
                    var funcList = _context.Funcionarios.ToList();
                    foreach (var func in funcList)
                    {
                        if (funcionario.Email == func.Email)
                        {
                            return RedirectToAction("index", "home");
                        }
                    }
                    var userList = _context.Utilizadors.ToList();
                    foreach (var user in userList)
                    {
                        if (funcionario.Email == user.Email)
                        {
                            return RedirectToAction("index", "home");
                        }
                    }

                    funcionario.Telemovel = funcionario.Telemovel.Replace(" ", "");
                    _context.Add(funcionario);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                ViewData["Funcao"] = new SelectList(_context.Funcaos, "IdFuncao", "IdFuncao", funcionario.Funcao);
                return View(funcionario);
        }

        // GET: Funcionarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (VerifyAdmin() == 0)
            {
                return RedirectToAction("index", "home");
            }
            else
            {
                if (id == null || _context.Funcionarios == null)
                {
                    return NotFound();
                }

                var funcionario = await _context.Funcionarios.FindAsync(id);
                if (funcionario == null)
                {
                    return NotFound();
                }
                ViewData["Funcao"] = new SelectList(_context.Funcaos, "IdFuncao", "IdFuncao", funcionario.Funcao);
                return View(funcionario);
            }
        }

        // POST: Funcionarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdFuncionario,Nome,Email,Password,Telemovel,CreationDate,FuncionarioPath,Funcao")] Funcionario funcionario)
        {
                if (id != funcionario.IdFuncionario)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(funcionario);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!FuncionarioExists(funcionario.IdFuncionario))
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
                ViewData["Funcao"] = new SelectList(_context.Funcaos, "IdFuncao", "IdFuncao", funcionario.Funcao);
                return View(funcionario);
        }

        // GET: Funcionarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (VerifyAdmin() == 0)
            {
                return RedirectToAction("index", "home");
            }
            else
            {
                if (id == null || _context.Funcionarios == null)
                {
                    return NotFound();
                }

                var funcionario = await _context.Funcionarios
                    .Include(f => f.FuncaoNavigation)
                    .FirstOrDefaultAsync(m => m.IdFuncionario == id);
                if (funcionario == null)
                {
                    return NotFound();
                }

                return View(funcionario);
            }
        }

        // POST: Funcionarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
                if (_context.Funcionarios == null)
                {
                    return Problem("Entity set 'WebFayreContext.Funcionarios'  is null.");
                }
                var funcionario = await _context.Funcionarios.FindAsync(id);
                if (funcionario != null)
                {
                    _context.Funcionarios.Remove(funcionario);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
        }

        private bool FuncionarioExists(int id)
        {
          return (_context.Funcionarios?.Any(e => e.IdFuncionario == id)).GetValueOrDefault();
        }

        public async Task<IActionResult> ViewProfile()
        {
            if (!userHasSession() || getUserType() != 1)
            {
                return RedirectToAction("login", "home");
            }
            else
            {
                var userid = (int)HttpContext.Session.GetInt32("utilizadorId");
                var user = await _context.Funcionarios.FirstOrDefaultAsync(m => m.IdFuncionario == userid);
                ViewBag.Nome = user.Nome;
                return View(user);
            }
        }
    }
}
