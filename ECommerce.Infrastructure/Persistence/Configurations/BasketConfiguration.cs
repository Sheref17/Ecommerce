    using ECommerce.Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace ECommerce.Infrastructure.Persistence.Configurations
    {
        public class BasketConfiguration: IEntityTypeConfiguration<Basket>
        {
            public void Configure(EntityTypeBuilder<Basket> builder)
            {
                builder.ToTable("Baskets");

                builder.HasKey(x => x.Id);

                builder.Property(x => x.UserId)
                    .IsRequired();

                builder.HasIndex(x => x.UserId)
                    .IsUnique();

                builder.HasMany(x => x.Items)
                    .WithOne()
                    .HasForeignKey("BasketId")
                    .OnDelete(DeleteBehavior.Cascade);
            }
        }
    }
