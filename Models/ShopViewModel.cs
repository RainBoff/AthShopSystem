using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp_mvc_std_v2.Models
{
    public class ShopViewModel
    {
        public int id;
        public string Lokalizacja;
        public string GodzinyOtwarcia;
        public string Tel;
        public string Mail;

        public ShopViewModel(int id, string lok, string godz, string tel, string mail)
        {
            this.id = id;
            this.Lokalizacja = lok;
            this.GodzinyOtwarcia = godz;
            this.Tel = tel;
            this.Mail = mail;
        }

    }
}
