using Microsoft.AspNetCore.Mvc;
using BethanysPieShopAdmin.Data.Models.Repositories;
using BethanysPieShopAdmin.Utilities;
using BethanysPieShopAdmin.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using BethanysPieShopAdmin.Data.Models;

namespace BethanysPieShopAdmin.Data.Controllers
{
    public class VezbaController : Controller
    {
        private readonly IVezbaRepository _vezbaRepository;
        private readonly ITreningRepository _treningRepository;
        private int pageSize = 5;

        public VezbaController(IVezbaRepository vezbaRepository, ITreningRepository treningRepository)
        {
            _vezbaRepository = vezbaRepository;
            _treningRepository = treningRepository;
        }

        public async Task<IActionResult> Index()
        {
            var vezbe = await _vezbaRepository.GetAllVezbaAsync();
            VezbaSearchViewModel model = new VezbaSearchViewModel
            {
                Vezbe = vezbe,
                SearchTrening = null,
                Treninzi = new List<SelectListItem>(),
                SearchQuery = string.Empty
            };

            return View(model);
        }
        public async Task<IActionResult> IndexPaging(int? pageNumber)
        {
            var vezbe = await _vezbaRepository.GetVezbaPagedAsync(pageNumber, pageSize);

            pageNumber ??= 1;

            var count = await _vezbaRepository.GetAllVezbaCountAsync();

            return View(new PaginatedList<Vezba>(vezbe.ToList(), count, pageNumber.Value, pageSize));
        }
        public async Task<IActionResult> IndexPagingSorting(string sortBy, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortBy;

            ViewData["IdSortParam"] = String.IsNullOrEmpty(sortBy) || sortBy == "id";
            ViewData["NameSortParam"] = sortBy == "name" ? "name_desc" : "name";

            pageNumber ??= 1;

            var vezbe = await _vezbaRepository.GetVezbaSortedAndPagedAsync(sortBy, pageNumber, pageSize);

            var count = await _vezbaRepository.GetAllVezbaCountAsync();

            return View(new PaginatedList<Vezba>(vezbe.ToList(), count, pageNumber.Value, pageSize));
        }
        public async Task<IActionResult> Details(int id)
        {
            var vezba = await _vezbaRepository.GetVezbaByIdAsync(id);
            return View(vezba);
        }
        public async Task<IActionResult> Add()
        {
            try
            {
                IEnumerable<Trening>? allTrening = await _treningRepository.GetAllTreningAsync();
                IEnumerable<SelectListItem> selectListItems = new SelectList(allTrening, "Id", "Name", null);

                VezbaAddViewModel vezbaAddViewModel = new() { Treninzi = selectListItems };
                return View(vezbaAddViewModel);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = $"There was an error: {ex.Message}";
            }
            return View(new VezbaAddViewModel());

        }

        [HttpPost]
        public async Task<IActionResult> Add(VezbaAddViewModel vezbaAddViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Vezba vezba = new()
                    {
                        TreningId = vezbaAddViewModel.Vezba.TreningId,
                        VezbaDescription = vezbaAddViewModel.Vezba.VezbaDescription,
                        Rekviziti = vezbaAddViewModel.Vezba.Rekviziti,
                        VezbaName = vezbaAddViewModel.Vezba.VezbaName
                    };

                    await _vezbaRepository.AddVezbaAsync(vezba);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Adding the pie failed, please try again! Error: {ex.Message}");
            }

            var allTrening = await _treningRepository.GetAllTreningAsync();

            IEnumerable<SelectListItem> selectListItems = new SelectList(allTrening, "TreningId", "Name", null);

            vezbaAddViewModel.Treninzi = selectListItems;

            return View(vezbaAddViewModel);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var allTrening = await _treningRepository.GetAllTreningAsync();


            List<SelectListItem> x1 = allTrening.Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Name }).ToList();

            var selectedVezba = await _vezbaRepository.GetVezbaByIdAsync(id.Value);

            VezbaEditViewModel vezbaEditViewModel = new() { Treninzi = x1, Vezba = selectedVezba };
            return View(vezbaEditViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(VezbaEditViewModel vezbaEditViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _vezbaRepository.UpdateVezbaAsync(vezbaEditViewModel.Vezba);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Updating the category failed, please try again! Error: {ex.Message}");
            }

            var allTrening = await _treningRepository.GetAllTreningAsync();

            IEnumerable<SelectListItem> selectListItems = new SelectList(allTrening, "TreningId", "Name", null);

            vezbaEditViewModel.Treninzi = selectListItems;

            return View(vezbaEditViewModel);
        }



        public async Task<IActionResult> Delete(int id)
        {
            var selectedVezba = await _vezbaRepository.GetVezbaByIdAsync(id);

            return View(selectedVezba);
        }


        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int? vezbaId)
        {
            if (vezbaId == null)
            {
                ViewData["ErrorMessage"] = "Deleting the pie failed, invalid ID!";
                return View();
            }

            try
            {
                await _vezbaRepository.DeleteVezbaAsync(vezbaId.Value);
                TempData["VezbaDeleted"] = "Uspešno ste izbrisali vežbu!";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = $"Deleting the pie failed, please try again! Error: {ex.Message}";
            }

            var selectedVezba = await _vezbaRepository.GetVezbaByIdAsync(vezbaId.Value);
            return View(selectedVezba);
        }
        public async Task<IActionResult> Search(string? searchQuery, int? searchTrening)
        {
            var allCategories = await _treningRepository.GetAllTreningAsync();

            IEnumerable<SelectListItem> selectListItems = new SelectList(allCategories, "TreningId", "Name", null);

            if (searchQuery != null)
            {
                var vezba = await _vezbaRepository.SearchVezba(searchQuery, searchTrening);

                return View(new VezbaSearchViewModel()
                {
                    Vezbe = vezba,
                    SearchTrening = searchTrening,
                    Treninzi = selectListItems,
                    SearchQuery = searchQuery
                });
            }

            return View(new VezbaSearchViewModel()
            {
                Vezbe = new List<Vezba>(),
                SearchTrening = null,
                Treninzi = selectListItems,
                SearchQuery = string.Empty
            });
        }

    }
}
