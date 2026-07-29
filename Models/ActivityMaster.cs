namespace ActivitiesManagement.Models
{
    public class ActivityMaster
    {
        public long Id { get; set; }
        public long? ActivityId { get; set; }
        public string Title { get; set; }
        public string Amount { get; set; }  
        public string ActionTypeList { get; set; }
        public string StatusFlag { get; set; }
        public string InAppShow { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public List<long> SelectedActionTypeIds { get; set; } = new List<long>();

        public string ActionListDisplay { get; set; }
    }
}
