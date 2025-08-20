using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DAL.Data.Models;
using DAL.Data;

namespace DAL.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");
        
        // Primary Key
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .HasColumnType("bigint unsigned")
            .ValueGeneratedOnAdd();
        
        // Required fields
        builder.Property(e => e.Email)
            .HasMaxLength(255)
            .IsRequired()
            .HasCharSet("utf8mb4")
            .UseCollation("utf8mb4_unicode_ci");
            
        builder.Property(e => e.PasswordHash)
            .HasMaxLength(255)
            .IsRequired()
            .HasCharSet("utf8mb4")
            .UseCollation("utf8mb4_unicode_ci")
            .HasColumnName("password_hash");
            
        builder.Property(e => e.FullName)
            .HasMaxLength(255)
            .IsRequired()
            .HasCharSet("utf8mb4")
            .UseCollation("utf8mb4_unicode_ci")
            .HasColumnName("full_name");
        
        // Enum conversions
        builder.Property(e => e.Role)
            .HasConversion<string>()
            .HasColumnType("enum('customer','seller','admin','manager')")
            .HasDefaultValue(UserRole.Customer);
        
        builder.Property(e => e.Status)
            .HasConversion<string>()
            .HasColumnType("enum('active','inactive','suspended','deleted')")
            .HasDefaultValue(UserStatus.Active);
        
        // Optional fields
        builder.Property(e => e.PhoneNumber)
            .HasMaxLength(20)
            .HasCharSet("utf8mb4")
            .UseCollation("utf8mb4_unicode_ci")
            .HasColumnName("phone_number");
            
        builder.Property(e => e.AvatarUrl)
            .HasMaxLength(500)
            .HasCharSet("utf8mb4")
            .UseCollation("utf8mb4_unicode_ci")
            .HasColumnName("avatar_url");
            
        builder.Property(e => e.VerificationToken)
            .HasMaxLength(255)
            .HasCharSet("utf8mb4")
            .UseCollation("utf8mb4_unicode_ci")
            .HasColumnName("verification_token");
        
        // Boolean defaults
        builder.Property(e => e.IsVerified)
            .HasDefaultValue(false)
            .HasColumnName("is_verified");
        
        // DateTime fields
        builder.Property(e => e.CreatedAt)
            .HasColumnType("timestamp")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .HasColumnName("created_at");
        
        builder.Property(e => e.UpdatedAt)
            .HasColumnType("timestamp")
            .HasDefaultValueSql("CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP")
            .HasColumnName("updated_at");
            
        builder.Property(e => e.LastLoginAt)
            .HasColumnType("timestamp")
            .HasColumnName("last_login_at");
            
        builder.Property(e => e.VerificationSentAt)
            .HasColumnType("timestamp")
            .HasColumnName("verification_sent_at");
            
        builder.Property(e => e.DeletedAt)
            .HasColumnType("timestamp")
            .HasColumnName("deleted_at");
        
        // Indexes
        builder.HasIndex(e => e.Email)
            .IsUnique()
            .HasDatabaseName("idx_email");
        
        builder.HasIndex(e => e.Role)
            .HasDatabaseName("idx_role");
            
        builder.HasIndex(e => e.Status)
            .HasDatabaseName("idx_status");
        
        builder.HasIndex(e => e.CreatedAt)
            .HasDatabaseName("idx_created_at");
    }
}
