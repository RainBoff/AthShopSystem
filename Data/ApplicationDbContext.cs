using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Data_Model_P;
using asp_mvc_std_v2.Models;

namespace asp_mvc_std_v2.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Data_Model_P.Produkt> Produkt { get; set; }
        public DbSet<Data_Model_P.Sklep> Sklep { get; set; }
        public DbSet<Data_Model_P.ProduktWSklepie> ProduktWSklepie { get; set; }
        public DbSet<Data_Model_P.Zamowienie> Zamowienie { get; set; }
        public DbSet<Data_Model_P.SzczegolyZamowienia> SzczegolyZamowienia { get; set; }
        public DbSet<asp_mvc_std_v2.Models.UsersAndShops> UsersAndShops { get; set; }
    }
}
