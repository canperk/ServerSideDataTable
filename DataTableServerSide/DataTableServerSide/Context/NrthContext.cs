using Microsoft.EntityFrameworkCore;
using DataTableServerSide.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataTableServerSide.Context
{
    public class NrthContext : DbContext
    {
        public NrthContext() : base(new DbContextOptions<NrthContext>())
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=Northwind;Trusted_Connection=True");
        }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Supplier> Suppliers { get; set; }
    }
}
