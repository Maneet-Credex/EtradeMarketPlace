namespace tradeMarketPlace.Models;

public partial class User
{
    public int UserId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string OrganisationName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? ContactNumber { get; set; }

    public string Type { get; set; } = null!;

    public string Status { get; set; } = "Inactive";

    public DateTime? CreationDate { get; set; } = DateTime.Now;

    public DateTime? UpdatedOn { get; set; } = DateTime.Now;

    public int? UpdatedBy { get; set; } = null;

    public int? CreatedBy { get; set; } = null;

    public string? Password { get; set; }
    public virtual ICollection<Bid>? CreatedBids { get; set; }
    public virtual ICollection<Bid>? UpdatedBids { get; set; }
    public ICollection<Rfp>? RfpCreatedByNavigations { get; set; }
    public ICollection<Rfp>? RfpUpdatedByNavigations { get; set; }
    public virtual User? CreatedByNavigation { get; set; }

    public virtual ICollection<User> InverseCreatedByNavigation { get; } = new List<User>();

    public virtual ICollection<User> InverseUpdatedByNavigation { get; } = new List<User>();

    public virtual User? UpdatedByNavigation { get; set; }
    public virtual ICollection<Message> MessagesSent { get; set; } = new List<Message>();

    public virtual ICollection<Message> MessagesReceived { get; set; } = new List<Message>();

    public virtual ICollection<PurchaseOrder>? PurchaseOrders { get; set; }

    public virtual ICollection<PurchaseOrder>? PurchaseOrdersCreatedByNavigation { get; set; }
    public virtual ICollection<PurchaseOrder>? PurchaseOrdersUpdatedByNavigation { get; set; }
    public virtual ICollection<SubCategory>? SubCategoriesCreated { get; set; }
    public virtual ICollection<ProductCatalogue>? ProductsCreated { get; set; }
    public virtual ICollection<ProductCatalogue>? ProductsUpdated { get; set; }

    public virtual ICollection<SubCategory>? SubCategoriesUpdated { get; set; }




    public virtual ICollection<PurchaseOrder>? PurchaseOrdersSeller { get; set; } = new List<PurchaseOrder>();




}

public class UserLoginResponse
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    public UserLoginInformation data { get; set; }
    public string token { get; set; }
}

public class UserLoginInformation
{
    public String UserName { get; set; }
    public String UserID { get; set; }
    public String UserEmail { get; set; }
    public String Role { get; set; }
}