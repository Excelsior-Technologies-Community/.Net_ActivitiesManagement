using Microsoft.AspNetCore.Mvc;
using ActivitiesManagement.Models;
using ActivitiesManagement.Repositories;

namespace ActivitiesManagement.Controllers
{
    public class EMailConfigController : Controller
    {
        private readonly EMailConfigRepository _repo;

        public EMailConfigController(EMailConfigRepository repo)
        {
            _repo = repo;
        }

        private long CurrentUserId => 1;

        public IActionResult Index()
        {
            var list = _repo.GetAll();
            return View(list);
        }

        [HttpGet]
        public IActionResult AddEdit(int? id)
        {
            if (id == null)
                return View(new EMailConfig());

            var model = _repo.GetById(id.Value);
            if (model == null) return NotFound();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddEdit(EMailConfig model, string action)
        {
            if (string.IsNullOrWhiteSpace(model.EmailHost) ||
                string.IsNullOrWhiteSpace(model.EmailPort) ||
                string.IsNullOrWhiteSpace(model.EmailUserName) ||
                string.IsNullOrWhiteSpace(model.EmailPassword) ||
                string.IsNullOrWhiteSpace(model.EmailType))
            {
                ModelState.AddModelError("", "Host, Port, User Name, Password and Email Type are required.");
                return View(model);
            }

            if (model.Id == 0)
            {
                _repo.Insert(model, CurrentUserId);
                TempData["SaveMessage"] = "Email configuration saved successfully.";
            }
            else
            {
                _repo.Update(model, CurrentUserId);
                TempData["SaveMessage"] = "Email configuration updated successfully.";
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
