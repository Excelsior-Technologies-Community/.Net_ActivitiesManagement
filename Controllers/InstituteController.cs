using Microsoft.AspNetCore.Mvc;
using ActivitiesManagement.Models;
using ActivitiesManagement.Repositories;

namespace ActivitiesManagement.Controllers
{
    public class InstituteController : Controller
    {
        private readonly InstituteRepository _repo;
        private readonly IWebHostEnvironment _env;

        public InstituteController(InstituteRepository repo, IWebHostEnvironment env)
        {
            _repo = repo;
            _env = env;
        }

        private string CurrentUser => User?.Identity?.Name ?? "System";

        public IActionResult Index()
        {
            var list = _repo.GetAll();
            return View(list);
        }

        private void LoadDropdowns(Institute model = null)
        {
            ViewBag.InstituteTypeList = _repo.GetInstituteTypeDropdown();
            ViewBag.CountryList = _repo.GetCountryDropdown();
            ViewBag.StateList = model != null && model.CountryId > 0 ? _repo.GetStateDropdown(model.CountryId) : new List<DropdownItem>();
            ViewBag.CityList = model != null && model.StateId > 0 ? _repo.GetCityDropdown(model.StateId) : new List<DropdownItem>();
            ViewBag.AreaList = model != null && model.CityId > 0 ? _repo.GetAreaDropdown(model.CityId) : new List<DropdownItem>();
        }

        [HttpGet]
        public IActionResult AddEdit(long? id)
        {
            if (id == null)
            {
                LoadDropdowns();
                return View(new Institute());
            }

            var model = _repo.GetById(id.Value);
            if (model == null) return NotFound();

            LoadDropdowns(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddEdit(Institute model, string action, IFormFile logoFile)
        {
            if (string.IsNullOrWhiteSpace(model.InstituteName) || model.InstituteTypeId == 0)
            {
                LoadDropdowns(model);
                ModelState.AddModelError("", "Institute Type and Institute Name are required.");
                return View(model);
            }

            if (logoFile != null && logoFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "institute-logos");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(logoFile.FileName)}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    logoFile.CopyTo(stream);
                }

                model.InstituteLogo = "/uploads/institute-logos/" + fileName;
            }

            if (model.Id == 0)
            {
                _repo.Insert(model, CurrentUser);
                TempData["SaveMessage"] = "Institute saved successfully.";
            }
            else
            {
                _repo.Update(model, CurrentUser);
                TempData["SaveMessage"] = "Institute updated successfully.";
            }

            TempData["ShowSaveModalOnIndex"] = true;

            if (action == "saveAndAddAnother")
                return RedirectToAction("AddEdit");

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ChangeStatus(long id, string status)
        {
            _repo.ChangeStatus(id, status, CurrentUser);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(long id)
        {
            _repo.Delete(id);
            return RedirectToAction("Index");
        }


        [HttpGet]
        public JsonResult GetStates(long countryId) => Json(_repo.GetStateDropdown(countryId));

        [HttpGet]
        public JsonResult GetCities(long stateId) => Json(_repo.GetCityDropdown(stateId));

        [HttpGet]
        public JsonResult GetAreas(long cityId) => Json(_repo.GetAreaDropdown(cityId));
    }
}