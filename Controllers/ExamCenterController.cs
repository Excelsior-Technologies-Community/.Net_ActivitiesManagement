using Microsoft.AspNetCore.Mvc;
using ActivitiesManagement.Models;
using ActivitiesManagement.DataAccess;

namespace ActivitiesManagement.Controllers
{
    public class ExamCenterController : Controller
    {
        private readonly ExamCenterRepository _repo;

        public ExamCenterController(ExamCenterRepository repo)
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
        public IActionResult AddEdit(int? id)
        {
            ViewBag.ExamProviderList = _repo.GetExamProviderDropDown();
            ViewBag.CountryId = _repo.GetCountryDropDown();

            if(id == null)
            {
                ViewBag.StateList = new List<DropDownItem>();
                ViewBag.CityList = new List<DropDownItem>();
                ViewBag.AreaList = new List<DropDownItem>();
                return View(new ExamCenter());
            }

            var model = _repo.GetById(id.Value);
            if (model == null) return NotFound();

            ViewBag.StateList = model.CountryId > 0 ? _repo.GetStateDropDown(model.CountryId) : new List<DropDownItem>();
            ViewBag.CityList = model.StateId > 0 ? _repo.GetCityDropDown(model.StateId) : new List<DropDownItem>();
            ViewBag.AreaList = model.AreaId > 0 ? _repo.GetAreaDropDown(model.AreaId) : new List<DropDownItem>();

            return View(model);
        }
    }
}
