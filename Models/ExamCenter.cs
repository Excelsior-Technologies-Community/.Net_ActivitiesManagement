namespace ActivitiesManagement.Models
{
    public class ExamCenter
    {
        public int Id { get; set; }
        public long? ExamTypeId { get; set; }
        public long ExamProviderId { get; set; }
        public string ExamProviderTitle { get; set; }
        public string ExamCenterName { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string Address { get; set; }
        public long CountryId { get; set; }
        public long StateId { get; set; }
        public long CityId { get; set; }
        public long AreaId { get; set; }
        public string Pincode { get; set; }
        public string StatusFlag { get; set; }
        public string StatusDisplay => StatusFlag == "A" ? "Active" : "InActive";
    }
}