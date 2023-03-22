namespace tradeMarketPlace.Models;

public partial class ProductCategory
{
    public int ProductCategoryId { get; set; }

    public string Name { get; set; } = null!;

    public DateTime CreationDate { get; set; }

    public DateTime UpdatedOn { get; set; }

    public int UpdatedBy { get; set; }

    public int CreatedBy { get; set; }

    public virtual ICollection<ProductCatalogue> ProductCatalogues { get; } = new List<ProductCatalogue>();

    public virtual ICollection<SubCategory> SubCategories { get; } = new List<SubCategory>();

    public ICollection<Rfp>? Rfps { get; set; } = null!;
}
