using ActivitiesManagement.DataAccess;
using ActivitiesManagement.Models;
using ActivitiesManagement.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ActivitiesManagement.Controllers
{
    public class CurrencyController : Controller
    {
        private readonly CurrencyRepository _repo;

        public CurrencyController(CurrencyRepository repo)
        {
            _repo = repo;
        }

        private int CurrentUserId => 1; 

        public IActionResult Index()
        {
            var list = _repo.GetAll();
            return View(list);
        }

        [HttpGet]
        public IActionResult AddEdit(int? id)
        {
            ViewBag.CountryList = _repo.GetCountryDropdown();

            if (id == null)
                return View(new Currency());

            var model = _repo.GetById(id.Value);
            if (model == null) return NotFound();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddEdit(Currency model, string action)
        {
            if (string.IsNullOrWhiteSpace(model.Title) || model.CountryId == 0)
            {
                ViewBag.CountryList = _repo.GetCountryDropdown();
                ModelState.AddModelError("", "Title and Country are required.");
                return View(model);
            }

            if (model.Id == 0)
            {
                _repo.Insert(model, CurrentUserId);
                TempData["SaveMessage"] = "Currency saved successfully.";
            }
            else
            {
                _repo.Update(model, CurrentUserId);
                TempData["SaveMessage"] = "Currency updated successfully.";
            }

            TempData["ShowSaveModalOnIndex"] = true;

            if (action == "saveAndAddAnother")
                return RedirectToAction("AddEdit");

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ChangeStatus(int id, string status)
        {
            _repo.ChangeStatus(id, status, CurrentUserId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _repo.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
