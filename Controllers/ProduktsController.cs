using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Data_Model_P;
using asp_mvc_std_v2.Data;
using AutoMapper;
using asp_mvc_std_v2.Models;

namespace asp_mvc_std_v2.Controllers
{
    public class ProduktsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ProduktsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: Produkts
        public async Task<IActionResult> Index()
        {
            var produkty = await _context.Produkt.ToListAsync();
            var produkty_mapowane = _mapper.Map<List<ProductViewModel>>(produkty);


            return View(produkty_mapowane);
        }

        // GET: Produkts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produkt = await _context.Produkt.FirstOrDefaultAsync(m => m.Id == id);
            var produkt_mapowany = _mapper.Map<ProductViewModel>(produkt);

            if (produkt_mapowany == null)
            {
                return NotFound();
            }

            return View(produkt_mapowany);
        }

        // GET: Produkts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Produkts/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nazwa,Opis,Cena")] ProductViewModel produkt_mapowany)
        {


            var odmapuj = new MapperConfiguration(cfg => { cfg.CreateMap<Produkt, ProductViewModel>().ReverseMap(); });
            var produtk_odmapowany = odmapuj.CreateMapper().Map<Produkt>(produkt_mapowany);

            if (ModelState.IsValid)
            {
                _context.Add(produtk_odmapowany);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(produkt_mapowany);
        }

        // GET: Produkts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produkt = await _context.Produkt.FindAsync(id);
            var produkt_mapowany = _mapper.Map<ProductViewModel>(produkt);

            if (produkt == null)
            {
                return NotFound();
            }
            return View(produkt_mapowany);
        }

        // POST: Produkts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nazwa,Opis,Cena")] Produkt produkt)
        {
            
            var produkt_mapowany = _mapper.Map<ProductViewModel>(produkt);
            if (id != produkt.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    _context.Update(produkt);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProduktExists(produkt.Id))
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
            return View(produkt_mapowany);
        }

        // GET: Produkts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produkt = await _context.Produkt
                .FirstOrDefaultAsync(m => m.Id == id);
            if (produkt == null)
            {
                return NotFound();
            }

            return View(produkt);
        }

        // POST: Produkts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var produkt = await _context.Produkt.FindAsync(id);
            _context.Produkt.Remove(produkt);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProduktExists(int id)
        {
            return _context.Produkt.Any(e => e.Id == id);
        }
    }
}