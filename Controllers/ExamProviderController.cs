using Microsoft.AspNetCore.Mvc;
using ActivitiesManagement.Models;
using ActivitiesManagement.DataAccess;

namespace ActivitiesManagement.Controllers
{
    public class ExamProviderController : Controller
    {
        private readonly ExamProviderRepository _repo;

        public ExamProviderController (ExamProviderRepository repo)
        {
            _repo = repo;
        }

        private string CurrentUser => User?.Identity?.Name ?? "System";

        public IActionResult Index()
        {
            var list = _repo.GetAll();
            return View(list);
        }

        [HttpGet]
        public IActionResult AddEdit(long? id)
        {
            ViewBag.ExamTypeList = _repo.GetExamTypeDropDown();

            if (id == null)
                return View(new ExamProvider());

            var model = _repo.GetById(id.Value);
            if (model == null) return NotFound();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddEdit(ExamProvider model)
        {
            if(string.IsNullOrWhiteSpace(model.Title) || model.ExamTypeId == 0)
            {
                ViewBag.ExamTypeList = _repo.GetExamTypeDropDown();
                ModelState.AddModelError("", "Exam Type And Title are required.. ");
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddEdit(ExamProvider model)
        {
            if(string.IsNullOrWhiteSpace(model.Title) || model.ExamTypeId == 0)
            {
                ViewBag.ExamTypeList = _repo.GetExamTypeDropDown();
                ModelState.AddModelError("", "Exam Type and Title are required..");
                return View(model);
            }

            if(model.Id == 0)
            {
                _repo.Insert(model, CurrentUser);
                TempData["SaveMessage"] = "Exam Provider saved Successdully..";
            }
            else
            {
                _repo.Update(model, CurrentUser);
                TempData["SaveMessage"] = "Exam Provider Updated Successfully..";
            }

            TempData["ShowSaveModalOnIndex"] = true;
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
    }
}
