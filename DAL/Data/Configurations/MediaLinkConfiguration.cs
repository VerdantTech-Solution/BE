using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DAL.Data.Models;
using DAL.Data.Converters;

namespace DAL.Data.Configurations;

public class MediaLinkConfiguration : IEntityTypeConfiguration<MediaLink>
{
    public void Configure(EntityTypeBuilder<MediaLink> builder)
    {
        builder.ToTable("media_links");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
               .HasColumnType("bigint unsigned")
               .HasColumnName("id")
               .ValueGeneratedOnAdd();

        // DÙNG ValueConverter -> string; cột DB là ENUM string
        builder.Property(e => e.OwnerType)
               .HasConversion(MediaLinkConverters.OwnerTypeToString)
               .HasColumnType("enum('vendor_certificates','chatbot_messages','products','product_registrations','product_certificates','product_reviews','forum_posts')")
               .IsRequired()
               .HasColumnName("owner_type");

        builder.Property(e => e.OwnerId)
               .HasColumnType("bigint unsigned")
               .IsRequired()
               .HasColumnName("owner_id");

        builder.Property(e => e.ImageUrl)
               .HasMaxLength(1024)
               .IsRequired()
               .HasCharSet("utf8mb4")
               .UseCollation("utf8mb4_unicode_ci")
               .HasColumnName("image_url");

        builder.Property(e => e.ImagePublicId)
               .HasMaxLength(512)
               .HasCharSet("utf8mb4")
               .UseCollation("utf8mb4_unicode_ci")
               .HasColumnName("image_public_id");

        builder.Property(e => e.Purpose)
               .HasConversion(MediaLinkConverters.PurposeToString)
               .HasColumnType("enum('front','back','none')")
               .HasDefaultValue(MediaPurpose.None)
               .HasColumnName("purpose");

        builder.Property(e => e.SortOrder)
               .HasColumnType("int")
               .HasDefaultValue(0)
               .HasColumnName("sort_order");

        builder.Property(e => e.CreatedAt)
               .HasColumnType("timestamp")
               .HasDefaultValueSql("CURRENT_TIMESTAMP")
               .HasColumnName("created_at");

        builder.Property(e => e.UpdatedAt)
               .HasColumnType("timestamp")
               .HasDefaultValueSql("CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP")
               .HasColumnName("updated_at");

        builder.HasIndex(e => new { e.OwnerType, e.OwnerId }).HasDatabaseName("idx_owner");
        builder.HasIndex(e => e.Purpose).HasDatabaseName("idx_purpose");
    }
}
