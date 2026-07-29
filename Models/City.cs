namespace ActivitiesManagement.Models
{
    public class City
    {
        public int Id { get; set; }
        public int CountryId { get; set; }
        public string? CountryName { get; set; }
        public int StateId { get; set; }
        public string? StateName { get; set; }
        public string CityName { get; set; }
        public string? ShortCode { get; set; }
        public string StatusFlag { get; set; } = "A";

        public string StatusDisplay => StatusFlag == "A" ? "Active" : "InActive";
    }
}
