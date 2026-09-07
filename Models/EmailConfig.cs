namespace ActivitiesManagement.Models
{
    public class EMailConfig
    {
        public int Id { get; set; }
        public string EmailHost { get; set; }
        public string EmailPort { get; set; }
        public string EmailUserName { get; set; }
        public string EmailPassword { get; set; }
        public bool EmailIsSSL { get; set; }
        public string EmailType { get; set; }
        public string StatusFlag { get; set; }
        public string StatusDisplay => StatusFlag == "A" ? "Active" : "InActive";
    }
}
