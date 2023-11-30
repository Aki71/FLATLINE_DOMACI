using BethanysPieShopAdmin.Data.Models;
using BethanysPieShopAdmin.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BethanysPieShopAdmin.ViewModels
{
    public class VezbaAddViewModel
    {
        public IEnumerable<SelectListItem>? Treninzi { get; set; } = default!;
        public Vezba Vezba { get; set; } 
    }
}