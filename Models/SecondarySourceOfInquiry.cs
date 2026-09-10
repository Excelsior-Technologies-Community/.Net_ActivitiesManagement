using System;

namespace ActivitiesManagement.Models
{
    public class SecondarySourceOfEnquiry
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string StatusFlag { get; set; }
        public long? CreateUser { get; set; }
        public long? UpdateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
