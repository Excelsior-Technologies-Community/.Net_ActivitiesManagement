using ActivitiesManagement.DataAccess;
using ActivitiesManagement.Models;
using ActivitiesManagement.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ActivitiesManagement.Controllers
{
    public class CompanyController : Controller
    {
        private readonly CompanyRepository _repo;
        private readonly IWebHostEnvironment _env;

        public CompanyController(CompanyRepository repo, IWebHostEnvironment env)
        {
            _repo = repo;
            _env = env;
        }

        private string CurrentUserName => "System"; 

        public IActionResult Index()
        {
            var list = _repo.GetAll();
            return View(list);
        }

        [HttpGet]
        public IActionResult AddEdit(long? id)
        {
            ViewBag.CountryList = _repo.GetCountryList();

            if (id == null)
                return View(new Company());

            var model = _repo.GetById(id.Value);
            if (model == null) return NotFound();

        
            if (model.CountryId.HasValue)
                ViewBag.StateList = _repo.GetStateListByCountry(model.CountryId.Value);
            if (model.StateId.HasValue)
                ViewBag.CityList = _repo.GetCityListByState(model.StateId.Value);
            if (model.CityId.HasValue)
                ViewBag.AreaList = _repo.GetAreaListByCity(model.CityId.Value);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddEdit(Company model, string action, IFormFile logoFile)
        {
            if (string.IsNullOrWhiteSpace(model.CompanyName) || string.IsNullOrWhiteSpace(model.Email))
            {
                ModelState.AddModelError("", "Company Name and Email are required.");
                ViewBag.CountryList = _repo.GetCountryList();
                return View(model);
            }

            if (logoFile != null && logoFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "company");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(logoFile.FileName)}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    logoFile.CopyTo(stream);
                }

                model.CompanyLogo = $"/uploads/company/{fileName}";
            }

            if (model.Id == 0)
            {
                _repo.Insert(model, CurrentUserName);
                TempData["SaveMessage"] = "Company saved successfully.";
            }
            else
            {
                _repo.Update(model, CurrentUserName);
                TempData["SaveMessage"] = "Company updated successfully.";
            }

            TempData["ShowSaveModalOnIndex"] = true;

            if (action == "saveAndAddAnother")
                return RedirectToAction("AddEdit");

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ChangeStatus(long id, string status)
        {
            _repo.ChangeStatus(id, status, CurrentUserName);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(long id)
        {
            _repo.Delete(id);
            return RedirectToAction("Index");
        }


        [HttpGet]
        public JsonResult GetStates(long countryId)
        {
            return Json(_repo.GetStateListByCountry(countryId));
        }

        [HttpGet]
        public JsonResult GetCities(long stateId)
        {
            return Json(_repo.GetCityListByState(stateId));
        }

        [HttpGet]
        public JsonResult GetAreas(long cityId)
        {
            return Json(_repo.GetAreaListByCity(cityId));
        }
    }
}
