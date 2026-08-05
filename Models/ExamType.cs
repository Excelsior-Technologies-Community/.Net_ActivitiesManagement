namespace ActivitiesManagement.Models
{
    public class ExamType
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public string IsLead { get; set; }
        public string IsInquiry { get; set; }
        public string IsRegistration { get; set; }
        public string IsCoaching { get; set; }
        public string IsProcess { get; set; }
        public string IsMock { get; set; }
        public string IsProfessional { get; set; }
        public string IsEnglishTest { get; set; }

        public string StatusFlag { get; set; }
        public string StatusDisplay => StatusFlag == "A" ? "Active" : "InActive";

        public List<ExamTypeDetail> GradeTitles { get; set; } = new List<ExamTypeDetail>();

        public bool IsLeadChecked => IsLead == "Y";
        public bool IsInquiryChecked => IsInquiry == "Y";
        public bool IsRegistrationChecked => IsRegistration == "Y";
        public bool IsCoachingChecked => IsCoaching == "Y";
        public bool IsProcessChecked => IsProcess == "Y";
        public bool IsMockChecked => IsMock == "Y";
        public bool IsProfessionalChecked => IsProfessional == "Y";
        public bool IsEnglishTestChecked => IsEnglishTest == "Y";
    }
}
