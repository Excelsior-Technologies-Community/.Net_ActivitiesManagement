namespace ActivitiesManagement.Models
{
    public class ActionType
    {
        public long ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? StatusFlag { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
