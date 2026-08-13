using Microsoft.AspNetCore.Mvc;
using ActivitiesManagement.Models;
using ActivitiesManagement.Repositories;

namespace ActivitiesManagement.Controllers
{
    public class ExamCenterController : Controller
    {
        private readonly ExamCenterRepository _repo;

        public ExamCenterController(ExamCenterRepository repo)
        {
            _repo = repo;
        }

        private string CurrentUser => User?.Identity?.Name ?? "System";

        public IActionResult Index()
        {
            var list = _repo.GetAll();
            return View(list);
        }

        [HttpGet]
        public IActionResult AddEdit(int? id)
        {
            ViewBag.ExamProviderList = _repo.GetExamProviderDropdown();
            ViewBag.CountryList = _repo.GetCountryDropdown();

            if (id == null)
            {
                ViewBag.StateList = new List<DropdownItem>();
                ViewBag.CityList = new List<DropdownItem>();
                ViewBag.AreaList = new List<DropdownItem>();
                return View(new ExamCenter());
            }

            var model = _repo.GetById(id.Value);
            if (model == null) return NotFound();

            ViewBag.StateList = model.CountryId > 0 ? _repo.GetStateDropdown(model.CountryId) : new List<DropdownItem>();
            ViewBag.CityList = model.StateId > 0 ? _repo.GetCityDropdown(model.StateId) : new List<DropdownItem>();
            ViewBag.AreaList = model.CityId > 0 ? _repo.GetAreaDropdown(model.CityId) : new List<DropdownItem>();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddEdit(ExamCenter model, string action)
        {
            if (string.IsNullOrWhiteSpace(model.ExamCenterName) || model.ExamProviderId == 0)
            {
                ViewBag.ExamProviderList = _repo.GetExamProviderDropdown();
                ViewBag.CountryList = _repo.GetCountryDropdown();
                ViewBag.StateList = model.CountryId > 0 ? _repo.GetStateDropdown(model.CountryId) : new List<DropdownItem>();
                ViewBag.CityList = model.StateId > 0 ? _repo.GetCityDropdown(model.StateId) : new List<DropdownItem>();
                ViewBag.AreaList = model.CityId > 0 ? _repo.GetAreaDropdown(model.CityId) : new List<DropdownItem>();
                ModelState.AddModelError("", "Center Name and Exam Provider are required.");
                return View(model);
            }

            if (model.Id == 0)
            {
                _repo.Insert(model, CurrentUser);
                TempData["SaveMessage"] = "Exam Center saved successfully.";
            }
            else
            {
                _repo.Update(model, CurrentUser);
                TempData["SaveMessage"] = "Exam Center updated successfully.";
            }

            TempData["ShowSaveModalOnIndex"] = true;

            if (action == "saveAndAddAnother")
                return RedirectToAction("AddEdit");

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ChangeStatus(int id, string status)
        {
            _repo.ChangeStatus(id, status, CurrentUser);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _repo.Delete(id);
            return RedirectToAction("Index");
        }


        [HttpGet]
        public JsonResult GetStates(long countryId)
        {
            var states = _repo.GetStateDropdown(countryId);
            return Json(states);
        }

        [HttpGet]
        public JsonResult GetCities(long stateId)
        {
            var cities = _repo.GetCityDropdown(stateId);
            return Json(cities);
        }

        [HttpGet]
        public JsonResult GetAreas(long cityId)
        {
            var areas = _repo.GetAreaDropdown(cityId);
            return Json(areas);
        }
    }
}








