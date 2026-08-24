namespace ActivitiesManagement.Models
{
    public class Grade
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ShortCode { get; set; }
        public string Description { get; set; }
        public string StatusFlag { get; set; }
        public string StatusDispay => StatusFlag == "A" ? "Active" : "InActive";
    }
}
