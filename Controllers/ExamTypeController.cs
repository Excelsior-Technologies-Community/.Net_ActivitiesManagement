using Microsoft.AspNetCore.Mvc;
using ActivitiesManagement.Models;
using ActivitiesManagement.DataAccess;

namespace ActivitiesManagement.Controllers
{
    public class ExamTypeController : Controller
    {
        private readonly ExamTypeRepository _repo;

        public ExamTypeController(ExamTypeRepository repo)
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
        public IActionResult AddEdit(long id = 0)
        {
            var model = id > 0 ? _repo.GetById(id) : new ExamType();
            if (model == null) return NotFound();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddEdit(ExamType model, List<string> GradeTitleList, bool saveAndAddAnother = false)
        {
            if (string.IsNullOrWhiteSpace(model.Title))
            {
                ModelState.AddModelError("Title", "Title is required.");
                if (model.Id > 0)
                    model.GradeTitles = _repo.GetDetailsByExamTypeId(model.Id);
                return View(model);
            }

            model.IsLead = Request.Form["IsLead"] == "on" ? "Y" : "N";
            model.IsInquiry = Request.Form["IsInquiry"] == "on" ? "Y" : "N";
            model.IsRegistration = Request.Form["IsRegistration"] == "on" ? "Y" : "N";
            model.IsCoaching = Request.Form["IsCoaching"] == "on" ? "Y" : "N";
            model.IsProcess = Request.Form["IsProcess"] == "on" ? "Y" : "N";
            model.IsMock = Request.Form["IsMock"] == "on" ? "Y" : "N";
            model.IsProfessional = Request.Form["IsProfessional"] == "on" ? "Y" : "N";
            model.IsEnglishTest = Request.Form["IsEnglishTest"] == "on" ? "Y" : "N";

            if (model.Id > 0)
            {
                _repo.Update(model, CurrentUser);
                TempData["SavedMessage"] = "Exam Type updated successfully.";
            }
            else
            {
                long newId = _repo.Insert(model, CurrentUser);

                if (GradeTitleList != null)
                {
                    foreach (var grade in GradeTitleList.Where(g => !string.IsNullOrWhiteSpace(g)))
                    {
                        _repo.InsertDetail(newId, grade, null);
                    }
                }

                TempData["SavedMessage"] = "Exam Type saved successfully.";
            }

            TempData["ShowSavedModalOnIndex"] = true;

            return saveAndAddAnother
                ? RedirectToAction("AddEdit")
                : RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult AddGradeDetail(long examTypeId, string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                return Json(new { success = false, message = "Grade title required." });

            long newId = _repo.InsertDetail(examTypeId, title, null);
            return Json(new { success = true, id = newId, title });
        }

        [HttpPost]
        public IActionResult RemoveGradeDetail(long id)
        {
            _repo.DeleteDetail(id);
            return Json(new { success = true });
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



