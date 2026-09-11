namespace ActivitiesManagement.Models
{
    public class DashboardSetting
    {
        public long Id { get; set; }
        public string? DashboardSettingsUserType { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string? Action { get; set; }
        public string? ActionType { get; set; }
        public string? ActionValue { get; set; }
    }
}