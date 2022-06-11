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
    public class ProduktWSklepieController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProduktWSklepieController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MainAdmin/ProduktWSklepie
        public async Task<IActionResult> Index(int id)
        {
            return View(await _context.ProduktWSklepie.Where(x=>x.SklepId==id).ToListAsync());
        }

        // GET: MainAdmin/ProduktWSklepie/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produktWSklepie = await _context.ProduktWSklepie
                .FirstOrDefaultAsync(m => m.Id == id);
            if (produktWSklepie == null)
            {
                return NotFound();
            }

            return View(produktWSklepie);
        }

        // GET: MainAdmin/ProduktWSklepie/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MainAdmin/ProduktWSklepie/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProductId,SklepId,CenaWSklepie")] ProduktWSklepie produktWSklepie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(produktWSklepie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(produktWSklepie);
        }

        // GET: MainAdmin/ProduktWSklepie/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produktWSklepie = await _context.ProduktWSklepie.FindAsync(id);
            if (produktWSklepie == null)
            {
                return NotFound();
            }
            return View(produktWSklepie);
        }

        // POST: MainAdmin/ProduktWSklepie/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductId,SklepId,CenaWSklepie")] ProduktWSklepie produktWSklepie)
        {
            if (id != produktWSklepie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(produktWSklepie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProduktWSklepieExists(produktWSklepie.Id))
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
            return View(produktWSklepie);
        }

        // GET: MainAdmin/ProduktWSklepie/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produktWSklepie = await _context.ProduktWSklepie
                .FirstOrDefaultAsync(m => m.Id == id);
            if (produktWSklepie == null)
            {
                return NotFound();
            }

            return View(produktWSklepie);
        }

        // POST: MainAdmin/ProduktWSklepie/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var produktWSklepie = await _context.ProduktWSklepie.FindAsync(id);
            _context.ProduktWSklepie.Remove(produktWSklepie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProduktWSklepieExists(int id)
        {
            return _context.ProduktWSklepie.Any(e => e.Id == id);
        }
    }
}
