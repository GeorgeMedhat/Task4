using Microsoft.EntityFrameworkCore;
using BusinessLogic.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations
{
    public class ProductConfigurations : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> product)
        {
            product.ToTable("Products", schema : "MasterSchema");
            
            product.HasKey(p => p.id);

            product.Property(c => c.title).IsRequired().HasMaxLength(50);

            product.Property(p => p.description).IsRequired(false).HasMaxLength(250);

            product.Property(p => p.author).IsRequired().HasMaxLength(50);

            product.Property(p => p.price).IsRequired().HasColumnName("BookPrice");//i dont know how to assign range in fluent api 

            product.HasOne(p => p.category).WithMany(c => c.products).HasForeignKey(b => b.id);//relationship
        }
    }
}
