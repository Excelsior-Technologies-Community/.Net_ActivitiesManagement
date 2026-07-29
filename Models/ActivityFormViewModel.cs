namespace ActivitiesManagement.Models
{
    public class ActivityFormViewModel
    {
        public long ID { get; set; }
        public string Title { get; set; }
        public bool InAppShow { get; set; }
        public string StatusFlag { get; set; }
        public List<ActivityDetailRow> Details { get; set; }
    }
    public class ActivityDetailRow
    {
        public long ID { get; set; }
        public string Title { get; set; }
        public long ActionTypeId { get; set; }
        public string ActionTypeTitle { get; set; }
        public string MasterName { get; set; }
        public bool IsInAppVisible { get; set; }
        public string StatusFlag { get; set; }
    }
}
