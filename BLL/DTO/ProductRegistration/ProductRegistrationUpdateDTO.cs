using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        public Dictionary<string, decimal> DimensionsCm { get; set; } = new();


        //// MEDIA
        //public IFormFile? CoverImage { get; set; }
        //public List<IFormFile>? Images { get; set; } = new();

        //// Xoá ảnh theo publicId (nếu có)
        //public List<string>? RemovePublicIds { get; set; } = new();

        //// PDF manual (nếu muốn thay)
        //public IFormFile? ManualPdf { get; set; }

        // Upload (multipart/form-data)
        public IFormFile? ManualPdf { get; set; }         // thay manual
        public List<IFormFile>? Images { get; set; }      // thêm ảnh
        public List<string>? RemoveImagePublicIds { get; set; } // xóa ảnh theo public_id

    }
}
