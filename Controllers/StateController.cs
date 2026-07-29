using ActivitiesManagement.DataAccess;
using ActivitiesManagement.Models;
using Microsoft.AspNetCore.Mvc;
namespace ActivitiesManagement.Controllers
{
    public class StateController : Controller
    {
        private readonly StateRepository _repo;
        private readonly CountryRepository _countryRepo;

        public StateController(StateRepository repo, CountryRepository countryRepo)
        {
            _repo = repo;
            _countryRepo = countryRepo;
        }
        public ActionResult Index()
        {
            return View(_repo.GetAll());
        }

        [HttpGet]
        public IActionResult AddEdit(int id = 0)
        {
            ViewBag.CountryList = _countryRepo.GetActiveList();
            var model = id > 0 ? _repo.GetById(id) : new State();
            return View(model);
        }

        [HttpPost]
        public IActionResult AddEdit(State model, string? saveMode)
        {
            ViewBag.CountryList = _countryRepo.GetActiveList();
            if (!ModelState.IsValid) return View(model);

            int currentUserId = 1;

            if (model.ID > 0)
                _repo.Update(model, currentUserId);
            else
                _repo.Insert(model, currentUserId);

            if (saveMode == "saveAndAdd")
                return RedirectToAction("AddEdit");

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ChangeStatus(int id, string status)
        {
            _repo.ChangeStatus(id, status, 1);
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
