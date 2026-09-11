using System;
using System.ComponentModel.DataAnnotations;

namespace ActivitiesManagement.Models
{
    public class SecondarySourceOfEnquiry
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }
        public string? StatusFlag { get; set; }
         public string StatusDisplay => StatusFlag == "A" ? "Active" : "InActive";

        public long? CreateUser { get; set; }
        public long? UpdateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
