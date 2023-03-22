namespace tradeMarketPlace.Models;

public partial class ProductCatalogue
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int SubCategoryId { get; set; }

    public int ProductCategoryId { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreationDate { get; set; }

    public DateTime UpdatedOn { get; set; }

    public int UpdatedBy { get; set; }

    public int CreatedBy { get; set; }

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual ProductCategory ProductCategory { get; set; } = null!;

    // public virtual ICollection<Rfp> Rfps { get; } = new List<Rfp>();

    public virtual SubCategory SubCategory { get; set; } = null!;

    public virtual User UpdatedByNavigation { get; set; } = null!;

    public ICollection<Rfp>? Rfps { get; set; } = null!;
}
