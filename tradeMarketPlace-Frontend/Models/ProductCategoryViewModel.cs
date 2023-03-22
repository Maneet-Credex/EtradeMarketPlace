namespace tradeMarketPlace_Frontend.Models
{
    public class ProductCategoryViewModel
    {
        public int ProductCategoryId { get; set; }
        public int ProductSubCategoryId { get; set; }
        public int ProductId { get; set; }

        public string Name { get; set; } = null!;

        public DateTime CreationDate { get; set; }

        public DateTime UpdatedOn { get; set; }

        public int UpdatedBy { get; set; }

        public int CreatedBy { get; set; }
    }
}
