namespace ActivitiesManagement.Models
{
    public class Institute
    {
        public long Id { get; set; }
        public long InstituteTypeId { get; set; }
        public string InstituteTypeTitle { get; set; }
        public string InstituteName { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string Institutecode { get; set; }
        public string Pincode { get; set; }
        public string InstituteLogo { get; set; }
        public string Address { get; set; }
        public string Remarks { get; set; }
        public long CountryId { get; set; }
        public string CountryName { get; set; }
        public long StateId { get; set; }
        public string StateName { get; set; }
        public long CityId { get; set; }
        public string CityName { get; set; }
        public long? AreaId { get; set; }
        public string Area { get; set; }
        public string StatusFlag { get; set; }
        public string StatusDisplay => StatusFlag == "A" ? "Active" : "InActive";
    }
}
