using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Data_Model_P;
using asp_mvc_std_v2.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using asp_mvc_std_v2.Areas.SklepArea.Models;

namespace asp_mvc_std_v2.Areas.SklepArea.Controllers
{
    [Area("SklepArea")]
    public class ShopStaffController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _context;

        public ShopStaffController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        private bool ZamowienieExists(int id)
        {
            return _context.Zamowienie.Any(e => e.Id == id);
        }

        public int GetUserShopId(string userId) 
        {
          return  _context.UsersAndShops.Where(x => x.UserId == userId).FirstOrDefault().ShopId;
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var UserId = GetUserShopId(_context.Users.Where(x => x.UserName == this.User.Identity.Name).FirstOrDefault().Id);
            if (UserId == null)
            {
                var Shop = new Sklep(); Shop.Id = 0;
                return View(Shop);
            }
            else
            {
                var Shop = _context.Sklep.Where(z => z.Id == UserId).FirstOrDefault();
                return View(Shop);
            }
               
        }

        public async Task<IActionResult> Produkty(int? id)
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

            foreach (var produkt in ProduktyWS.Where(x => x.SklepId == id))
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

            return View(Sklep_Produkty);
        }


        public async Task<IActionResult> Produkty_Add(int? id)
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
        public async Task<IActionResult> Produkty_Add(int sklepId, int produktId)
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
            var ProduktyWS = _context.ProduktWSklepie.Where(x => x.SklepId == sklepId);
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
        public async Task<IActionResult> Produkty_Remove(int sklepId, int produktId)
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

            return View("Produkty", Sklep_Produkty);
        }

        public async Task<IActionResult> Zamowienia(int Id)
        {

            ZamowieniaIndexViewModel zam = new ZamowieniaIndexViewModel();
            zam.zamowienia = await _context.Zamowienie.Where(x => x.IdSklepu == Id).ToListAsync();
            zam.IdSklepu = Id;
            return View(zam);
        }

        public async Task<IActionResult> Zamowienia_Edit(int Id)
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

        public async Task<IActionResult> Zamowienia_Details(int Id, int IdSklepu)
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
            foreach (var prod in zamowienieVM.szczegolyZamowienia)
            {
                zamowienieVM.Wartosc = zamowienieVM.Wartosc + (prod.Cena * prod.Ilosc);
            }

            if (zamowienie == null)
            {
                return NotFound();
            }
            return View(zamowienieVM);
        }

        public async Task<IActionResult> Zamowienia_Create(int Id)
        {
            var zamowieniaViewModel = new ZamowieniaViewModel();
            zamowieniaViewModel.IdSklepu = Id;


            return View(zamowieniaViewModel);
        }

        public async Task<IActionResult> Zamowienia_Edit_Zam(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var zamowienie = await _context.Zamowienie.FindAsync(id);
            if (zamowienie == null)
            {
                return NotFound();
            }
            return View(zamowienie);
        }

       
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Zamowienia_Edit_ZamM(int id, [Bind("Id,IdSklepu,DataUtworzenia,Status")] Zamowienie zamowienie)
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
        public async Task<IActionResult> ZamowieniaSZ_Remove(int Id, int IdSklepu)
        {
            var szczegolyZamowienia = await _context.SzczegolyZamowienia.FindAsync(Id);
            _context.SzczegolyZamowienia.Remove(szczegolyZamowienia);
            await _context.SaveChangesAsync();
            ZamowieniaIndexViewModel zam = new ZamowieniaIndexViewModel();
            zam.zamowienia = await _context.Zamowienie.Where(x => x.IdSklepu == IdSklepu).ToListAsync();
            zam.IdSklepu = IdSklepu;

            return RedirectToAction("Zamowienia", new { Id = IdSklepu });

        }

        [HttpPost]
        public async Task<IActionResult> Zamowienia_Create([Bind("Id,IdSklepu,DataUtworzenia,Status")] ZamowieniaViewModel zamowieniaViewModel)
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

            return View("Zamowienia", zam);
        }

        [HttpPost]
        public async Task<IActionResult> Zamowienia_Edit(int id, [Bind("Id,IdSklepu,DataUtworzenia,Status")] Zamowienie zamowienie)
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


            return RedirectToAction("Zamowienia",new { Id = IdSklepu });

        }

        /////
    }
}