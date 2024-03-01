using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using profe.webui.Entities;

namespace profe.webui.Data.Configurations
{
	public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.productName).IsRequired().HasMaxLength(100);

        }
    }
}

