
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
namespace BethanysPieShopAdmin.Data.Models.Repositories
{
    public class TreningRepository : ITreningRepository
    {
        private readonly TreningDbContext treningDbContext;
        private IMemoryCache _MemoryCache;
        private const string AllTreningsCache = "AllTrenings";

        public TreningRepository(TreningDbContext treningDbContext, IMemoryCache memoryCache)
        {
            this.treningDbContext = treningDbContext;
            _MemoryCache = memoryCache;
        }
        public IEnumerable<Trening> GetAllTrenings()
        {
            return treningDbContext.Trening.AsNoTracking().OrderBy(p => p.Id);
        }
        public async Task<IEnumerable<Trening>> GetAllTreningAsync()
        {
            List<Trening> allTrening = null;


            allTrening = await treningDbContext.Trening.AsNoTracking().OrderBy(c => c.Id).ToListAsync();
            var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(60));




            return allTrening;
        }
        public async Task<Trening?> GetTreningByIdAsync(int id)
        {
            return await treningDbContext.Trening.AsNoTracking().Include(p => p.Vezbe).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<int> AddTreningAsync(Trening trening)
        {
            bool categoryWithSameNameExist = await treningDbContext.Trening.AnyAsync(c => c.Name == trening.Name);

            if (categoryWithSameNameExist)
            {
                throw new Exception("A category with the same name already exists");
            }

            treningDbContext.Trening.Add(trening);

            return await treningDbContext.SaveChangesAsync();
        }
        public async Task<int> UpdateTreningAsync(Trening trening)
        {
            bool treningWithSameNameExist = await treningDbContext.Trening.AnyAsync(c => c.Name == trening.Name && c.Id != trening.Id);

            if (treningWithSameNameExist)
            {
                throw new Exception("A category with the same name already exists");
            }

            var treningToUpdate = await treningDbContext.Trening.FirstOrDefaultAsync(c => c.Id == trening.Id);

            if (treningToUpdate != null)
            {

                treningToUpdate.Name = trening.Name;
                treningToUpdate.Date = trening.Date;

                treningDbContext.Trening.Update(treningToUpdate);
                return await treningDbContext.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException($"The category to update can't be found.");
            }
        }
        public async Task<int> DeleteTreningAsync(int id)
        {

            var treningToDelete = await treningDbContext.Trening.FirstOrDefaultAsync(c => c.Id == id);

            if (treningToDelete != null)
            {
                treningDbContext.Trening.Remove(treningToDelete);
                return await treningDbContext.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException($"The category to delete can't be found.");
            }
        }
        public async Task<int> UpdateTreningNamesAsync(List<Trening> treninzi)
        {
            foreach (var trening in treninzi)
            {
                var treningToUpdate = await treningDbContext.Trening.FirstOrDefaultAsync(c => c.Id == trening.Id);

                if (treningToUpdate != null)
                {
                    treningToUpdate.Name = trening.Name;

                    treningDbContext.Trening.Update(treningToUpdate);
                }
            }

            return await treningDbContext.SaveChangesAsync();
        }
    }
}
