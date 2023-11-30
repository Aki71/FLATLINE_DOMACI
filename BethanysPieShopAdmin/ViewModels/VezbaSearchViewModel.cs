using BethanysPieShopAdmin.Data.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BethanysPieShopAdmin.ViewModels
{
    public class VezbaSearchViewModel
    {
        public IEnumerable<Vezba>? Vezbe { get; set; }
        public IEnumerable<SelectListItem>? Treninzi { get; set; } = default!;
        public string? SearchQuery { get; set; }
        public int? SearchTrening { get; set; }

    }
}
