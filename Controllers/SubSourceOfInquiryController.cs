using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ActivitiesManagement.Models;
using ActivitiesManagement.Repositories;

namespace ActivitiesManagement.Controllers
{
    public class SubSourceOfInquiryController : Controller
    {
        private readonly SubSourceOfInquiryRepository _repo;
        private readonly SourceOfInquiryRepository _sourceOfInquiryRepo;

        public SubSourceOfInquiryController(SubSourceOfInquiryRepository repo, SourceOfInquiryRepository sourceOfInquiryRepo)
        {
            _repo = repo;
            _sourceOfInquiryRepo = sourceOfInquiryRepo;
        }

        private long CurrentUserId => 1;

        
        private const string AddEditViewName = "SubSourceOfInquiryAddEdit";

        public IActionResult Index()
        {
            var list = _repo.GetAll();
            return View(list);
        }

        private void LoadSourceOfInquiryDropdown(long? selectedId = null)
        {
            var options = _sourceOfInquiryRepo.GetActiveList()
                .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Title })
                .ToList();

            ViewBag.SourceOfInquiryList = new SelectList(options, "Value", "Text", selectedId);
        }

        [HttpGet]
        public IActionResult SubSourceOfInquiryInsert()
        {
            LoadSourceOfInquiryDropdown();
            return View(AddEditViewName, new SubSourceOfInquiry());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SubSourceOfInquiryInsert(SubSourceOfInquiry model, string action)
        {
            if (string.IsNullOrWhiteSpace(model.Title) || model.SourceOfInquiryId == 0)
            {
                ModelState.AddModelError("", "Source Of Inquiry and Title are required.");
                LoadSourceOfInquiryDropdown(model.SourceOfInquiryId);
                return View(AddEditViewName, model);
            }

            _repo.Insert(model, CurrentUserId);
            TempData["SaveMessage"] = "Sub source of inquiry saved successfully.";
            TempData["ShowSaveModalOnIndex"] = true;

            if (action == "saveAndAddAnother")
                return RedirectToAction("SubSourceOfInquiryInsert");

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult SubSourceOfInquiryEdit(long id)
        {
            var model = _repo.GetById(id);
            if (model == null) return NotFound();

            LoadSourceOfInquiryDropdown(model.SourceOfInquiryId);
            return View(AddEditViewName, model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SubSourceOfInquiryEdit(SubSourceOfInquiry model)
        {
            if (string.IsNullOrWhiteSpace(model.Title) || model.SourceOfInquiryId == 0)
            {
                ModelState.AddModelError("", "Source Of Inquiry and Title are required.");
                LoadSourceOfInquiryDropdown(model.SourceOfInquiryId);
                return View(AddEditViewName, model);
            }

            _repo.Update(model, CurrentUserId);
            TempData["SaveMessage"] = "Sub source of inquiry updated successfully.";
            TempData["ShowSaveModalOnIndex"] = true;

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