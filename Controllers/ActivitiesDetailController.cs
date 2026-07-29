using ActivitiesManagement.DataAccess;
using ActivitiesManagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace ActivitiesManagement.Controllers
{
    public class ActivitiesDetailController : Controller
    {
        private readonly ActivityDetailRepository _repo;
        private readonly ActivityMasterRepository _activityRepo;
        private readonly ActionTypeRepository _actionTypeRepo;

        public ActivitiesDetailController(
            ActivityDetailRepository repo,
            ActivityMasterRepository activityRepo,
            ActionTypeRepository actionTypeRepo)
        {
            _repo = repo;
            _activityRepo = activityRepo;
            _actionTypeRepo = actionTypeRepo;
        }

        public IActionResult Index()
        {
            return View(_repo.GetAll());
        }

        [HttpGet]
        public IActionResult AddEdit(long id = 0)
        {
            ViewBag.ActivityList = _activityRepo.GetAll().Where(a => a.StatusFlag == "Active").ToList();
            ViewBag.ActionTypeList = _actionTypeRepo.GetActiveList();
            var model = id > 0 ? _repo.GetById(id) : new ActivityDetailMaster();
            return View(model);
        }

        [HttpPost]
        public IActionResult AddEdit(ActivityDetailMaster model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ActivityList = _activityRepo.GetAll().Where(a => a.StatusFlag == "Active").ToList();
                ViewBag.ActionTypeList = _actionTypeRepo.GetActiveList();
                return View(model);
            }

            string currentUser = "admin";

            if (model.ID > 0)
                _repo.Update(model, currentUser);
            else
                _repo.Insert(model, currentUser);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public JsonResult GetMasterStatusList(string masterKey)
        {
            var options = _repo.GetStatusOptionsForMaster(masterKey);
            return Json(options);
        }

        [HttpPost]
        public IActionResult ChangeStatus(long id, string status)
        {
            _repo.ChangeStatus(id, status, "admin");
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