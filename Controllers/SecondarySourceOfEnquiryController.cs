using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ActivitiesManagement.Models;
using ActivitiesManagement.DataAccess;

namespace ActivitiesManagement.Controllers
{
    public class SecondarySourceOfEnquiryController : Controller
    {
        private readonly ISecondarySourceOfEnquiryRepository _repo;

        public SecondarySourceOfEnquiryController(ISecondarySourceOfEnquiryRepository repo)
        {
            _repo = repo;
        }

        public IActionResult Index()
        {
            var list = _repo.GetAll();
            return View(list);
        }

       
        public IActionResult AddEdit(long id = 0)
        {
            var model = id != 0 ? _repo.GetById(id) : new SecondarySourceOfEnquiry();

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddEdit(SecondarySourceOfEnquiry model, string action)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            long currentUserId = 1;

            if (model.Id == 0)
            {
                _repo.Insert(model, currentUserId);
                TempData["SuccessMessage"] = "Secondary Source of Enquiry added successfully.";

                if (action == "saveAndAddAnother")
                {
                    return RedirectToAction("AddEdit");
                }
            }
            else
            {
                _repo.Update(model, currentUserId);
                TempData["SuccessMessage"] = "Secondary Source of Enquiry updated successfully.";
            }

            return RedirectToAction("Index");
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangeStatus(long id, string currentStatus)
        {
            long currentUserId = 1; 

            string newStatus = currentStatus == "Active" ? "InActive" : "Active";
            _repo.ChangeStatus(id, newStatus, currentUserId);

            TempData["SuccessMessage"] = $"Status changed to {newStatus}.";
            return RedirectToAction("Index");
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(long id)
        {
            _repo.Delete(id);
            TempData["SuccessMessage"] = "Secondary Source of Enquiry deleted successfully.";
            return RedirectToAction("Index");
        }
    }
}