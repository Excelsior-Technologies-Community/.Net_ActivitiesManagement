namespace ActivitiesManagement.Models
{
    public class QuestionMaster
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string AnsType { get; set; }
        public long? PageMasterId { get; set; }
        public string StatusFlag { get; set; }
        public string StatusDisplay => StatusFlag == "A" ? "Active" : "InActive";
    }

    public class PageMasterLookup
    {
        public long Id { get; set; }
        public string Title { get; set; }
    }
}
