using BethanysPieShopAdmin.Models;

namespace BethanysPieShopAdmin.Data.Models.Repositories
{
    public interface IVezbaRepository
    {
        Task<IEnumerable<Vezba>> GetAllVezbaAsync();
        Task<Vezba?> GetVezbaByIdAsync(int vezbaId);
        Task<int> AddVezbaAsync(Vezba vezba);
        Task<int> UpdateVezbaAsync(Vezba vezba);
        Task<int> DeleteVezbaAsync(int Id);
        Task<int> GetAllVezbaCountAsync();
        Task<IEnumerable<Vezba>> GetVezbaPagedAsync(int? pageNumber, int pageSize);
        Task<IEnumerable<Vezba>> GetVezbaSortedAndPagedAsync(string sortBy, int? pageNumber, int pageSize);
        Task<IEnumerable<Vezba>> SearchVezba(string searchQuery, int? treningId);
    }
}
