using BethanysPieShopAdmin.Data.Models;

namespace BethanysPieShopAdmin.Data
{
    public static class DbInitializerTrening
    {
        public static void Seed(TreningDbContext context)
        {
            context.Database.EnsureCreated();

            if (!context.Trening.Any())
            {
                context.Trening.AddRange(
                    new Trening { Name = "Trening leđa", Date = DateTime.Now.Date});

            }

            context.SaveChanges();


        }
    }
}
