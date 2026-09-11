using Microsoft.AspNetCore.Mvc;
using ActivitiesManagement.Models;
using ActivitiesManagement.Repositories;

namespace ActivitiesManagement.Controllers
{
    public class UserTypeController : Controller
    {
        private readonly UserTypeRepository _repo;

        public UserTypeController(UserTypeRepository repo)
        {
            _repo = repo;
        }
 
       private string CurrentUserName => User?.Identity?.Name ?? "system";

        private const string AddEditViewName = "UserTypeAddEdit";

        public IActionResult Index()
        {
            var list = _repo.GetAll();
            return View(list);
        }

        [HttpGet]
        public IActionResult UserTypeInsert()
        {
            return View(AddEditViewName, new UserType());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UserTypeInsert(UserType model, string action)
        {
            if (string.IsNullOrWhiteSpace(model.Title) || string.IsNullOrWhiteSpace(model.ShortName))
            {
                ModelState.AddModelError("", "Title and Short Name are required.");
                return View(AddEditViewName, model);
            }

            _repo.Insert(model, CurrentUserName);
            TempData["SaveMessage"] = "User type saved successfully.";
            TempData["ShowSaveModalOnIndex"] = true;

            if (action == "saveAndAddAnother")
                return RedirectToAction("UserTypeInsert");

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult UserTypeEdit(long id)
        {
            var model = _repo.GetById(id);
            if (model == null) return NotFound();

            return View(AddEditViewName, model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UserTypeEdit(UserType model)
        {
            if (string.IsNullOrWhiteSpace(model.Title) || string.IsNullOrWhiteSpace(model.ShortName))
            {
                ModelState.AddModelError("", "Title and Short Name are required.");
                return View(AddEditViewName, model);
            }

            _repo.Update(model, CurrentUserName);
            TempData["SaveMessage"] = "User type updated successfully.";
            TempData["ShowSaveModalOnIndex"] = true;

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
    }
}