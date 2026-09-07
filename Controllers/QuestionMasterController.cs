using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ActivitiesManagement.Models;
using ActivitiesManagement.Repositories;

namespace ActivitiesManagement.Controllers
{
    public class QuestionMasterController : Controller
    {
        private readonly QuestionMasterRepository _repo;

        public QuestionMasterController(QuestionMasterRepository repo)
        {
            _repo = repo;
        }

        private long CurrentUserId => 1;

        private static readonly string[] AnswerTypes = new[]
        {
            "Text Box",
            "Text with Numerical Only",
            "File Upload",
            "Date",
            "Yes or No",
            "Drop-Down"
        };

        private void LoadDropdowns(QuestionMaster model)
        {
            ViewBag.AnswerTypeList = new SelectList(AnswerTypes, model?.AnsType);
            ViewBag.PageMasterList = new SelectList(_repo.GetPageMasterList(), "Id", "Title", model?.PageMasterId);
        }

        public IActionResult Index()
        {
            var list = _repo.GetAll();
            return View(list);
        }

        [HttpGet]
        public IActionResult AddEdit(long? id)
        {
            QuestionMaster model;

            if (id == null)
            {
                model = new QuestionMaster();
            }
            else
            {
                model = _repo.GetById(id.Value);
                if (model == null) return NotFound();
            }

            LoadDropdowns(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddEdit(QuestionMaster model, string action)
        {
            if (string.IsNullOrWhiteSpace(model.Title) || string.IsNullOrWhiteSpace(model.AnsType))
            {
                ModelState.AddModelError("", "Title and Answer Type are required.");
                LoadDropdowns(model);
                return View(model);
            }

            if (model.Id == 0)
            {
                _repo.Insert(model, CurrentUserId);
                TempData["SaveMessage"] = "Question saved successfully.";
            }
            else
            {
                _repo.Update(model, CurrentUserId);
                TempData["SaveMessage"] = "Question updated successfully.";
            }

            TempData["ShowSaveModalOnIndex"] = true;

            if (action == "saveAndAddAnother")
                return RedirectToAction("AddEdit");

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ChangeStatus(long id, string status)
        {
            _repo.ChangeStatus(id, status, CurrentUserId);
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
