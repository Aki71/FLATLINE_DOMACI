
namespace BethanysPieShopAdmin.Data.Models.Repositories
{
    public interface ITreningRepository
    {
        IEnumerable<Trening> GetAllTrenings();
        Task<IEnumerable<Trening>> GetAllTreningAsync();
        Task<Trening?> GetTreningByIdAsync(int Id);
        Task<int> AddTreningAsync(Trening trening);
        Task<int> UpdateTreningAsync(Trening trening);
        Task<int> DeleteTreningAsync(int Id);
        Task<int> UpdateTreningNamesAsync(List<Trening> trening);
    }
}
