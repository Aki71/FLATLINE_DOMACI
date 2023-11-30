namespace BethanysPieShopAdmin.Data.Models
{
    public class Trening
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public string? Name { get; set; }
        public DateTime Date { get; set; }
        public ICollection<Vezba>? Vezbas { get; set; }

    }
}
