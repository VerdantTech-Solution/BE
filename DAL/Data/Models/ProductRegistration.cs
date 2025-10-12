using System.ComponentModel.DataAnnotations;

namespace DAL.Data.Models;

/// <summary>
/// Product registration requests from vendors
/// </summary>
public partial class ProductRegistration
{
    public ulong Id { get; set; }

    public ulong VendorId { get; set; }

    public ulong CategoryId { get; set; }

    [Required]
    [StringLength(100)]
    public string ProposedProductCode { get; set; } = null!;

    [Required]
    [StringLength(255)]
    public string ProposedProductName { get; set; } = null!;

    public string? Description { get; set; }

    public decimal UnitPrice { get; set; }

    [StringLength(10)]
    public string? EnergyEfficiencyRating { get; set; }

    /// <summary>
    /// Technical specifications as key-value pairs (JSON)
    /// </summary>
    public Dictionary<string, object> Specifications { get; set; } = new();

    [StringLength(1000)]
    public string? ManualUrls { get; set; }

    /// <summary>
    /// Public URL for manual document access (v8.1)
    /// </summary>
    [StringLength(500)]
    public string? PublicUrl { get; set; }

    public int WarrantyMonths { get; set; } = 12;

    public decimal? WeightKg { get; set; }

    /// <summary>
    /// {length, width, height} (JSON)
    /// </summary>
    public Dictionary<string, decimal> DimensionsCm { get; set; } = new();

    public ProductRegistrationStatus Status { get; set; } = ProductRegistrationStatus.Pending;
    //public ProductRegistrationStatus Status { get; set; } 

    [StringLength(500)]
    public string? RejectionReason { get; set; }

    public ulong? ApprovedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? ApprovedAt { get; set; }

    // Navigation Properties
    public virtual User Vendor { get; set; } = null!;
    public virtual ProductCategory Category { get; set; } = null!;
    public virtual User? ApprovedByUser { get; set; }
}

