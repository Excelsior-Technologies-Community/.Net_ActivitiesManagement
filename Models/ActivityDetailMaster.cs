namespace ActivitiesManagement.Models
{
    public class ActivityDetailMaster
    {
        public long ID { get; set; }
        public long ActivityId { get; set; }
        public string ActivityTitle { get; set; }
        public string Title { get; set; }
        public long ActionTypeId { get; set; }
        public string ActionTypeTitle { get; set; }

        public string ActionIsMarkAsStatusVal { get; set; }
        public string ActionIsMarkAsStatusText { get; set; }
        public long? ActionIsMarkAsStatusId { get; set; }

        public string NewActionIsMarkAsStatusId { get; set; }

        public string PageMaster {  get; set; }
        public string StatusFlag { get; set; }
    }
    public class StatusOption
    {
        public string Id { get; set; }
        public string Text { get; set; }
    }
}
