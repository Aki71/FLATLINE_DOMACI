using BethanysPieShopAdmin.Data.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BethanysPieShopAdmin.ViewModels
{
    public class VezbaEditViewModel
    {
        public IEnumerable<SelectListItem>? Treninzi { get; set; } = default!;
        public Vezba Vezba { get; set; }
    }
}