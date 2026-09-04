using ActivitiesManagement.DataAccess;
using ActivitiesManagement.Models;
using ActivitiesManagement.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ActivitiesManagement.Controllers
{
    public class ProgramController : Controller
    {
        private readonly ProgramRepository _repo;

        public ProgramController(ProgramRepository repo)
        {
            _repo = repo;
        }

        private static long CurrentUserId => 1; 

        public IActionResult Index()
        {
            return View(_repo.GetAll());
        }

        private void LoadDropdowns(long? instituteTypeId = null)
        {
            ViewBag.ProgramTypeList = _repo.GetProgramTypeDropdown();
            ViewBag.CountryList = _repo.GetCountryDropdown();
            ViewBag.InstituteTypeList = _repo.GetInstituteTypeDropdown();
            ViewBag.InstituteList = instituteTypeId.HasValue ? _repo.GetInstituteDropdown(instituteTypeId.Value) : new List<DropdownItem>();
            ViewBag.StreamList = _repo.GetStreamDropdown();
            ViewBag.SpecializationList = _repo.GetSpecializationDropdown();
            ViewBag.ProgramDurationList = _repo.GetProgramDurationDropdown();
            ViewBag.GradeList = _repo.GetGradeDropdown();
        }

        [HttpGet]
        public IActionResult AddEdit(long? id)
        {
            if (id == null)
            {
                LoadDropdowns();
                return View(new ProgramMaster());
            }

            var model = _repo.GetById(id.Value);
            if (model == null) return NotFound();

            LoadDropdowns(model.InstituteTypeId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddEdit(ProgramMaster model)
        {
            if (string.IsNullOrWhiteSpace(model.Title))
            {
                LoadDropdowns(model.InstituteTypeId);
                ModelState.AddModelError("", "Program Title is required.");
                return View(model);
            }

            if (model.Id == 0)
            {
                _repo.Insert(model, CurrentUserId);
                TempData["SaveMessage"] = "Program saved successfully.";
            }
            else
            {
                _repo.Update(model, CurrentUserId);
                TempData["SaveMessage"] = "Program updated successfully.";
            }

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

        [HttpGet]
        public JsonResult GetInstitutes(long instituteTypeId) => Json(_repo.GetInstituteDropdown(instituteTypeId));
    }
}
