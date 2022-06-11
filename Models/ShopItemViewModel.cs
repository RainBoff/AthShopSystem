using System.Collections.Generic;

namespace asp_mvc_std_v2.Models
{
    public class ShopItemViewModel
    {
        public List<ShopViewModel> Lista;

        public ShopItemViewModel()
        { Lista = new List<ShopViewModel>(); }
        public void GetData() { }

        public void TestData()
        {
            var test = new ShopViewModel(1, "Goleszowska 17", "10:00 do 18:00", "+48 000 000 001", "testowy1.mail.pl");
            Lista.Add(test);
            test = new ShopViewModel(2, "Testowa 21", "08:00 do 16:00", "+48 000 000 002", "innytestowy2.mail.pl");
            Lista.Add(test);
            test = new ShopViewModel(3, "Lokalizacjowa 8", "06:00 do 20:00", "+48 000 000 111", "mail.mail.pl");
            Lista.Add(test);
        }

    }
}