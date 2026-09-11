namespace ActivitiesManagement.Models
{
    public class UserType
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string ShortName { get; set; }
        public string Description { get; set; }
        public bool IsShowAllRecord { get; set; }
        public string StatusFlag { get; set; }
        public string StatusDisplay => StatusFlag == "A" ? "Active" : "InActive";

    }
}