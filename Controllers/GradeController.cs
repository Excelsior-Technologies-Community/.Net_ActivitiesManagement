using ActivitiesManagement.DataAccess;
using ActivitiesManagement.Models;
using ActivitiesManagement.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ActivitiesManagement.Controllers
{
    public class GradeController : Controller
    {
        private readonly GradeRepository _repo;

        public GradeController(GradeRepository repo)
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
            if (id == null)
                return View(new Grade());

            var model = _repo.GetById(id.Value);
            if (model == null) return NotFound();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddEdit(Grade model, string action)
        {
            if (string.IsNullOrWhiteSpace(model.Title) || string.IsNullOrWhiteSpace(model.ShortCode))
            {
                ModelState.AddModelError("", "Title and Short Code are required.");
                return View(model);
            }

            if (model.Id == 0)
            {
                _repo.Insert(model, CurrentUserId);
                TempData["SaveMessage"] = "Grade saved successfully.";
            }
            else
            {
                _repo.Update(model, CurrentUserId);
                TempData["SaveMessage"] = "Grade updated successfully.";
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