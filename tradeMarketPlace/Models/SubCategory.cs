namespace tradeMarketPlace.Models;

public partial class SubCategory
{
    public int SubCategoryId { get; set; }

    public string Name { get; set; } = null!;

    public int ProductCategoryId { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime UpdatedOn { get; set; }

    public int UpdatedBy { get; set; }

    public int CreatedBy { get; set; }

    public virtual User? CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<ProductCatalogue>? ProductCatalogues { get; } = new List<ProductCatalogue>();

    public virtual ProductCategory? ProductCategory { get; set; } = null!;

    public virtual User? UpdatedByNavigation { get; set; } = null!;

    public ICollection<Rfp>? Rfps { get; set; } = null!;
}
