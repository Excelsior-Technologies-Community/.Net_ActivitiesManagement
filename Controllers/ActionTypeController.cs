using ActivitiesManagement.DataAccess;
using ActivitiesManagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace ActivitiesManagement.Controllers
{
    public class ActionTypeController : Controller
    {
        private readonly ActionTypeRepository _repo;
        public ActionTypeController(ActionTypeRepository repo) { _repo = repo; }

        public IActionResult Index()
        {
            var list = _repo.GetAll();
            ViewBag.DebugCount = list.Count;
            return View(list);

        }

        [HttpGet]
        public IActionResult AddEdit(long id)
        {
            var model = id > 0 ? _repo.GetById(id) : new ActionType();
            return View(model);
        }

        [HttpPost]
        public IActionResult AddEdit(ActionType model)
        {
            if (!ModelState.IsValid) return View(model);

            long currentUserId = 1; 

            if (model.ID > 0)
                _repo.Update(model, currentUserId);
            else
                _repo.Insert(model, currentUserId);

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
            _repo.Delete(id);
            return RedirectToAction("Index");
     
        }
        
    }
}