using BethanysPieShopAdmin.Models;

namespace BethanysPieShopAdmin.Data.Models
{
    public class Vezba
    {
        public int VezbaId { get; set; } 
        public int PlanId { get; set; }
        public string VezbaName { get; set; }
        public string VezbaDescription { get; set; }
        public string Rekviziti { get; set; }
        public int TreningId { get; set; }
        public Trening? Trening { get; set; }

    }
}
