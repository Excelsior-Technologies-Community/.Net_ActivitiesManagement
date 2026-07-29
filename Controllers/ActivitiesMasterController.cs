using ActivitiesManagement.DataAccess;
using ActivitiesManagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace ActivitiesManagement.Controllers
{
    public class ActivitiesMasterController : Controller
    {
        private readonly ActivityMasterRepository _repo;
        private readonly ActivityDetailRepository _detailRepo;
        private readonly ActionTypeRepository _actionTypeRepo;

        public ActivitiesMasterController(ActivityMasterRepository repo, ActivityDetailRepository detailRepo, ActionTypeRepository actionTypeRepo)
        {
            _repo = repo;
            _detailRepo = detailRepo;
            _actionTypeRepo = actionTypeRepo;
        }

        public IActionResult Index()
        {
            return View(_repo.GetAll());
        }

        [HttpGet]
        public IActionResult AddEdit(long id = 0)
        {
            ViewBag.ActionTypeList = _actionTypeRepo.GetActiveList();

            var vm = new ActivityFormViewModel();
            if (id > 0)
            {
                var master = _repo.GetById(id);
                vm.ID = master.Id;
                vm.Title = master.Title;
                vm.InAppShow = master.InAppShow == "true";
                vm.StatusFlag = master.StatusFlag;
                vm.Details = _detailRepo.GetByActivityId(id);
            }
            return View(vm);
        }

        [HttpPost]
        public IActionResult AddEdit(ActivityFormViewModel vm, string detailsJson)
        {
            long currentUserId = 1;
            string currentUser = "admin";

            if (!string.IsNullOrEmpty(detailsJson))
            {
                vm.Details = System.Text.Json.JsonSerializer.Deserialize<List<ActivityDetailRow>>(
                    detailsJson, new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }

            var master = new ActivityMaster
            {
                Id = vm.ID,
                Title = vm.Title,
                Amount = "",
                InAppShow = vm.InAppShow ? "true" : "false",
                SelectedActionTypeIds = new List<long>() 
            };

            long activityId;
            if (vm.ID > 0)
            {
                _repo.Update(master, currentUserId);
                activityId = vm.ID;
                _detailRepo.DeleteByActivityId(activityId); 
            }
            else
            {
                activityId = _repo.Insert(master, currentUserId);
            }

            foreach (var row in vm.Details ?? new List<ActivityDetailRow>())
            {
                _detailRepo.InsertRow(activityId, row, currentUser);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ChangeStatus(long id, string status)
        {
            _repo.ChangeStatus(id, status, 1);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(long id)
        {
            _detailRepo.DeleteByActivityId(id);
            _repo.Delete(id);
            return RedirectToAction("Index");
        }
    }
}