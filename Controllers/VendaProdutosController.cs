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
    public class VendaProdutosController : Controller
    {
        private readonly WebFayreContext _context;

        public VendaProdutosController(WebFayreContext context)
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

        // GET: VendaProdutos
        public async Task<IActionResult> Index()
        {
            if (VerifyAdmin() == 0)
            {
                return RedirectToAction("index", "home");
            }
            else
            {
                var webFayreContext = _context.VendaProdutos.Include(v => v.Produto).Include(v => v.Venda);
                return View(await webFayreContext.ToListAsync());
            }
        }

        // GET: VendaProdutos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.VendaProdutos == null)
            {
                return NotFound();
            }

            var vendaProduto = await _context.VendaProdutos
                .Include(v => v.Produto)
                .Include(v => v.Venda)
                .FirstOrDefaultAsync(m => m.VendaId == id);
            if (vendaProduto == null)
            {
                return NotFound();
            }

            return View(vendaProduto);
        }

        // GET: VendaProdutos/Create
        public IActionResult Create()
        {
            if (VerifyAdmin() == 0)
            {
                return RedirectToAction("index", "home");
            }
            else
            {
                ViewData["ProdutoId"] = new SelectList(_context.Produtos, "IdProduto", "IdProduto");
                ViewData["VendaId"] = new SelectList(_context.Venda, "IdVenda", "IdVenda");
                return View();
            }
        }

        // POST: VendaProdutos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VendaId,ProdutoId,Preco,Quantidade")] VendaProduto vendaProduto)
        {
            await _context.VendaProdutos.Include(vp => vp.Produto).Include(vp => vp.Venda).LoadAsync();
            if (ModelState.IsValid)
            {
                _context.Add(vendaProduto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProdutoId"] = new SelectList(_context.Produtos, "IdProduto", "IdProduto", vendaProduto.ProdutoId);
            ViewData["VendaId"] = new SelectList(_context.Venda, "IdVenda", "IdVenda", vendaProduto.VendaId);
            return View(vendaProduto);
        }

        // GET: VendaProdutos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (VerifyAdmin() == 0)
            {
                return RedirectToAction("index", "home");
            }
            else
            {
                if (id == null || _context.VendaProdutos == null)
                {
                    return NotFound();
                }

                var vendaProduto = await _context.VendaProdutos.FindAsync(id);
                if (vendaProduto == null)
                {
                    return NotFound();
                }
                ViewData["ProdutoId"] = new SelectList(_context.Produtos, "IdProduto", "IdProduto", vendaProduto.ProdutoId);
                ViewData["VendaId"] = new SelectList(_context.Venda, "IdVenda", "IdVenda", vendaProduto.VendaId);
                return View(vendaProduto);
            }
        }

        // POST: VendaProdutos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VendaId,ProdutoId,Preco,Quantidade")] VendaProduto vendaProduto)
        {
            if (id != vendaProduto.VendaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vendaProduto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VendaProdutoExists(vendaProduto.VendaId))
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
            ViewData["ProdutoId"] = new SelectList(_context.Produtos, "IdProduto", "IdProduto", vendaProduto.ProdutoId);
            ViewData["VendaId"] = new SelectList(_context.Venda, "IdVenda", "IdVenda", vendaProduto.VendaId);
            return View(vendaProduto);
        }

        // GET: VendaProdutos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (VerifyAdmin() == 0)
            {
                return RedirectToAction("index", "home");
            }
            else
            {
                if (id == null || _context.VendaProdutos == null)
                {
                    return NotFound();
                }

                var vendaProduto = await _context.VendaProdutos
                    .Include(v => v.Produto)
                    .Include(v => v.Venda)
                    .FirstOrDefaultAsync(m => m.VendaId == id);
                if (vendaProduto == null)
                {
                    return NotFound();
                }

                return View(vendaProduto);
            }
        }

        // POST: VendaProdutos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.VendaProdutos == null)
            {
                return Problem("Entity set 'WebFayreContext.VendaProdutos'  is null.");
            }
            var vendaProduto = await _context.VendaProdutos.FindAsync(id);
            if (vendaProduto != null)
            {
                _context.VendaProdutos.Remove(vendaProduto);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VendaProdutoExists(int id)
        {
          return (_context.VendaProdutos?.Any(e => e.VendaId == id)).GetValueOrDefault();
        }
    }
}
