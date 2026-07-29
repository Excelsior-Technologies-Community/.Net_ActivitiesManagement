using ActivitiesManagement.DataAccess;
using ActivitiesManagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace ActivitiesManagement.Controllers
{
    public class CountryController : Controller
    {
        private readonly CountryRepository _repo;
        private readonly IWebHostEnvironment _env;

        public CountryController(CountryRepository repo, IWebHostEnvironment env)
        {
            _repo = repo;
            _env = env;
        }

        public IActionResult Index()
        {
            return View(_repo.GetAll());
        }

        [HttpGet]
        public IActionResult AddEdit(long id = 0)
        {
            var model = id > 0 ? _repo.GetById(id) : new Country();
            return View(model);
        }

        [HttpPost]
        public IActionResult AddEdit(Country model, IFormFile? flagImageFile, string? saveMode)
        {
            if (!ModelState.IsValid) return View(model);

            long currentUserId = 1;

            if (flagImageFile != null && flagImageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "countries");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid() + Path.GetExtension(flagImageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    flagImageFile.CopyTo(stream);
                }

                model.CountryFlagImage = "/uploads/countries/" + fileName;
            }

            bool isNew = model.ID == 0;

            if (model.ID > 0)
                _repo.Update(model, currentUserId);
            else
                model.ID = _repo.Insert(model, currentUserId);

            TempData["SavedMessage"] = isNew ? "Country Added Successfully" : "Country Updated Successfully";

            if (saveMode == "saveAndAdd")
            {
                TempData["ShowSavedModal"] = "true";
                return RedirectToAction("AddEdit");
            }

            TempData["ShowSavedModalOnIndex"] = "true";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ChangeStatus(long id, string status)
        {
            _repo.ChangeStatus(id, status, 1);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(long id)
        {
            _repo.Delete(id);
            return RedirectToAction("Index");
        }
    }
}