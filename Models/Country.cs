namespace ActivitiesManagement.Models
{
    public class Country
    {
        public long ID { get; set; }
        public string CountryName { get; set; }
        public string? ShortCode { get; set; }
        public bool IsIntrested { get; set; }
        public bool IsPastRejection { get; set; }
        public bool IsInquiry { get; set; }
        public string? CountryFlagImage { get; set; }
        public string? StatusFlag { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}