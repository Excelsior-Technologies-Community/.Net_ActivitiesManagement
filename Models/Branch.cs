namespace ActivitiesManagement.Models
{
    public class Branch
    {
        public long Id { get; set; }
        public string BranchCode { get; set; } = string.Empty;
        public string BranchName { get; set; } = string.Empty;
        public bool IsHeadOffice { get; set; }
        public string? ContactPersonName { get; set; }
        public string? ContactNumber { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public long? CountryId { get; set; }
        public long? StateId { get; set; }
        public long? CityId { get; set; }
        public long? AreaId { get; set; }
        public string? Pincode { get; set; }
        public long? CompanyId { get; set; }
        public string? CompanyName { get; set; }
        public string? Description { get; set; }
        public string StatusFlag { get; set; } = "A";
        public string StatusDisplay => StatusFlag == "A" ? "Active" : "InActive";

        public List<long> SelectedSubBranchIds { get; set; } = new();
    }

    public class SubBranchOption
    {
        public long Id { get; set; }
        public string BranchName { get; set; } = string.Empty;
        public string BranchCode { get; set; } = string.Empty;
        public bool IsSelected { get; set; }
    }
}