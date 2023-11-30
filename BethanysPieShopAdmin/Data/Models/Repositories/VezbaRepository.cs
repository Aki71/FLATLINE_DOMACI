
using Microsoft.EntityFrameworkCore;

namespace BethanysPieShopAdmin.Data.Models.Repositories
{
    public class VezbaRepository : IVezbaRepository
    {
        private readonly TreningDbContext _treningDbContext;

        public VezbaRepository(TreningDbContext treningDbContext)
        {
            _treningDbContext = treningDbContext;
        }
        public async Task<IEnumerable<Vezba>> GetAllVezbaAsync()
        {
            return await _treningDbContext.Vezba.OrderBy(c => c.VezbaId).AsNoTracking().ToListAsync();
        }
        public async Task<Vezba?> GetVezbaByIdAsync(int vezbaId)
        {
            return await _treningDbContext.Vezba.Include(p => p.Trening).AsNoTracking().FirstOrDefaultAsync(p => p.VezbaId == vezbaId);
        }
        public async Task<int> GetAllVezbaCountAsync()
        {
            IQueryable<Vezba> allVezba = from p in _treningDbContext.Vezba
                                      select p;
            var count = await allVezba.CountAsync();
            return count;
        }
        public async Task<IEnumerable<Vezba>> GetVezbaPagedAsync(int? pageNumber, int pageSize)
        {
            IQueryable<Vezba> vezbe = from p in _treningDbContext.Vezba
                                   select p;

            pageNumber ??= 1;

            vezbe = vezbe.Skip((pageNumber.Value - 1) * pageSize).Take(pageSize);

            return await vezbe.AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<Vezba>> GetVezbaSortedAndPagedAsync(string sortBy, int? pageNumber, int pageSize)
        {
            IQueryable<Vezba> allVezba = from p in _treningDbContext.Vezba
                                      select p;
            IQueryable<Vezba> vezbe;

            switch (sortBy)
            {
                case "name_desc":
                    vezbe = allVezba.OrderByDescending(p => p.VezbaDescription);
                    break;
                case "name":
                    vezbe = allVezba.OrderBy(p => p.VezbaName);
                    break;
                case "id_desc":
                    vezbe = allVezba.OrderByDescending(p => p.VezbaId);
                    break;
                case "id":
                    vezbe = allVezba.OrderBy(p => p.VezbaId);
                    break;
                case "rekviziti":
                    vezbe = allVezba.OrderBy(p => p.Rekviziti);
                    break;
                default:
                    vezbe = allVezba.OrderBy(p => p.VezbaId);
                    break;
            }

            pageNumber ??= 1;

            vezbe = vezbe.Skip((pageNumber.Value - 1) * pageSize).Take(pageSize);

            return await vezbe.AsNoTracking().ToListAsync(); ;
        }
        public async Task<int> AddVezbaAsync(Vezba vezba)
        {
            _treningDbContext.Vezba.Add(vezba);
            return await _treningDbContext.SaveChangesAsync();
        }
        public async Task<int> UpdateVezbaAsync(Vezba vezba)
        {

            var vezbaToUpdate = await _treningDbContext.Vezba.FirstOrDefaultAsync(c => c.VezbaId == vezba.VezbaId);
            if (vezbaToUpdate != null)
            {
                vezbaToUpdate.TreningId = vezba.TreningId;
                vezbaToUpdate.VezbaDescription = vezba.VezbaDescription;
                vezbaToUpdate.Rekviziti = vezba.Rekviziti;
                vezbaToUpdate.VezbaName = vezba.VezbaName;

                _treningDbContext.Vezba.Update(vezbaToUpdate);
                return await _treningDbContext.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException($"The pie to update can't be found.");
            }
        }
        public async Task<int> DeleteVezbaAsync(int id)
        {
            var vezbaToDelete = await _treningDbContext.Vezba.FirstOrDefaultAsync(c => c.VezbaId == id);

            if (vezbaToDelete != null)
            {
                _treningDbContext.Vezba.Remove(vezbaToDelete);
                return await _treningDbContext.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException($"The pie to delete can't be found.");
            }
        }
        public async Task<IEnumerable<Vezba>> SearchVezba(string searchQuery, int? treningId)
        {
            var vezbe = from p in _treningDbContext.Vezba
                       select p;

            if (!string.IsNullOrEmpty(searchQuery))
            {
                vezbe = vezbe.Where(s => s.VezbaName.Contains(searchQuery) || s.VezbaDescription.Contains(searchQuery) || s.Rekviziti.Contains(searchQuery));
            }

            if (treningId != null)
            {
                vezbe = vezbe.Where(s => s.TreningId == treningId);
            }

            return await vezbe.ToListAsync();
        }
    }
}
