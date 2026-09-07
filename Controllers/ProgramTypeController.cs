using ActivitiesManagement.DataAccess;
using ActivitiesManagement.Models;
using ActivitiesManagement.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ActivitiesManagement.Controllers
{
    public class ProgramTypeController : Controller
    {
        private readonly ProgramTypeRepository _repo;

        public ProgramTypeController(ProgramTypeRepository repo)
        {
            _repo = repo;
        }

        public IActionResult Index()
        {
            var list = _repo.GetAll();
            return View(list);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return PartialView("_CreateEdit", new ProgramType());
        }
        [HttpGet]
        public IActionResult AddEdit(long id = 0)
        {
            if (id == 0)
            {
                return View(new ProgramType());
            }

            var item = _repo.GetById(id);
            if (item == null) return NotFound();

            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProgramType model)
        {
            if (ModelState.IsValid)
            {
                ModelState.AddModelError("", "Title and Short Code are required.");
                return View(model   );
            }
            return PartialView("_CreateEdit", model);
        }

        [HttpGet]
        public IActionResult Edit(long id)
        {
            var item = _repo.GetById(id);
            if (item == null) return NotFound();
            return PartialView("_CreateEdit", item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProgramType model)
        {
            if (ModelState.IsValid)
            {
                long currentUserId = 1; // Replace with user context/session
                _repo.Update(model, currentUserId);
                return Json(new { success = true, message = "Program Type updated successfully." });
            }
            return PartialView("_CreateEdit", model);
        }

        [HttpPost]
        public IActionResult ToggleStatus(long id, string currentStatus)
        {
            long currentUserId = 1;
            string newStatus = currentStatus == "A" ? "I" : "A";
            _repo.ChangeStatus(id, newStatus, currentUserId);
            return Json(new { success = true, message = "Status changed successfully." });
        }

        [HttpPost]
        public IActionResult Delete(long id)
        {
            _repo.Delete(id);
            return Json(new { success = true, message = "Program Type deleted successfully." });
        }
    }
}
