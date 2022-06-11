using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Data_Model_P;
using asp_mvc_std_v2.Data;
using asp_mvc_std_v2.Areas.MainAdminArea.Models;

namespace asp_mvc_std_v2.Areas.MainAdminArea.Controllers
{
    [Area("MainAdminArea")]
    public class SklepController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SklepController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MainAdmin/Sklep
        public async Task<IActionResult> Index()
        {
            return View(await _context.Sklep.ToListAsync());
        }
        
        // GET: MainAdmin/Sklep/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sklep = await _context.Sklep
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sklep == null)
            {
                return NotFound();
            }

            return View(sklep);
        }

        // GET: MainAdmin/Sklep/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MainAdmin/Sklep/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nazwa,Opis")] Sklep sklep)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sklep);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sklep);
        }

        // GET: MainAdmin/Sklep/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sklep = await _context.Sklep.FindAsync(id);
            if (sklep == null)
            {
                return NotFound();
            }
            return View(sklep);
        }

        // POST: MainAdmin/Sklep/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nazwa,Opis")] Sklep sklep)
        {
            if (id != sklep.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sklep);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SklepExists(sklep.Id))
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
            return View(sklep);
        }

        // GET: MainAdmin/Sklep/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sklep = await _context.Sklep
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sklep == null)
            {
                return NotFound();
            }

            return View(sklep);
        }

        // POST: MainAdmin/Sklep/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sklep = await _context.Sklep.FindAsync(id);
            _context.Sklep.Remove(sklep);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SklepExists(int id)
        {
            return _context.Sklep.Any(e => e.Id == id);
        }

        public async Task<IActionResult> ProduktyWSklepie(int? id) 
        {
            if (id == null)
            {
                return NotFound();
            }

            var sklep = await _context.Sklep.FirstOrDefaultAsync(m => m.Id == id);
            if (sklep == null)
            {
                return NotFound();
            }


            Sklep_ProduktyViewModel Sklep_Produkty = new Sklep_ProduktyViewModel();
            Sklep_Produkty.Id = id.Value;
            Sklep_Produkty.Nazwa = sklep.Nazwa;
            Sklep_Produkty.Opis = sklep.Opis;
            var ProduktyWS = _context.ProduktWSklepie;
            var Produkty = _context.Produkt;
            
            foreach(var produkt in ProduktyWS.Where(x => x.SklepId == id)) 
            {
                Sklep_Produkty.produktList.Add(produkt);
            }

            foreach (var produkt in Produkty)
            {
                bool zawiera = false;
                for(int i = 0; i < ProduktyWS.Count(); i++) 
                {
                if(produkt.Id == ProduktyWS.ToArray()[i].ProductId) { zawiera = true; }
                }

                if (!zawiera) { Sklep_Produkty.AllproduktList.Add(produkt); }
            }

            return View(Sklep_Produkty);
        }

        public async Task<IActionResult> ZamowieniaWSklepie_Index(int Id)
        {

            ZamowieniaIndexViewModel zam = new ZamowieniaIndexViewModel();
            zam.zamowienia = await _context.Zamowienie.Where(x => x.IdSklepu == Id).ToListAsync();
            zam.IdSklepu = Id;
                return View(zam);
           
        }

        public async Task<IActionResult> ZamowieniaWSklepie_Edit(int Id)
        {
                if (Id == null)
                {
                    return NotFound();
                }

            var zamowienie = await _context.Zamowienie.FindAsync(Id);
            ZamowieniaViewModel zamowienieVM = new ZamowieniaViewModel();
            zamowienieVM.Id = zamowienie.Id;
            zamowienieVM.IdSklepu = zamowienie.IdSklepu;
            zamowienieVM.Status = zamowienie.Status;
            zamowienieVM.DataUtworzenia = zamowienie.DataUtworzenia;

            zamowienieVM.produkty = await _context.ProduktWSklepie.ToListAsync();
            zamowienieVM.szczegolyZamowienia = await _context.SzczegolyZamowienia.Where(x => x.Id == Id).ToListAsync();

               
                if (zamowienie == null)
                {
                    return NotFound();
                }
                return View(zamowienieVM);
        }

        public async Task<IActionResult> ZamowieniaWSklepie_Details(int Id, int IdSklepu)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var zamowienie = await _context.Zamowienie.FindAsync(Id);
            ZamowieniaViewModel zamowienieVM = new ZamowieniaViewModel();
            zamowienieVM.Id = zamowienie.Id;
            zamowienieVM.IdSklepu = zamowienie.IdSklepu;
            zamowienieVM.Status = zamowienie.Status;
            zamowienieVM.DataUtworzenia = zamowienie.DataUtworzenia;

            zamowienieVM.produkty = await _context.ProduktWSklepie.ToListAsync();
            zamowienieVM.szczegolyZamowienia = await _context.SzczegolyZamowienia.Where(x => x.IdZamowienia == Id).ToListAsync();

            zamowienieVM.Wartosc = 0;
            foreach(var prod in zamowienieVM.szczegolyZamowienia) 
            {
                zamowienieVM.Wartosc = zamowienieVM.Wartosc+(prod.Cena * prod.Ilosc);
            }

            if (zamowienie == null)
            {
                return NotFound();
            }
            return View(zamowienieVM);
        }

        public async Task<IActionResult> ZamowieniaWSklepie_Create(int Id)
        {
            var zamowieniaViewModel = new ZamowieniaViewModel();
            zamowieniaViewModel.IdSklepu = Id;


            return View(zamowieniaViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ZamowieniaWSklepieSZ_Remove(int Id, int IdSklepu)
        {
            var szczegolyZamowienia = await _context.SzczegolyZamowienia.FindAsync(Id);
            _context.SzczegolyZamowienia.Remove(szczegolyZamowienia);
            await _context.SaveChangesAsync();
            ZamowieniaIndexViewModel zam = new ZamowieniaIndexViewModel();
            zam.zamowienia = await _context.Zamowienie.Where(x => x.IdSklepu == IdSklepu).ToListAsync();
            zam.IdSklepu = IdSklepu;

            return View("ZamowieniaWSklepie_Index", zam);

        }

        [HttpPost]
        public async Task<IActionResult> ZamowieniaWSklepie_Create([Bind("Id,IdSklepu,DataUtworzenia,Status")] ZamowieniaViewModel zamowieniaViewModel)
        {
            var ZamowienieToAdd = new Zamowienie();
            ZamowienieToAdd.IdSklepu = zamowieniaViewModel.IdSklepu;
            ZamowienieToAdd.Status = zamowieniaViewModel.Status;
            ZamowienieToAdd.DataUtworzenia = zamowieniaViewModel.DataUtworzenia;
          
                _context.Zamowienie.Add(ZamowienieToAdd);
                await _context.SaveChangesAsync();

            ZamowieniaIndexViewModel zam = new ZamowieniaIndexViewModel();
            zam.zamowienia = await _context.Zamowienie.Where(x => x.IdSklepu == zamowieniaViewModel.IdSklepu).ToListAsync();
            zam.IdSklepu = zamowieniaViewModel.IdSklepu;

            return View("Zamowienia_Index", zam);
        }

        [HttpPost]

        public async Task<IActionResult> ZamowieniaWSklepie_Edit(int id, [Bind("Id,IdSklepu,DataUtworzenia,Status")] Zamowienie zamowienie)
        {
            if (id != zamowienie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(zamowienie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ZamowienieExists(zamowienie.Id))
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
            return View(zamowienie);
        }

        [HttpPost]
        public async Task<IActionResult> ZamowieniaDodajProdukt(int Ilosc, int Id, int IdSklepu, int ProductId)
        {
            var szegolyZamowienia = new SzczegolyZamowienia();
            szegolyZamowienia.IdProduktu = ProductId;
            szegolyZamowienia.IdZamowienia = Id;
            szegolyZamowienia.Ilosc = Ilosc;
            szegolyZamowienia.Cena = _context.Produkt.Where(x => x.Id == ProductId).FirstOrDefault().Cena;
            _context.SzczegolyZamowienia.Add(szegolyZamowienia);
            _context.SaveChanges();


            ZamowieniaIndexViewModel zam = new ZamowieniaIndexViewModel();
            zam.zamowienia = await _context.Zamowienie.Where(x => x.IdSklepu == IdSklepu).ToListAsync();
            zam.IdSklepu = IdSklepu;

            return View("ZamowieniaWSklepie_Index",zam);
        }



        private bool ZamowienieExists(int id)
        {
            return _context.Zamowienie.Any(e => e.Id == id);
        }

        public async Task<IActionResult> ZamowieniaWSklepie_Add(int Id)
        {


            return View(await _context.Zamowienie.Where(x => x.IdSklepu == Id).ToListAsync());

        }


        public async Task<IActionResult> ProduktyWSklepie_Add(int? id) 
        {
            if (id == null)
            {
                return NotFound();
            }

            var sklep = await _context.Sklep.FirstOrDefaultAsync(m => m.Id == id);
            if (sklep == null)
            {
                return NotFound();
            }
            


            Sklep_ProduktyViewModel Sklep_Produkty = new Sklep_ProduktyViewModel();
            Sklep_Produkty.Id = id.Value;
            Sklep_Produkty.Nazwa = sklep.Nazwa;
            Sklep_Produkty.Opis = sklep.Opis;
            var ProduktyWS = _context.ProduktWSklepie.Where(x => x.SklepId == id);
            var Produkty = _context.Produkt;

           

            foreach (var produkt in Produkty)
            {
                bool zawiera = false;
                for (int i = 0; i < ProduktyWS.Count(); i++)
                {
                    if (produkt.Id == ProduktyWS.ToArray()[i].ProductId) { zawiera = true; }
                }

                if (!zawiera) { Sklep_Produkty.AllproduktList.Add(produkt); }
            }

            return View(Sklep_Produkty);
        }

        [HttpPost]
        public async Task<IActionResult> ProduktyWSklepie_Remove(int sklepId, int produktId)
        {
            var ProduktToRemove = await _context.ProduktWSklepie.Where(x => x.Id == produktId).ToListAsync();
            

            _context.ProduktWSklepie.Remove(ProduktToRemove.FirstOrDefault());
            _context.SaveChanges();

            if (sklepId == null)
            {
                return NotFound();
            }

            var sklep = await _context.Sklep.FirstOrDefaultAsync(m => m.Id == sklepId);
            if (sklep == null)
            {
                return NotFound();
            }


            Sklep_ProduktyViewModel Sklep_Produkty = new Sklep_ProduktyViewModel();
            Sklep_Produkty.Id = sklepId;
            Sklep_Produkty.Nazwa = sklep.Nazwa;
            Sklep_Produkty.Opis = sklep.Opis;
            var ProduktyWS = _context.ProduktWSklepie;
            var Produkty = _context.Produkt;

            foreach (var produkt in ProduktyWS.Where(x => x.SklepId == sklepId))
            {
                Sklep_Produkty.produktList.Add(produkt);
            }

            foreach (var produkt in Produkty)
            {
                bool zawiera = false;
                for (int i = 0; i < ProduktyWS.Count(); i++)
                {
                    if (produkt.Id == ProduktyWS.ToArray()[i].ProductId) { zawiera = true; }
                }

                if (!zawiera) { Sklep_Produkty.AllproduktList.Add(produkt); }
            }

            return View("ProduktyWSklepie", Sklep_Produkty);
        }



        public async Task<IActionResult> ZamowieniaZestawiania(int Id)
        {

            ZamowieniaIndexViewModel zam = new ZamowieniaIndexViewModel();
            zam.zamowienia = await _context.Zamowienie.Where(x => x.IdSklepu == Id).ToListAsync();
            zam.IdSklepu = Id;
            return View(zam);

        }


        [HttpPost]
        public async Task<IActionResult> ProduktyWSklepie_Add(int sklepId, int produktId)
        {
            var ProduktToAdd = _context.Produkt.Where(x => x.Id == produktId).FirstOrDefault();
            ProduktWSklepie ProduktWSToAdd = new ProduktWSklepie();
            ProduktWSToAdd.ProductId = produktId;
            ProduktWSToAdd.ProductName = ProduktToAdd.Nazwa;
            ProduktWSToAdd.SklepId = sklepId;
            ProduktWSToAdd.CenaWSklepie = ProduktToAdd.Cena;
           
            _context.ProduktWSklepie.Add(ProduktWSToAdd);
            _context.SaveChanges();

            var sklep = await _context.Sklep.FirstOrDefaultAsync(m => m.Id == sklepId);
            if (sklep == null)
            {
                return NotFound();
            }


            Sklep_ProduktyViewModel Sklep_Produkty = new Sklep_ProduktyViewModel();
            Sklep_Produkty.Id = sklepId;
            Sklep_Produkty.Nazwa = sklep.Nazwa;
            Sklep_Produkty.Opis = sklep.Opis;
            var ProduktyWS = _context.ProduktWSklepie.Where(x=>x.SklepId==sklepId);
            var Produkty = _context.Produkt;

    

            foreach (var produkt in Produkty)
            {
                bool zawiera = false;
                for (int i = 0; i < ProduktyWS.Count(); i++)
                {
                    if (produkt.Id == ProduktyWS.ToArray()[i].ProductId ) { zawiera = true; }
                }

                if (!zawiera) { Sklep_Produkty.AllproduktList.Add(produkt); }
            }

            return View(Sklep_Produkty);
        }
    }
}
