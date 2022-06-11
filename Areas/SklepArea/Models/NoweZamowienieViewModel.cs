using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data_Model_P;

namespace asp_mvc_std_v2.Areas.SklepArea.Models
{
    public class NoweZamowienieViewModel
    {
        public List<SzczegolyZamowienia> ProduktList;
        public Zamowienie zamowienie;
        public List<Produkt> AllproduktList;
        public NoweZamowienieViewModel() 
        {
            ProduktList = new List<SzczegolyZamowienia>();
        }
        
    }
}
