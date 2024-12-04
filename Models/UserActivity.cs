namespace EMS.Models
{
    public class UserActivity
    {
        public string? CreatedByID { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime ModifiedOn { get; set; }
    }

    public class ApprovalActivity
    {
        public string? ApprovedById { get; set; }

        public DateTime ApprovedOn { get; set; }
    }
}
