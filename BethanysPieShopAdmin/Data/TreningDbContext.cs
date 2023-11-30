using BethanysPieShopAdmin.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BethanysPieShopAdmin.Data
{
    public class TreningDbContext : DbContext
    {
        public TreningDbContext(DbContextOptions<TreningDbContext> options) : base(options)
        {

        }
        public DbSet<Trening> Trening { get; set; }
        public DbSet<Vezba> Vezba { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(Trening).Assembly);

            modelBuilder.Entity<Trening>().ToTable("Treninzi");
            modelBuilder.Entity<Vezba>().ToTable("Vezbe");

            modelBuilder.Entity<Trening>()
           .Property(b => b.Name)
           .IsRequired();
        }
    }
}
