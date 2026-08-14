using Microsoft.AspNetCore.Mvc;
using ActivitiesManagement.Models;
using ActivitiesManagement.DataAccess;

namespace ActivitiesManagement.Controllers
{
    public class InstituteTypeController : Controller
    {
        private readonly InstituteTypeRepository _repo;

        public InstituteTypeController(InstituteTypeRepository repo)
        {
            _repo = repo;
        }

        private int CurrentUserId => 1;
        public ActionResult Index()
        {
            var list = _repo.GetAll();
            return View(list);
        }

        [HttpGet]
        public IActionResult InstituteTypeInsert()
        {
            return View(new InstituteType());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult InstituteypeInsert(InstituteType model, string action)
        {
            if(string.IsNullOrWhiteSpace(model.Title) || string.IsNullOrWhiteSpace(model.ShortCode))
            {
                ModelState.AddModelError("", "Title and Short Name are required.");
                return View(model);
            }

            _repo.Insert(model, CurrentUserId);

            TempData["SaveMessage"] = "Institute Type Saved Successfully.";
            TempData["ShowSaveModalOnIndex"] = true;

            if (action == "saveAndAddAnother")
                return RedirectToAction("InstituteTypeIsert");

            return RedirectToAction("Index");

        }

        [HttpGet]
        public IActionResult InstituteTypeEdit(int id)
        {
            var model = _repo.GetById(id);
            if (model == null) return NotFound();
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult InstituteTypeEdit(InstituteType model)
        {
            if (string.IsNullOrWhiteSpace(model.Title) || string.IsNullOrWhiteSpace(model.ShortCode))
            {
                ModelState.AddModelError("", "Title and Short Name are Required.");
                return View(model);
            }

            _repo.Update(model, CurrentUserId);

            TempData["SaveMessage"] = "Institute Type Updated Successfully";
            TempData["ShowSaveModalOnIndex"] = true;

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
