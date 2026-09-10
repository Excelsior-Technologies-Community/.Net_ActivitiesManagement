namespace ActivitiesManagement.Models
{
    public class SubSourceOfInquiry
    {
        public long Id { get; set; }
        public long SourceOfInquiryId { get; set; }
        public string SourceOfInquiryTitle { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string StatusFlag { get; set; }
        public string StatusDisplay => StatusFlag == "A" ? "Active" : "InActive";
    }
}
