using BLL.DTO.MediaLink;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static BLL.DTO.Product.ProductUpdateDTO;

namespace BLL.DTO.ProductRegistration
{
    public class ProductRegistrationUpdateDTO
    {
        [Required] public ulong Id { get; set; }
        [Required] public ulong VendorId { get; set; }
        [Required] public ulong CategoryId { get; set; }

        [Required, StringLength(100)]
        public string ProposedProductCode { get; set; } = null!;

        [Required, StringLength(255)]
        public string ProposedProductName { get; set; } = null!;

        public string? Description { get; set; }
        public decimal UnitPrice { get; set; }
        [StringLength(10)] public string? EnergyEfficiencyRating { get; set; }

        public Dictionary<string, object> Specifications { get; set; } = new();
        //[StringLength(1000)] public string? ManualUrls { get; set; }
        //[StringLength(500)] public string? PublicUrl { get; set; }

        public int WarrantyMonths { get; set; } = 12;
        public decimal? WeightKg { get; set; }
        //public Dictionary<string, decimal> DimensionsCm { get; set; } = new();
        public required DimensionsDTO DimensionsCm { get; set; }


        public string? ManualUrl { get; set; }
        public string? ManualPublicUrl { get; set; }

        // Ảnh SP
        public List<MediaLinkItemDTO>? AddProductImages { get; set; }
        public List<string>? RemoveImagePublicIds { get; set; }

        // File chứng chỉ
        public List<MediaLinkItemDTO>? AddCertificateFiles { get; set; }
        public List<string>? RemoveCertificatePublicIds { get; set; }
    }
}
