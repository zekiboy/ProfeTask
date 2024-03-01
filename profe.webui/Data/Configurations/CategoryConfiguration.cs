using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using profe.webui.Entities;

namespace profe.webui.Data.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.categoryName).IsRequired().HasMaxLength(100);

        }
    }
}