namespace ActivitiesManagement.Models
{
    public class MasterStatus
    {
        public int Id { get; set; }
        public string StatusCode { get; set; }
        public string TitleStatusCode { get; set; } 
        public string Title { get; set; }
        public string Description { get; set; }
        public string StatusFlag { get; set; }
        public string StatusDisplay => StatusFlag == "A" ? "Active" : "InActive";
    }
}
