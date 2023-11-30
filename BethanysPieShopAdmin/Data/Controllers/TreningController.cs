using Microsoft.AspNetCore.Mvc;
using BethanysPieShopAdmin.Data.Models;
using BethanysPieShopAdmin.Data.Models.Repositories;
using BethanysPieShopAdmin.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;
using BethanysPieShopAdmin.Models;

namespace BethanysPieShopAdmin.Data.Controllers
{
    public class TreningController : Controller
    {
        private readonly ITreningRepository _treningRepository;

        public TreningController(ITreningRepository treningRepository)
        {
            _treningRepository = treningRepository;
        }
        public async Task<IActionResult> Index()
        {
            TreningListViewModel model = new()
            {
                Treninzi = (await _treningRepository.GetAllTreningAsync()).ToList()
            };

            return View(model);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var selectedTrening= await _treningRepository.GetTreningByIdAsync(id.Value);
            return View(selectedTrening);
        }
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add([Bind("Name,Description,DateAdded")] Trening trening)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _treningRepository.AddTreningAsync(trening);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Adding the category failed, please try again! Error: {ex.Message}");
            }

            return View(trening);
        }
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var selectedTrening = await _treningRepository.GetTreningByIdAsync(id.Value);
            return View(selectedTrening);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Trening trening)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _treningRepository.UpdateTreningAsync(trening);
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

            return View(trening);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var selectedTrening = await _treningRepository.GetTreningByIdAsync(id);

            return View(selectedTrening);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? TreningId)
        {
            if (TreningId == null)
            {
                ViewData["ErrorMessage"] = "Deleting the category failed, invalid ID!";
                return View();
            }

            try
            {
                await _treningRepository.DeleteTreningAsync(TreningId.Value);
                TempData["CategoryDeleted"] = "Category deleted successfully!";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = $"Deleting the category failed, please try again! Error: {ex.Message}";
            }

            var selectedTrening = await _treningRepository.GetTreningByIdAsync(TreningId.Value);
            return View(selectedTrening);

        }
        public async Task<IActionResult> BulkEdit()
        {
            List<TreningBulkEditViewModel> treningBulkEditViewModels = new List<TreningBulkEditViewModel>();

            var allTrening = await _treningRepository.GetAllTreningAsync();
            foreach (var trening in allTrening)
            {
                treningBulkEditViewModels.Add(new TreningBulkEditViewModel
                {
                    TreningId = trening.Id,
                    TreningName = trening.Name
                });
            }

            return View(treningBulkEditViewModels);
        }
        [HttpPost]
        public async Task<IActionResult> BulkEdit(List<TreningBulkEditViewModel> treningBulkEditViewModels)
        {
            List<Trening> treninzi = new List<Trening>();

            foreach (var treningBulkEditViewModel in treningBulkEditViewModels)
            {
                treninzi.Add(new Trening() { Id = treningBulkEditViewModel.TreningId, Name = treningBulkEditViewModel.TreningName });
            }

            await _treningRepository.UpdateTreningNamesAsync(treninzi);

            return RedirectToAction(nameof(Index));
        }
    }
}
