using Microsoft.AspNetCore.Mvc;
using ActivitiesManagement.Models;
using ActivitiesManagement.DataAccess;

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
            ViewBag.ExamProviderList = _repo.GetExamProviderDropDown();
            ViewBag.CountryId = _repo.GetCountryDropDown();

            if(id == null)
            {
                ViewBag.StateList = new List<DropDownItem>();
                ViewBag.CityList = new List<DropDownItem>();
                ViewBag.AreaList = new List<DropDownItem>();
                return View(new ExamCenter());
            }

            var model = _repo.GetById(id.Value);
            if (model == null) return NotFound();

            ViewBag.StateList = model.CountryId > 0 ? _repo.GetStateDropDown(model.CountryId) : new List<DropDownItem>();
            ViewBag.CityList = model.StateId > 0 ? _repo.GetCityDropDown(model.StateId) : new List<DropDownItem>();
            ViewBag.AreaList = model.AreaId > 0 ? _repo.GetAreaDropDown(model.AreaId) : new List<DropDownItem>();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddEdit(ExamCenter model, string action)
        {
            if(string.IsNullOrWhiteSpace(model.ExamCenterName) || model.ExamProviderId == 0)
            {
                ViewBag.ExamProviderList = _repo.GetExamProviderDropDown();
                ViewBag.CountryList = _repo.GetCountryDropDown();
                ViewBag.StateList = model.CountryId > 0 ? _repo.GetStateDropDown(model.CountryId) : new List<DropDownItem>();
                ViewBag.CityList = model.StateId > 0 ?_repo.GetCityDropDown(model.StateId) : new List<DropDownItem>();
                ViewBag.AreaList = model.CityId > 0 ? _repo.GetAreaDropDown(model.CityId) : new List<DropDownItem>();
                ModelState.AddModelError("", "Center Name and Exam Provider Are Required..");
                return View(model);
            }

            if(model.Id == 0)
            {
                _repo.Insert(model, CurrentUser);
                TempData["SaveMessage"] = "Exam Center Saved SuccesFully.";
            }
            else
            {
                _repo.Update(model, CurrentUser);
                TempData["SaveMessge"] = "Exam Center Updated SuccesFully.";
            }

            TempData["ShowSaveModalOnIndex"] = true;

            if (action == "saveAndAddAnother")
                return RedirectToAction("AddEdit");

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ChangeStatus(int id,string status)
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
        public JsonResult GetState(long countryId)
        {
            var states = _repo.GetStateDropDown(countryId);
            return Json(states);
        }

        [HttpGet]
        public JsonResult GetCities(long stateId)
        {
            var cities = _repo.GetCityDropDown(stateId);
            return Json(cities);
        }

        [HttpGet]
        public JsonResult GetAreas(long cityId)
        {
            var areas = _repo.GetAreaDropDown(cityId);
            return Json(areas);
        }
    }
}
