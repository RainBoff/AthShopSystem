using Data_Model_P;
using Data_Model_P.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp_mvc_std_v2.Areas.MainAdminArea.Models
{
    public class ZamowieniaViewModel
    {
        public int Id { get; set; }
        public int IdSklepu { get; set; }
        public DateTime DataUtworzenia { get; set; }
        public Status Status { get; set; }
        public List<SzczegolyZamowienia> szczegolyZamowienia { get; set; }
        public List<ProduktWSklepie> produkty { get; set; }

        public int ProductId;
        public int Ilosc;
        public double Wartosc;
        public string ProductName;
        public ZamowieniaViewModel()
        {
            szczegolyZamowienia = new List<SzczegolyZamowienia>();
            produkty = new List<ProduktWSklepie>();
        }
    }
}
