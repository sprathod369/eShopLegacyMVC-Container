using Microsoft.EntityFrameworkCore; 
using Microsoft.EntityFrameworkCore.Metadata.Builders; 
using eShopLegacyMVC_Core.Models; 
using Microsoft.Extensions.Logging; 
using Microsoft.AspNetCore.Identity;
// using System.Data.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore; 
namespace eShopLegacyMVC_Core.Models 
{ 
    public class CatalogDBContext : IdentityDbContext<IdentityUser>
    { 
        private readonly ILogger<CatalogDBContext> _logger; 
        public CatalogDBContext(DbContextOptions<CatalogDBContext> options, ILogger<CatalogDBContext> logger) 
            : base(options) 
        { 
            _logger = logger; 
        } 
        public DbSet<CatalogItem> CatalogItems { get; set; } 
        public DbSet<CatalogBrand> CatalogBrands { get; set; } 
        public DbSet<CatalogType> CatalogTypes { get; set; } 
        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        { 
            _logger.LogInformation("Configuring model..."); 
            ConfigureCatalogType(modelBuilder.Entity<CatalogType>()); 
            ConfigureCatalogBrand(modelBuilder.Entity<CatalogBrand>()); 
            ConfigureCatalogItem(modelBuilder.Entity<CatalogItem>()); 
            base.OnModelCreating(modelBuilder); 
        } 
        private void ConfigureCatalogType(EntityTypeBuilder<CatalogType> builder) 
        { 
            _logger.LogInformation("Configuring CatalogType entity..."); 
            builder.ToTable("CatalogType"); 
            builder.HasKey(ci => ci.Id); 
            builder.Property(ci => ci.Id) 
                .IsRequired(); 
            builder.Property(cb => cb.Type) 
                .IsRequired() 
                .HasMaxLength(100); 
        } 
        private void ConfigureCatalogBrand(EntityTypeBuilder<CatalogBrand> builder) 
        { 
            _logger.LogInformation("Configuring CatalogBrand entity..."); 
            builder.ToTable("CatalogBrand"); 
            builder.HasKey(ci => ci.Id); 
            builder.Property(ci => ci.Id) 
                .IsRequired(); 
            builder.Property(cb => cb.Brand) 
                .IsRequired() 
                .HasMaxLength(100); 
        } 
        private void ConfigureCatalogItem(EntityTypeBuilder<CatalogItem> builder) 
        { 
            _logger.LogInformation("Configuring CatalogItem entity..."); 
            builder.ToTable("Catalog"); 
            builder.HasKey(ci => ci.Id); 
            builder.Property(ci => ci.Id) 
                .ValueGeneratedNever() 
                .IsRequired(); 
            builder.Property(ci => ci.Name) 
                .IsRequired() 
                .HasMaxLength(50); 
            builder.Property(ci => ci.Price) 
                .IsRequired(); 
            builder.Property(ci => ci.PictureFileName) 
                .IsRequired(); 
            builder.Ignore(ci => ci.PictureUri); 
            builder.HasOne(ci => ci.CatalogBrand) 
                .WithMany() 
                .HasForeignKey(ci => ci.CatalogBrandId) 
                .IsRequired(); 
            builder.HasOne(ci => ci.CatalogType) 
                .WithMany() 
                .HasForeignKey(ci => ci.CatalogTypeId) 
                .IsRequired(); 
        } 
    } 
}