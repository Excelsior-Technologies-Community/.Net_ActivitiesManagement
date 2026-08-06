namespace ActivitiesManagement.Models
{
    public class ExamProvider
    {
        public long Id { get; set; }
        public long ExamTypeId { get; set; }
        public string ExamTypeTitle { get; set; }
        public string Title { get; set; }
        public string Website { get; set; }
        public string Description {  get; set; }
        public string StatusFlag {  get; set; }
        public string StatusDisplay => StatusFlag == "A" ? "Active" : "InActive";
    }
}
