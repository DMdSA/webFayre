using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebFayre.Common;
using WebFayre.Models;

namespace WebFayre.Controllers
{
    public class ProdutosController : Controller
    {
        private readonly WebFayreContext _context;

        public ProdutosController(WebFayreContext context)
        {
            _context = context;
        }

        private int getUserId()
        {
            return (int)HttpContext.Session.GetInt32("utilizadorId");
        }

        // GET: Produtoes
        public async Task<IActionResult> Index()
        {
            var webFayreContext = _context.Produtos.Include(p => p.Stand);
            return View(await webFayreContext.ToListAsync());
        }

        public async Task<IActionResult> ProdutosByStand(int feiraId, int id)
        {
            StandShoppingCart ssc = new StandShoppingCart();
            ssc.StandId = id;
            ssc.FeiraId = feiraId;
            ssc.Products = new List<ProductInfo>();
            ViewBag.StandShoppingCart = ssc;
            //if (HttpContext.Session.GetObject<StandShoppingCart>("StandShoppingCart") != null)
            //    ViewBag.StandShoppingCart = HttpContext.Session.GetObject<StandShoppingCart>("StandShoppingCart");
            
            var prodList = _context.Produtos.Include(s => s.Stand).Where(s => s.StandId == id);
            var stand = await _context.Stands.Where(p => p.IdStand == id).FirstOrDefaultAsync();
            if (stand != null)
                ViewBag.NomeStand = stand.Nome;
            return View(await prodList.ToListAsync());
        }

            // GET: Produtoes/Details/5
            public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Produtos == null)
            {
                return NotFound();
            }

            var produto = await _context.Produtos
                .Include(p => p.Stand)
                .FirstOrDefaultAsync(m => m.IdProduto == id);
            if (produto == null)
            {
                return NotFound();
            }

            return View(produto);
        }

        // GET: Produtoes/Create
        public IActionResult Create()
        {
            ViewData["StandId"] = new SelectList(_context.Stands, "IdStand", "Nome");
            return View();
        }

        // POST: Produtoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdProduto,Stock,Descricao,Preco,Iva,ImagemPath,StandId")] Produto produto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(produto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StandId"] = new SelectList(_context.Stands, "IdStand", "Nome", produto.StandId);
            return View(produto);
        }

        // GET: Produtoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Produtos == null)
            {
                return NotFound();
            }

            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null)
            {
                return NotFound();
            }
            ViewData["StandId"] = new SelectList(_context.Stands, "IdStand", "Nome", produto.StandId);
            return View(produto);
        }

        // POST: Produtoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdProduto,Stock,Descricao,Preco,Iva,ImagemPath,StandId")] Produto produto)
        {
            if (id != produto.IdProduto)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(produto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProdutoExists(produto.IdProduto))
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
            ViewData["StandId"] = new SelectList(_context.Stands, "IdStand", "Nome", produto.StandId);
            return View(produto);
        }

        // GET: Produtoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Produtos == null)
            {
                return NotFound();
            }

            var produto = await _context.Produtos
                .Include(p => p.Stand)
                .FirstOrDefaultAsync(m => m.IdProduto == id);
            if (produto == null)
            {
                return NotFound();
            }

            return View(produto);
        }

        // POST: Produtoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Produtos == null)
            {
                return Problem("Entity set 'WebFayreContext.Produtos'  is null.");
            }
            var produto = await _context.Produtos.FindAsync(id);
            if (produto != null)
            {
                _context.Produtos.Remove(produto);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProdutoExists(int id)
        {
            return (_context.Produtos?.Any(e => e.IdProduto == id)).GetValueOrDefault();
        }

        public async Task RemoveProduto(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto != null)
            {
                _context.Produtos.Remove(produto);
            }
        }


        [HttpPost]
        public async Task<StandShoppingCart> ReadJsonCart([FromBody] StandShoppingCart ssc)
        {
            List<ProductInfo> products = ssc.Products;
            products.RemoveAll(p => p == null);
            ssc.Products = products;

            int standId = ssc.StandId;
            int feiraId = ssc.FeiraId;

            HttpContext.Session.SetObject("CartObject", ssc);
            return ssc;
        }

        [HttpPost]
        public async Task<IActionResult> ViewCart()
        {

            StandShoppingCart? ssc = HttpContext.Session.GetObject<StandShoppingCart>("CartObject");
            if (ssc == null) return NoContent();
            decimal total = calculateTotal(ssc.Products);
            ViewBag.Total = total;
            return View(ssc);
        }

        public decimal calculateTotal(List<ProductInfo> products)
        {
            decimal total = 0;
            foreach (var item in products)
            {
                total += item.FinalPrice;
            }
            return total;
        }

        [HttpGet]
        public IActionResult FinalizePurchase()
        {
            int id = getUserId();
            var user = _context.Utilizadors.FirstOrDefault(x => x.Id == id);
            ViewBag.Nif = user.Nif;
            ViewBag.Telemovel = user.Telemovel;
            return View();
        }

        public async Task<IActionResult> FinalizePurchase(string nif, string tel)
        {
            if (HttpContext.Session.GetInt32("utilizadorId") == null)
            {
                RedirectToAction("login", "home");
            }
            int userid = (int)HttpContext.Session.GetInt32("utilizadorId");

            //@todo -atualizar stock; registar a compra; redirect correto para lista de produtos
            Console.WriteLine(nif);
            Console.WriteLine(tel);

            // se for finalizada..
            StandShoppingCart? ssc = HttpContext.Session.GetObject<StandShoppingCart>("CartObject");
            
            // aqui devia ser redirecionado para o sitio devido
            if (ssc == null) return NoContent() ;

            // lista de produtos no carrinho
            List<ProductInfo> productsInfo = ssc.Products;
            // var com os ids de cada produto retirado
            var productsIds = productsInfo.Select(x => x.Id);
            // lista com a relacao entre a venda e os produtos - para a bd
            List<VendaProduto> vendaProdutos = new List<VendaProduto>();
            // entidades dos produtos
            var produtosEntity = _context.Produtos.Where(p => productsIds.Contains(p.IdProduto)).ToDictionary(p => p.IdProduto);

            // para cada produto do carrinho
            foreach (var pi in productsInfo)
            {
                // criar uma relação entre venda-produto
                // não há referência a IVAs, devia?
                vendaProdutos.Add(new VendaProduto() { VendaId = 0, ProdutoId = pi.Id, Preco = pi.FinalPrice, Quantidade = pi.Quantity });

                // atualizar o stock de cada produto
                produtosEntity[pi.Id].Stock -= pi.Quantity;
            }

            // criar o objeto da venda
            await _context.Venda.Include(v => v.VendaProdutos).LoadAsync();
            Vendum venda = new Vendum
            {
                IdVenda = 0,
                Data = DateTime.Now,
                Total = calculateTotal(productsInfo),
                UtilizadorId = userid,
                StandId = ssc.StandId,
                VendaProdutos = vendaProdutos
            };

            // atualizar tudo devidamente
            _context.Add(venda);
            _context.Produtos.UpdateRange(produtosEntity.Values);
            await _context.SaveChangesAsync();

            HttpContext.Session.Remove("CartObject"); 
            return RedirectToAction("index", "home");

        }
    }
}
