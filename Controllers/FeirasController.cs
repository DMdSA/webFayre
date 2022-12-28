using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using WebFayre.Models;

namespace WebFayre.Controllers
{
    public class FeirasController : Controller
    {
        private readonly WebFayreContext _context;

        public FeirasController(WebFayreContext context)
        {
            _context = context;
        }

        // GET: Feiras
        public async Task<IActionResult> Index()
        {
            return _context.Feiras != null ?
                    View(await _context.Feiras.ToListAsync()) :
                    Problem("Entity set 'WebFayreContext.Feiras'  is null.");
        }

        // GET: Feiras/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Feiras == null)
            {
                return NotFound();
            }

            //var feira = await _context.Feiras.Include(x => x.FeiraCategoria1s)
            //    .ThenInclude(y => y.IdCategoriaFeira)  //Devia ser o nome da table
            //    .SingleOrDefaultAsync(m => m.IdFeira == id);

            var feira = await _context.Feiras
                .SingleOrDefaultAsync(m => m.IdFeira == id);
            if (feira == null)
            {
                return NotFound();
            }

            

            return View(feira);
        }

        // GET: Feiras/Create
        public IActionResult Create()
        {

            ViewData["Categorias"] = new MultiSelectList(_context.Categoriafeiras, "IdCategoriaFeira", "Descricao");
            ViewData["Patrocinadores"] = new MultiSelectList(_context.Patrocinadors, "IdPatrocinador", "Nome");
            return View();
        }

        // POST: Feiras/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdFeira,Descricao,Nome,DataInicio,DataFim,CapacidadeClientes,NStands,Email,Telefone,Morada,FeiraPath,FeiraCategoria1s,Patrocinadors")] Feira feira)
        {
            await _context.Feiras.Include(x => x.FeiraCategoria1s).Include(x => x.Patrocinadors).LoadAsync();

            if (ModelState.IsValid)
            {
                var category_values = ModelState.Values.ToList()[9];
                var category_ids = category_values.AttemptedValue.Split(",");
                var category_idsList = category_ids.Select(int.Parse).ToList();

                var patroc_values = ModelState.Values.ToList()[10];
                var patroc_ids = patroc_values.AttemptedValue.Split(",");
                var patroc_idsList = patroc_ids.Select(int.Parse).ToList();

                var category_entities = _context.Categoriafeiras.Where(x => category_idsList.Contains(x.IdCategoriaFeira));
                var patroc_entities = _context.Patrocinadors.Where(x => patroc_idsList.Contains(x.IdPatrocinador));
                feira.FeiraCategoria1s.AddRange(category_entities);
                feira.Patrocinadors.AddRange(patroc_entities);

                _context.Add(feira).Collection(c => c.FeiraCategoria1s);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }


            ViewData["Categorias"] = new MultiSelectList(_context.Categoriafeiras, "IdCategoriaFeira", "Descricao", feira.FeiraCategoria1s);
            ViewData["Patrocinadores"] = new MultiSelectList(_context.Patrocinadors, "IdPatrocinador", "Nome", feira.Patrocinadors);


            return View(feira);
        }

        // GET: Feiras/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Feiras == null)
            {
                return NotFound();
            }

            var feira = await _context.Feiras.FindAsync(id);
            if (feira == null)
            {
                return NotFound();
            }
            ViewData["Categorias"] = new MultiSelectList(_context.Categoriafeiras, "IdCategoriaFeira", "Descricao", feira.FeiraCategoria1s);
            ViewData["Patrocinadores"] = new MultiSelectList(_context.Patrocinadors, "IdPatrocinador", "Nome", feira.Patrocinadors);
            return View(feira);
        }

        // POST: Feiras/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdFeira,Descricao,Nome,DataInicio,DataFim,CapacidadeClientes,NStands,Email,Telefone,Morada,FeiraPath,FeiraCategoria1s,Patrocinadors")] Feira feira)
        {
            if (id != feira.IdFeira)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var category_values = ModelState.Values.ToList()[12];
                    var category_ids = category_values.AttemptedValue.Split(",");
                    var category_idsList = category_ids.Select(int.Parse).ToList();

                    var patroc_values = ModelState.Values.ToList()[11];
                    var patroc_ids = patroc_values.AttemptedValue.Split(",");
                    var patroc_idsList = patroc_ids.Select(int.Parse).ToList();

                    foreach (var category in feira.FeiraCategoria1s.ToList())
                    {
                        feira.FeiraCategoria1s.Remove(category);
                    }
                    foreach (var patrocinador in feira.Patrocinadors.ToList())
                    {
                        feira.Patrocinadors.Remove(patrocinador);
                    }

                    var category_entities = _context.Categoriafeiras.Where(x => category_idsList.Contains(x.IdCategoriaFeira));
                    var patroc_entities = _context.Patrocinadors.Where(x => patroc_idsList.Contains(x.IdPatrocinador));
                    feira.FeiraCategoria1s.AddRange(category_entities);
                    feira.Patrocinadors.AddRange(patroc_entities);
                    _context.Update(feira);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FeiraExists(feira.IdFeira))
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
            ViewData["FeiraCategoria1s"] = new MultiSelectList(_context.Categoriafeiras, "IdCategoriaFeira", "IdCategoriaFeira", feira.FeiraCategoria1s);
            ViewData["Patrocinadores"] = new MultiSelectList(_context.Patrocinadors, "IdPatrocinador", "Nome", feira.Patrocinadors);
            return View(feira);
        }

        // GET: Feiras/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Feiras == null)
            {
                return NotFound();
            }

            var feira = await _context.Feiras.Include(x => x.Stands).Include(x => x.Tickets).Include(x => x.FeiraCategoria1s)
                .FirstOrDefaultAsync(m => m.IdFeira == id);
            if (feira == null)
            {
                return NotFound();
            }

            return View(feira);
        }

        // POST: Feiras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Feiras == null)
            {
                return Problem("Entity set 'WebFayreContext.Feiras'  is null.");
            }
            var feira = await _context.Feiras.Include(f => f.FeiraCategoria1s).FirstOrDefaultAsync(f => f.IdFeira == id);
            if (feira != null)
            {
                foreach (var category in feira.FeiraCategoria1s.ToList())
                {
                    feira.FeiraCategoria1s.Remove(category);
                }
                StandsController standsController = new StandsController(_context);
                foreach (var stand in feira.Stands.ToList())
                {
                    await standsController.DeleteConfirmed(stand.IdStand, stand.FeiraId);
                }
                TicketsController tc = new TicketsController(_context);
                foreach (var ticket in feira.Tickets.ToList())
                {
                    await tc.Delete(ticket.Id);
                }
                _context.Feiras.Remove(feira);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FeiraExists(int id)
        {
            return (_context.Feiras?.Any(e => e.IdFeira == id)).GetValueOrDefault();
        }

        public async Task<IActionResult> Enter(int id)
        {
            // se não houver nenhuma session ativa, redirecionar para o login
            if (HttpContext.Session.GetInt32("utilizadorId") == null)
            {
                return RedirectToAction("Login", "Home");
            }

            // get current user id
            var userid = HttpContext.Session.GetInt32("utilizadorId");

            if (FeiraExists(id))
            {

                bool ticket_exists = _context.Tickets.Any(t => t.UtilizadorId == userid && t.FeiraId == id);

                if (!ticket_exists)
                {
                    // criar um ticket manualmente com valores default
                    Ticket t = new Ticket() { Id = 0, Data = DateTime.Now, UtilizadorId = (int)userid, FeiraId = id };
                    // utilizar o controller dos tickets para o adicionar corretamente à db
                    TicketsController tc = new TicketsController(_context);
                    await tc.Create(t);
                    TempData["feiraticket"] = "Ticket gerado com sucesso!";
                    return RedirectToAction("standsByFeira", "stands", new {id});
                }

                TempData["feiraticket"] = "Já tinhas um ticket! podes entrar! :)";
                // change this to feira's stands
                return RedirectToAction("standsByFeira", "stands", new {id});
            }

            return NotFound();
        }


    }
}
