using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using ActivitiesManagement.Models;
using ActivitiesManagement.Repositories;
using ActivitiesManagement.DataAccess;

namespace ActivitiesManagement.Controllers
{
    public class DashboardSettingsController : Controller
    {
        private readonly DashboardSettingsRepository _repo;
        private readonly BranchRepository _branchRepo;
        private readonly CompanyRepository _companyRepo;
        private readonly SourceOfInquiryRepository _sourceOfInquiryRepo;
        private readonly ActivityMasterRepository _activityRepo;
        private readonly ActionTypeRepository _actionTypeRepo;
        private readonly MasterStatusRepository _masterStatusRepo;
        private readonly VisaTypeRepository _visaTypeRepo;

        public DashboardSettingsController(
            DashboardSettingsRepository repo,
            BranchRepository branchRepo,
            CompanyRepository companyRepo,
            SourceOfInquiryRepository sourceOfInquiryRepo,
            ActivityMasterRepository activityRepo,
            ActionTypeRepository actionTypeRepo,
            MasterStatusRepository masterStatusRepo,
            VisaTypeRepository visaTypeRepo)
        {
            _repo = repo;
            _branchRepo = branchRepo;
            _companyRepo = companyRepo;
            _sourceOfInquiryRepo = sourceOfInquiryRepo;
            _activityRepo = activityRepo;
            _actionTypeRepo = actionTypeRepo;
            _masterStatusRepo = masterStatusRepo;
            _visaTypeRepo = visaTypeRepo;
        }

        private static long CurrentUserId => 1; 

        [HttpGet]
        public IActionResult Index()
        {
            var branches = _branchRepo.GetAll()
                .Where(b => (b.StatusFlag == "A" || b.StatusFlag == "Active") && !string.IsNullOrWhiteSpace(b.BranchName))
                .Select(b => b.BranchName.Trim())
                .Distinct()
                .ToList();

            var companies = _companyRepo.GetAll()
                .Where(c => !string.IsNullOrWhiteSpace(c.CompanyName))
                .Select(c => c.CompanyName.Trim())
                .Distinct()
                .ToList();

            var inquirySources = _sourceOfInquiryRepo.GetAll()
                .Where(s => (s.StatusFlag == "A" || s.StatusFlag == "Active") && !string.IsNullOrWhiteSpace(s.Title))
                .Select(s => s.Title.Trim())
                .Distinct()
                .ToList();

            var activities = _activityRepo.GetAll()
                .Where(a => (a.StatusFlag == "Active" || a.StatusFlag == "A") && !string.IsNullOrWhiteSpace(a.Title))
                .ToList();

            var actionTypes = _actionTypeRepo.GetActiveList();

            var masterStatuses = _masterStatusRepo.GetAll()
                .Where(m => (m.StatusFlag == "A" || m.StatusFlag == "Active") && !string.IsNullOrWhiteSpace(m.StatusCode))
                .ToList();

            var visaTypes = _visaTypeRepo.GetAll()
                .Where(v => (v.StatusFlag == "A" || v.StatusFlag == "Active") && !string.IsNullOrWhiteSpace(v.Title))
                .ToList();

            var existing = _repo.GetAllForUser(CurrentUserId);

            ViewBag.Branches = branches;
            ViewBag.Companies = companies;
            ViewBag.InquirySources = inquirySources;
            ViewBag.Activities = activities;
            ViewBag.ActionTypes = actionTypes;
            ViewBag.MasterStatuses = masterStatuses;
            ViewBag.Services = visaTypes;
            ViewBag.ExistingSettingsJson = JsonSerializer.Serialize(existing, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(string SettingsJson, string? DashboardSettingsUserType)
        {
            var settings = new List<DashboardSetting>();

            if (!string.IsNullOrWhiteSpace(SettingsJson))
            {
                settings = JsonSerializer.Deserialize<List<DashboardSetting>>(SettingsJson,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<DashboardSetting>();
            }

            foreach (var s in settings)
            {
                s.DashboardSettingsUserType = DashboardSettingsUserType;
            }

            _repo.SaveAllForUser(CurrentUserId, settings);

            TempData["SaveMessage"] = "Dashboard settings saved successfully.";
            return RedirectToAction("Index");
        }
    }
}