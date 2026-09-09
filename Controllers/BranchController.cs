using Microsoft.AspNetCore.Mvc;
using ActivitiesManagement.Models;
using ActivitiesManagement.Repositories;

namespace ActivitiesManagement.Controllers
{
    public class BranchController : Controller
    {
        private readonly BranchRepository _repo;

        public BranchController(BranchRepository repo)
        {
            _repo = repo;
        }

        private static long CurrentUserId => 1; 

        public IActionResult Index()
        {
            return View(_repo.GetAll());
        }

        private void LoadDropdowns(Branch? model = null)
        {
            ViewBag.CompanyList = _repo.GetCompanyDropdown();
            ViewBag.CountryList = _repo.GetCountryDropdown();
            ViewBag.StateList = model?.CountryId > 0 ? _repo.GetStateDropdown(model.CountryId.Value) : new List<DropdownItem>();
            ViewBag.CityList = model?.StateId > 0 ? _repo.GetCityDropdown(model.StateId.Value) : new List<DropdownItem>();
            ViewBag.AreaList = model?.CityId > 0 ? _repo.GetAreaDropdown(model.CityId.Value) : new List<DropdownItem>();
        }

        [HttpGet]
        public IActionResult AddEdit(long? id)
        {
            if (id == null)
            {
                LoadDropdowns();
                ViewBag.SubBranchOptions = _repo.GetSubBranchOptions(null, new List<long>());
                return View(new Branch());
            }

            var model = _repo.GetById(id.Value);
            if (model == null) return NotFound();

            LoadDropdowns(model);
            var assignedIds = _repo.GetAssignedSubBranchIds(id.Value);
            ViewBag.SubBranchOptions = _repo.GetSubBranchOptions(id.Value, assignedIds);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddEdit(Branch model, List<long>? SelectedSubBranchIds)
        {
            if (string.IsNullOrWhiteSpace(model.BranchName) || string.IsNullOrWhiteSpace(model.BranchCode))
            {
                LoadDropdowns(model);
                ViewBag.SubBranchOptions = _repo.GetSubBranchOptions(model.Id == 0 ? null : model.Id, SelectedSubBranchIds ?? new());
                ModelState.AddModelError("", "Branch Code and Branch Name are required.");
                return View(model);
            }

            long branchId;
            if (model.Id == 0)
            {
                branchId = _repo.Insert(model, CurrentUserId);
                TempData["SaveMessage"] = "Branch saved successfully.";
            }
            else
            {
                _repo.Update(model, CurrentUserId);
                branchId = model.Id;
                TempData["SaveMessage"] = "Branch updated successfully.";
            }

            _repo.SaveSubBranches(branchId, SelectedSubBranchIds ?? new List<long>());

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
        public JsonResult GetStates(long countryId) => Json(_repo.GetStateDropdown(countryId));

        [HttpGet]
        public JsonResult GetCities(long stateId) => Json(_repo.GetCityDropdown(stateId));

        [HttpGet]
        public JsonResult GetAreas(long cityId) => Json(_repo.GetAreaDropdown(cityId));
    }
}