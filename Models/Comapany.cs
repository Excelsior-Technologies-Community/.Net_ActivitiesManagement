namespace ActivitiesManagement.Models
{
    public class Company
    {
        public long Id { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public string CompanyName { get; set; }
        public string ContactPerson1Name { get; set; }
        public string ContactPerson1Mobile { get; set; }
        public string ContactPerson2Name { get; set; }
        public string ContactPerson2Mobile { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string CompanyDescription { get; set; }
        public string CompanyMapLink { get; set; }
        public string CompanyLogo { get; set; } 

        public long? CountryId { get; set; }
        public string CountryName { get; set; }
        public long? StateId { get; set; }
        public string StateName { get; set; }
        public long? CityId { get; set; }
        public string CityName { get; set; }
        public long? AreaId { get; set; }
        public string Area { get; set; }

        public string Pincode { get; set; }
        public string Address { get; set; }
        public string TermAndConditions { get; set; }
        public string GSTNumber { get; set; }

        public string StatusFlag { get; set; }
        public string StatusDisplay => StatusFlag == "A" ? "Active" : "InActive";
    }

    
    public class LookupItem
    {
        public long Id { get; set; }
        public string Title { get; set; }
    }
}
