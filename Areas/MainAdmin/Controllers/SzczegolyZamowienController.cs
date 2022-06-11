using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Data_Model_P;
using asp_mvc_std_v2.Data;

namespace asp_mvc_std_v2.Areas.MainAdmin.Controllers
{
    [Area("MainAdmin")]
    public class SzczegolyZamowienController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SzczegolyZamowienController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MainAdmin/SzczegolyZamowien
        public async Task<IActionResult> Index()
        {
            return View(await _context.SzczegolyZamowienia.ToListAsync());
        }

        // GET: MainAdmin/SzczegolyZamowien/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var szczegolyZamowienia = await _context.SzczegolyZamowienia
                .FirstOrDefaultAsync(m => m.Id == id);
            if (szczegolyZamowienia == null)
            {
                return NotFound();
            }

            return View(szczegolyZamowienia);
        }

        // GET: MainAdmin/SzczegolyZamowien/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MainAdmin/SzczegolyZamowien/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdZamowienia,IdProduktu,Ilosc,Cena")] SzczegolyZamowienia szczegolyZamowienia)
        {
            if (ModelState.IsValid)
            {
                _context.Add(szczegolyZamowienia);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(szczegolyZamowienia);
        }

        // GET: MainAdmin/SzczegolyZamowien/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var szczegolyZamowienia = await _context.SzczegolyZamowienia.FindAsync(id);
            if (szczegolyZamowienia == null)
            {
                return NotFound();
            }
            return View(szczegolyZamowienia);
        }

        // POST: MainAdmin/SzczegolyZamowien/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdZamowienia,IdProduktu,Ilosc,Cena")] SzczegolyZamowienia szczegolyZamowienia)
        {
            if (id != szczegolyZamowienia.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(szczegolyZamowienia);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SzczegolyZamowieniaExists(szczegolyZamowienia.Id))
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
            return View(szczegolyZamowienia);
        }

        // GET: MainAdmin/SzczegolyZamowien/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var szczegolyZamowienia = await _context.SzczegolyZamowienia
                .FirstOrDefaultAsync(m => m.Id == id);
            if (szczegolyZamowienia == null)
            {
                return NotFound();
            }

            return View(szczegolyZamowienia);
        }

        // POST: MainAdmin/SzczegolyZamowien/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var szczegolyZamowienia = await _context.SzczegolyZamowienia.FindAsync(id);
            _context.SzczegolyZamowienia.Remove(szczegolyZamowienia);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SzczegolyZamowieniaExists(int id)
        {
            return _context.SzczegolyZamowienia.Any(e => e.Id == id);
        }
    }
}
