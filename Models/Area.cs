
namespace ActivitiesManagement.Models
{
    public class Area
    {
        public int Id { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public int StateId { get; set; }
        public string StateName { get; set; }
        public int CityId { get; set; }
        public string? CityName { get; set; }
        public string AreaName { get; set; }
        public string? Pincode { get; set; }
        public string StatusFlag { get; set; }

        public string StatusDispaly => StatusFlag == "A" ? "Active" : "InActive";
    }
}
