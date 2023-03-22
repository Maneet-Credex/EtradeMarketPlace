using Microsoft.EntityFrameworkCore;


namespace tradeMarketPlace.Models;

public partial class TradeMarketPlaceContext : DbContext
{
    public TradeMarketPlaceContext()
    {
    }

    public TradeMarketPlaceContext(DbContextOptions<TradeMarketPlaceContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.\\SQLExpress;Database=Trade_Market_Place; Trusted_Connection=true;Integrated Security=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<User>(entity =>
        {

            entity.HasKey(e => e.UserId).HasName("PK__user__B9BE370FAD1578DA");

            entity.ToTable("user");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.ContactNumber)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("contact_number");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreationDate)
                .HasPrecision(0)
                .HasColumnName("creation_date");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("last_name");
            entity.Property(e => e.OrganisationName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("organisation_name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Status)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("status");
            entity.Property(e => e.Type)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("type");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.UpdatedOn)
                .HasPrecision(0)
                .HasColumnName("updated_on");

            entity.HasOne(d => d.CreatedByNavigation)
            .WithMany(p => p.InverseCreatedByNavigation)
            .HasForeignKey(d => d.CreatedBy)
            .HasConstraintName("FK_created_by");

            entity.HasOne(d => d.UpdatedByNavigation)
                .WithMany(p => p.InverseUpdatedByNavigation)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_updated_by");
            /*
                        entity.HasMany(d => d.RfpCreatedByNavigations)
                        .WithOne(p => p.CreatedByNavigation)
                        .HasForeignKey(d => d.CreatedBy)
                        .OnDelete(DeleteBehavior.ClientSetNull);
                        entity.HasMany(d => d.RfpUpdatedByNavigations)
                            .WithOne(p => p.UpdatedByNavigation)
                            .HasForeignKey(d => d.UpdatedBy)
                            .OnDelete(DeleteBehavior.ClientSetNull);
            */
        });

        modelBuilder.Entity<Bid>(entity =>
        {
            entity.HasKey(e => e.BidId).HasName("PK__bid__7D0E5C2ABE7297AC");

            entity.ToTable("bid");

            entity.HasKey(e => e.BidId);

            entity.Property(e => e.BidId).HasColumnName("bid_id");
            entity.Property(e => e.RfpId).HasColumnName("rfp_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.BidDateTime).HasColumnName("bid_date_time");
            entity.Property(e => e.Comments).HasColumnName("comments");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.CreationDate).HasColumnName("creation_date");
            entity.Property(e => e.UpdateOn).HasColumnName("update_on");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");


            entity.HasOne(d => d.CreatedByNavigation)
                .WithMany(p => p.CreatedBids)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_bid_created_by");

            entity.HasOne(d => d.UpdatedByNavigation)
                .WithMany(p => p.UpdatedBids)
                .HasForeignKey(d => d.UpdatedBy)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_bid_updated_by");


        });

        modelBuilder.Entity<Bid>()
        .ToTable(tb => tb.HasTrigger("trg_Bid_Update"));

        modelBuilder.Entity<ProductCategory>(entity =>
        {
            entity.ToTable("product_category");

            entity.HasKey(e => e.ProductCategoryId);

            entity.Property(e => e.ProductCategoryId).HasColumnName("product_category_id");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.CreationDate).HasColumnName("creation_date");
            entity.Property(e => e.UpdatedOn).HasColumnName("updated_on");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");


        });

        modelBuilder.Entity<SubCategory>(entity =>
        {
            entity.ToTable("sub_category");

            entity.Property(e => e.SubCategoryId)
                .HasColumnName("sub_category_id")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.Name)
                .HasColumnName("name")
                .IsRequired();

            entity.Property(e => e.ProductCategoryId)
                .HasColumnName("product_category_id")
                .IsRequired();

            entity.Property(e => e.CreationDate)
                .HasColumnName("creation_date")
                .HasColumnType("datetime")
                .IsRequired();

            entity.Property(e => e.UpdatedOn)
                .HasColumnName("updated_on")
                .HasColumnType("datetime")
                .IsRequired();

            entity.Property(e => e.UpdatedBy)
                .HasColumnName("updated_by")
                .IsRequired();

            entity.Property(e => e.CreatedBy)
                .HasColumnName("created_by")
                .IsRequired();

            entity.HasOne(d => d.CreatedByNavigation)
                .WithMany(p => p.SubCategoriesCreated)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_sub_categories_created_by");

            entity.HasOne(d => d.UpdatedByNavigation)
                .WithMany(p => p.SubCategoriesUpdated)
                .HasForeignKey(d => d.UpdatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_sub_categories_updated_by");

            entity.HasOne(d => d.ProductCategory)
                .WithMany(p => p.SubCategories)
                .HasForeignKey(d => d.ProductCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_sub_categories_product_category");
        });

        modelBuilder.Entity<ProductCatalogue>(entity =>
        {
            entity.ToTable("product_catalogue");

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.Name)
                .HasColumnName("name")
                .IsRequired();

            entity.Property(e => e.Description)
                .HasColumnName("description")
                .IsRequired();

            entity.Property(e => e.SubCategoryId)
                .HasColumnName("sub_category_id")
                .IsRequired();

            entity.Property(e => e.ProductCategoryId)
                .HasColumnName("product_category_id")
                .IsRequired();

            entity.Property(e => e.Status)
                .HasColumnName("status")
                .IsRequired();

            entity.Property(e => e.CreationDate)
                .HasColumnName("creation_date")
                .HasColumnType("datetime")
                .IsRequired();

            entity.Property(e => e.UpdatedOn)
                .HasColumnName("updated_on")
                .HasColumnType("datetime")
                .IsRequired();

            entity.Property(e => e.UpdatedBy)
                .HasColumnName("updated_by")
                .IsRequired();

            entity.Property(e => e.CreatedBy)
                .HasColumnName("created_by")
                .IsRequired();

            entity.HasOne(d => d.CreatedByNavigation)
                .WithMany(p => p.ProductsCreated)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_product_catalogues_created_by");

            entity.HasOne(d => d.UpdatedByNavigation)
                .WithMany(p => p.ProductsUpdated)
                .HasForeignKey(d => d.UpdatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_product_catalogues_updated_by");

            entity.HasOne(d => d.ProductCategory)
                .WithMany(p => p.ProductCatalogues)
                .HasForeignKey(d => d.ProductCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_product_catalogues_product_category");

            entity.HasOne(d => d.SubCategory)
                .WithMany(p => p.ProductCatalogues)
                .HasForeignKey(d => d.SubCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_product_catalogues_sub_category");
        });


        modelBuilder.Entity<Rfp>(entity =>
        {
            entity.Property(e => e.RfpId).HasColumnName("rfp_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.ProductCategoryId).HasColumnName("product_category_id");
            entity.Property(e => e.ProductSubCategoryId).HasColumnName("sub_category_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.MaxPrice).HasColumnName("max_price");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.CreationDate).HasColumnName("creation_date");
            entity.Property(e => e.UpdateOn).HasColumnName("update_on");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.RfpName).HasColumnName("rfp_name");
            entity.Property(e => e.RfpDescription).HasColumnName("rfp_description");
            entity.Property(e => e.LastDate).HasColumnName("last_date");



            entity.HasOne(d => d.CreatedByNavigation)
                .WithMany(p => p.RfpCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.UpdatedByNavigation)
                .WithMany(p => p.RfpUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Product)
                .WithMany(p => p.Rfps)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.ProductCategory)
                .WithMany(p => p.Rfps)
                .HasForeignKey(d => d.ProductCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.ProductSubCategory)
                .WithMany(p => p.Rfps)
                .HasForeignKey(d => d.ProductSubCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull);

        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.ToTable("messages");

            entity.Property(e => e.MessageId)
                .HasColumnName("message_id");

            entity.Property(e => e.SenderId)
                .HasColumnName("sender_id");

            entity.Property(e => e.ReciverId)
                .HasColumnName("reciver_id");

            entity.Property(e => e.MsgContent)
                .HasColumnName("msg_content")
                .IsRequired();

            entity.Property(e => e.Date)
                .HasColumnName("date");

            entity.Property(e => e.Status)
                .HasColumnName("status")
                .IsRequired();

            entity.Property(e => e.Attachment)
                .HasColumnName("attachment")
                .IsRequired();

            entity.HasOne(d => d.Reciver)
                .WithMany(p => p.MessagesReceived)
                .HasForeignKey(d => d.ReciverId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_messages_receiver");

            entity.HasOne(d => d.Sender)
                .WithMany(p => p.MessagesSent)
                .HasForeignKey(d => d.SenderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_messages_sender");
        });

        modelBuilder.Entity<PurchaseOrder>(entity =>
        {
            entity.ToTable("purchase_order");

            entity.Property(e => e.PurchaseOrderId)
                .HasColumnName("purchase_order_id");

            entity.Property(e => e.BidId)
                .HasColumnName("bid_id");

            entity.Property(e => e.BuyerId)
                .HasColumnName("buyer_id");

            entity.Property(e => e.SellerId)
                .HasColumnName("seller_id");

            entity.Property(e => e.Invoice)
                .HasColumnName("invoice");

            entity.Property(e => e.Quantity)
                .HasColumnName("quantity");

            entity.Property(e => e.Status)
                .HasColumnName("status")
                .IsRequired();

            entity.Property(e => e.CreationDate)
                .HasColumnName("creation_date");

            entity.Property(e => e.UpdatedOn)
                .HasColumnName("updated_on");

            entity.Property(e => e.CreatedBy)
                .HasColumnName("created_by");

            entity.Property(e => e.UpdatedBy)
                .HasColumnName("updated_by");

            entity.HasOne(d => d.Bid)
                .WithMany(p => p.PurchaseOrders)
                .HasForeignKey(d => d.BidId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_purchase_orders_bid");

            entity.HasOne(d => d.Buyer)
                .WithMany(p => p.PurchaseOrders)
                .HasForeignKey(d => d.BuyerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_purchase_orders_buyer");

            entity.HasOne(d => d.CreatedByNavigation)
                .WithMany(p => p.PurchaseOrdersCreatedByNavigation)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_purchase_orders_created_by");

            entity.HasOne(d => d.QuantityNavigation)
                .WithMany(p => p.PurchaseOrders)
                .HasForeignKey(d => d.Quantity)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_purchase_orders_rfp");

            entity.HasOne(d => d.Seller)
                .WithMany(p => p.PurchaseOrdersSeller)
                .HasForeignKey(d => d.SellerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_purchase_orders_seller");

            entity.HasOne(d => d.UpdatedByNavigation)
                .WithMany(p => p.PurchaseOrdersUpdatedByNavigation)
                .HasForeignKey(d => d.UpdatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_purchase_orders_updated_by");
        });



        // modelBuilder.Entity<Bid>()
        //.ForSqlServerHasTrigger(x => x.AfterInsert, "MyInsertTrigger");



        OnModelCreatingPartial(modelBuilder);
    }


    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);


    public DbSet<tradeMarketPlace.Models.BidHistory> BidHistory { get; set; }


    public DbSet<tradeMarketPlace.Models.Message> Message { get; set; }


    public DbSet<tradeMarketPlace.Models.Bid> Bid { get; set; }


    public DbSet<tradeMarketPlace.Models.ProductCategory> ProductCategory { get; set; }


    public DbSet<tradeMarketPlace.Models.Rfp> Rfp { get; set; }


    public DbSet<tradeMarketPlace.Models.SubCategory> SubCategory { get; set; }


    public DbSet<tradeMarketPlace.Models.PurchaseOrder> PurchaseOrder { get; set; }


    public DbSet<tradeMarketPlace.Models.ProductCatalogue>? ProductCatalogue { get; set; }
}