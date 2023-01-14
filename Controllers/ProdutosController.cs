using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
        private string getFuncFuncao()
        {
            return HttpContext.Session.GetString("Funcao");
        }
        private int getUserType()
        {
            return (int)HttpContext.Session.GetInt32("isFuncionario");
        }

        private Boolean userHasSession()
        {
            return (HttpContext.Session.GetInt32("utilizadorId") != null);
        }

        private int VerifyAdmin()
        {
            if (HttpContext.Session.GetInt32("utilizadorId") == null || getFuncFuncao() != "Admin")
                return 0;
            else
                return 1;
        }

        public IActionResult RedirectIndex(int feiraId, int id)
        {
            if (getUserType() == 0)
            {
                return RedirectToAction("produtosByStand", "produtos", new { feiraId, id });
            }
            else if (getFuncFuncao() == "Admin")
            {
                return RedirectToAction("prodByStandAdmin", "produtos", new { id });
            }
            else
            {
                return RedirectToAction("prodByStandDefault", "produtos", new { id });
            }
        }


        // GET: Produtos
        public async Task<IActionResult> Index()
        {
            if (VerifyAdmin() == 0)
            {
                return RedirectToAction("index", "home");
            }
            else
            {
                var webFayreContext = _context.Produtos.Include(p => p.Stand);
                return View(await webFayreContext.ToListAsync());
            }
        }


        public async Task<IActionResult> ProdByStandAdmin(int id)
        {
            if (VerifyAdmin() == 0)
            {
                return RedirectToAction("index", "home");
            }
            else
            {
                var prodList = _context.Produtos.Include(s => s.Stand).Where(s => s.StandId == id);
                var stand = await _context.Stands.Where(p => p.IdStand == id).FirstOrDefaultAsync();
                if (stand != null)
                {
                    ViewBag.NomeStand = stand.Nome;
                    ViewBag.StandId = stand.IdStand;
                }
                return View(await prodList.ToListAsync());
            }
        }

        public async Task<IActionResult> ProdByStandDefault(int id)
        {
            if (getUserType() != 1)
            {
                return RedirectToAction("index", "home");
            }
            else
            {
                var prodList = _context.Produtos.Include(s => s.Stand).Where(s => s.StandId == id).Where(s => s.Stock > 0);
                var stand = await _context.Stands.Where(p => p.IdStand == id).FirstOrDefaultAsync();
                if (stand != null)
                    ViewBag.NomeStand = stand.Nome;
                return View(await prodList.ToListAsync());
            }
        }

        public async Task<IActionResult> StaffIndex(int id)
        {
            if (!userHasSession())
            {
                return RedirectToAction("login", "home");
            }
            var users = _context.Utilizadors.Where(p => p.Id == getUserId()).ToList().Select(p => p.Email);
            var staff = _context.Standstaffs.Where(p => p.IdStand == id).ToList().Select(p => p.StaffEmail);
            if (staff.Contains(users.First()))
            {
                var prodList = _context.Produtos.Include(s => s.Stand).Where(s => s.StandId == id);
                var stand = await _context.Stands.Where(p => p.IdStand == id).FirstOrDefaultAsync();
                if (stand != null)
                {
                    ViewBag.NomeStand = stand.Nome;
                    ViewBag.IdStand = id;

                    if (stand.Disponibilidade == 1)
                    {
                        ViewBag.Disponibilidade = "Aberto";
                    }
                    else
                    {
                        ViewBag.Disponibilidade = "Fechado";
                    }
                }


                return View(await prodList.ToListAsync());
            }
            return RedirectToAction("index", "home");
        }

        public async Task<IActionResult> ChangeDisp(int id)
        {
            var stand = await _context.Stands.Where(s => s.IdStand == id).FirstOrDefaultAsync();
            if (stand.Disponibilidade == 0)
            {
                stand.Disponibilidade = 1;
            }
            else
            {
                stand.Disponibilidade = 0;
            }
            _context.Update(stand);
            await _context.SaveChangesAsync();
            return RedirectToAction("staffIndex", "Produtos", new { feiraId = id, id });
        }

        public async Task<IActionResult> ProdutosByStand(int feiraId, int id)
        {
            if (HttpContext.Session.GetInt32("utilizadorId") == null)
            {
                return RedirectToAction("index", "home");
            }
            else
            {
                if (getUserType() != 0)
                {
                    return RedirectToAction("index", "home");
                }
                else
                {
                    StandShoppingCart ssc = new StandShoppingCart();
                    ssc.StandId = id;
                    ssc.FeiraId = feiraId;
                    ssc.Products = new List<ProductInfo>();
                    ViewBag.StandShoppingCart = ssc;
                    //if (HttpContext.Session.GetObject<StandShoppingCart>("StandShoppingCart") != null)
                    //    ViewBag.StandShoppingCart = HttpContext.Session.GetObject<StandShoppingCart>("StandShoppingCart");

                    var prodList = _context.Produtos.Include(s => s.Stand).Where(s => s.StandId == id).Where(s => s.Stock > 0);
                    var stand = await _context.Stands.Where(p => p.IdStand == id).FirstOrDefaultAsync();
                    if (stand != null)
                    {
                        ViewBag.NomeStand = stand.Nome;
                        ViewBag.IdStand = id;

                        if (stand.Disponibilidade == 1)
                        {
                            ViewBag.Disponibilidade = "Aberto";
                        }
                        else
                        {
                            ViewBag.Disponibilidade = "Fechado";
                        }
                    }
                    return View(await prodList.ToListAsync());
                }
            }
        }


        // GET: Produtos/Details/5
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


        public async Task<IActionResult> redirectBack(int id)
        {
            if (getUserType() == 1)
            {
                if (getFuncFuncao() == "Admin")
                {
                    return RedirectToAction("ProdByStandAdmin", "produtos", new { id });
                }
                else
                {
                    return RedirectToAction("ProdByStandDefault", "produtos", new { id });
                }
            }
            else
            {
                var users = _context.Utilizadors.Where(p => p.Id == getUserId()).FirstOrDefault();
                var staff = _context.Standstaffs.Where(p => p.IdStand == id).ToList().Select(p => p.StaffEmail);
                if (staff.Contains(users.Email))
                {
                    return RedirectToAction("StaffIndex", "produtos", new { id });
                }
                else
                {
                    return RedirectToAction("ProdutosByStand", "produtos", new { id });
                }
            }
        }


        // GET: Produtos/Create
        public IActionResult Create(int id)
        {
            if (HttpContext.Session.GetInt32("utilizadorId") == null)
            {
                return RedirectToAction("login", "home");
            }
            else
            {

                var users = _context.Utilizadors.Where(p => p.Id == getUserId()).ToList().Select(p => p.Email);
                var staff = _context.Standstaffs.Where(p => p.IdStand == id).ToList().Select(p => p.StaffEmail);
                if (staff.Contains(users.First()) || VerifyAdmin() != 0)
                {
                    if (id == 0)
                    {
                        ViewData["StandId"] = new SelectList(_context.Stands, "IdStand", "Nome");
                    }
                    else
                    {
                        var stands = _context.Stands.Where(p => p.IdStand == id);
                        ViewData["StandId"] = new SelectList(stands, "IdStand", "Nome");
                    }
                    return View();
                }
                else
                {
                    return RedirectToAction("index", "home");
                }
            }
        }

        // POST: Produtos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdProduto,Stock,Name,Descricao,Preco,Iva,ImagemPath,StandId")] Produto produto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(produto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StandId"] = new SelectList(_context.Stands, "IdStand", "Nome", produto.StandId);
            if (getFuncFuncao() == "Admin")
            {
                return View(produto);
            }
            return RedirectToAction("standstaff", "produtos");
        }

        // GET: Produtos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (VerifyAdmin() == 0)
            {
                return RedirectToAction("index", "home");
            }
            else
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
        }

        // POST: Produtos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdProduto,Stock,Name,Descricao,Preco,Iva,ImagemPath,StandId")] Produto produto)
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

        // GET: Produtos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (VerifyAdmin() == 0)
            {
                return RedirectToAction("index", "home");
            }
            else
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
        }

        // POST: Produtos/Delete/5
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
            if (products == null || products.Count == 0)
            {
                return null;
            }
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
            if (getUserType() != 0)
            {
                return RedirectToAction("index", "home");
            }
            else
            {

                StandShoppingCart? ssc = HttpContext.Session.GetObject<StandShoppingCart>("CartObject");
                if (ssc == null || ssc.Products.Count == 0)
                {
                    return NoContent();
                }
                decimal total = calculateTotal(ssc.Products);
                ViewBag.Total = total;
                return View(ssc);
            }
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
            if (getUserType() != 0)
            {
                return RedirectToAction("index", "home");
            }
            else
            {
                int id = getUserId();
                var user = _context.Utilizadors.FirstOrDefault(x => x.Id == id);
                ViewBag.Nif = user.Nif;
                ViewBag.Telemovel = user.Telemovel;
                return View();
            }
        }

        public async Task<IActionResult> FinalizePurchase(string nif, string tel)
        {
            if (HttpContext.Session.GetInt32("utilizadorId") == null)
            {
                RedirectToAction("login", "home");
            }
            if (getUserType() != 0)
            {
                return RedirectToAction("index", "home");
            }
            else
            {
                int userid = (int)HttpContext.Session.GetInt32("utilizadorId");

                //@todo -atualizar stock; registar a compra; redirect correto para lista de produtos
                Console.WriteLine(nif);
                Console.WriteLine(tel);

                // se for finalizada..
                StandShoppingCart? ssc = HttpContext.Session.GetObject<StandShoppingCart>("CartObject");

                // aqui devia ser redirecionado para o sitio devido
                if (ssc == null) return NoContent();

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
                    VendaProdutos = vendaProdutos,
                    Nif = nif,
                    Telemovel = tel
                };

                // atualizar tudo devidamente
                _context.Add(venda);
                _context.Produtos.UpdateRange(produtosEntity.Values);
                await _context.SaveChangesAsync();

                HttpContext.Session.Remove("CartObject");
                return RedirectToAction("index", "home");
            }

        }
        [HttpGet]
        public IActionResult Restock(int? id, int standId)
        {
            if (getUserType() != 0)
            {
                return RedirectToAction("index", "home");
            }
            var users = _context.Utilizadors.Where(p => p.Id == getUserId()).ToList().Select(p => p.Email);
            var staff = _context.Standstaffs.Where(p => p.IdStand == standId).ToList().Select(p => p.StaffEmail);
            foreach (var email in users)
            {
                if (staff.Contains(email))
                {
                    var prod = _context.Produtos.Where(p => p.IdProduto == id).FirstOrDefault();
                    ViewBag.Stock = prod.Stock;
                    return View(prod);
                }
            }
            return RedirectToAction("index", "home");
        }

        [HttpPost]
        public async Task<IActionResult> Restock(int stock, int standId, int id)
        {
            var prod = await _context.Produtos.Where(p => p.IdProduto == id).FirstOrDefaultAsync();
            if (stock >= 0)
            {
                prod.Stock = stock;
                _context.Update(prod);
                await _context.SaveChangesAsync();
            }
            else
            {
                ViewBag.Message = "Invalid number";
                return View(prod);
            }

            return RedirectToAction("StaffIndex", "Produtos", new { id = standId });
        }
    }
}
