namespace ActivitiesManagement.Models
{
    public class Currency
    {
        public int Id { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string StatusFlag { get; set; }
        public string StatusDisplay => StatusFlag == "A" ? "Active" : "InActive";
    }
}
