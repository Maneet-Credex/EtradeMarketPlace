namespace tradeMarketPlace_Frontend.Models
{
    public class UserViewModel
    {
        public int UserId { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string OrganisationName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string? ContactNumber { get; set; }

        public string Type { get; set; } = null!;

        public string Status { get; set; }

        public DateTime? CreationDate { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public int? UpdatedBy { get; set; }

        public int? CreatedBy { get; set; }
    }
    public class UserInformation
    {
        public String UserName { get; set; }
        public String UserID { get; set; }
        public String UserEmail { get; set; }
        public String Role { get; set; }
    }
}
