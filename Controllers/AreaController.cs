using ActivitiesManagement.DataAccess;
using ActivitiesManagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace ActivitiesManagement.Controllers
{
    public class AreaController : Controller
    {
        private readonly AreaRepository _repo;
        private readonly CountryRepository _countryRepo;
        private readonly StateRepository _stateRepo;
        private readonly CityRepository _cityRepo;

        public AreaController(AreaRepository repo, CountryRepository countryRepo, StateRepository stateRepo, CityRepository cityRepo)
        {
            _repo = repo;
            _countryRepo = countryRepo;
            _stateRepo = stateRepo;
            _cityRepo = cityRepo;
        }

        public IActionResult Index()
        {
            return View(_repo.GetAll());
        }

        [HttpGet]
        public IActionResult AddEdit(int id = 0)
        {
            ViewBag.CountryList = _countryRepo.GetActiveList();

            var model = id > 0 ? _repo.GetById(id) : new Area();

            ViewBag.StateList = model.CountryId > 0 ? _stateRepo.GetByCountryId(model.CountryId) : new List<State>();
            ViewBag.CityList = model.StateId > 0 ? _cityRepo.GetByStateId(model.StateId) : new List<City>();

            return View(model);
        }

        [HttpPost]
        public IActionResult AddEdit(Area model, string? saveMode)
        {
            ViewBag.CountryList = _countryRepo.GetActiveList();
            ViewBag.StateList = model.CountryId > 0 ? _stateRepo.GetByCountryId(model.CountryId) : new List<State>();
            ViewBag.CityList = model.StateId > 0 ? _cityRepo.GetByStateId(model.StateId) : new List<City>();

            if (!ModelState.IsValid) return View(model);

            int currentUserId = 1;
            bool isNew = model.Id == 0;

            if (model.Id > 0)
                _repo.Update(model, currentUserId);
            else
                model.Id = _repo.Insert(model, currentUserId);

            TempData["SavedMessage"] = isNew ? "Area Added Successfully" : "Area Updated Successfully";

            if (saveMode == "saveAndAdd")
            {
                TempData["ShowSavedModal"] = "true";
                return RedirectToAction("AddEdit");
            }

            TempData["ShowSavedModalOnIndex"] = "true";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public JsonResult GetStatesByCountry(int countryId)
        {
            var states = _stateRepo.GetByCountryId(countryId);
            return Json(states.Select(s => new { id = s.ID, text = s.StateName }));
        }

        [HttpGet]
        public JsonResult GetCitiesByState(int stateId)
        {
            var cities = _cityRepo.GetByStateId(stateId);
            return Json(cities.Select(c => new { id = c.Id, text = c.CityName }));
        }

        [HttpPost]
        public IActionResult ChangeStatus(int id, string status)
        {
            _repo.ChangeStatus(id, status, 1);
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
