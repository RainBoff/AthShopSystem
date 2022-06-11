using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data_Model_P;

namespace asp_mvc_std_v2.Areas.MainAdmin.Models
{
    public class Sklep_ProduktyViewModel
    {
        public int Id { get; set; }
        public string Nazwa { get; set; }
        public string Opis { get; set; }

        public List<ProduktWSklepie> produktList;

        public List<Produkt> AllproduktList;


        public Sklep_ProduktyViewModel() 
        {
            produktList = new List<ProduktWSklepie>();
            AllproduktList = new List<Produkt>();
        }
    }
}
