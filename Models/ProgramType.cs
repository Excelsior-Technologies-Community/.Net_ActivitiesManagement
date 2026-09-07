namespace ActivitiesManagement.Models
{
    public class ProgramType
    {
        public long Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? ShortCode { get; set; }
        public string? Description { get; set; }
        public string StatusFlag { get; set; } = "A";
        public string StatusDisplay => StatusFlag == "A" ? "Active" : "InActive";
    }
}