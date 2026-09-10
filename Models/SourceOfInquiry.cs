namespace ActivitiesManagement.Models
{
    public class SourceOfInquiry
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string StatusFlag { get; set; }
        public string StatusDisplay => StatusFlag == "A" ? "Active" : "InActive";

        public bool IsLead { get; set; }
        public bool IsInquiry { get; set; }
        public bool IsRegistration { get; set; }
        public bool IsCoaching { get; set; }
        public bool IsProcess { get; set; }
    }
}
