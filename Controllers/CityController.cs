using ActivitiesManagement.DataAccess;
using ActivitiesManagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace ActivitiesManagement.Controllers
{
    public class CityController : Controller
    {
        private readonly CityRepository _repo;
        private readonly CountryRepository _countryRepo;
        private readonly StateRepository _stateRepo;

        public CityController(CityRepository repo, CountryRepository countryRepo, StateRepository stateRepo)
        {
            _repo = repo;
            _countryRepo = countryRepo;
            _stateRepo = stateRepo;
        }

        public IActionResult Index()
        {
            return View(_repo.GetAll());
        }

        [HttpGet]
        public IActionResult AddEdit(int id = 0)
        {
            ViewBag.CountryList = _countryRepo.GetActiveList();

            var model = id > 0 ? _repo.GetById(id) : new City();

            // Preload states for the selected country (Edit scenario)
            ViewBag.StateList = model.CountryId > 0 ? _stateRepo.GetByCountryId(model.CountryId) : new List<State>();

            return View(model);
        }

        [HttpPost]
        public IActionResult AddEdit(City model, string? saveMode)
        {
            ViewBag.CountryList = _countryRepo.GetActiveList();
            ViewBag.StateList = model.CountryId > 0 ? _stateRepo.GetByCountryId(model.CountryId) : new List<State>();

            if (!ModelState.IsValid) return View(model);

            int currentUserId = 1;

            if (model.Id > 0)
                _repo.Update(model, currentUserId);
            else
                _repo.Insert(model, currentUserId);

            if (saveMode == "saveAndAdd")
                return RedirectToAction("AddEdit");

            return RedirectToAction("Index");
        }

        // AJAX: called when Country dropdown changes
        [HttpGet]
        public JsonResult GetStatesByCountry(int countryId)
        {
            var states = _stateRepo.GetByCountryId(countryId);
            return Json(states.Select(s => new { id = s.ID, text = s.StateName }));
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
