using AutoMapper;
using BLL.DTO.Address;
using BLL.DTO.Cart;
using BLL.DTO.CO2;
using BLL.DTO.Courier;
using BLL.DTO.FarmProfile;
using BLL.DTO.ForumCategory;
using BLL.DTO.ForumComment;
using BLL.DTO.ForumPost;
using BLL.DTO.Order;
using BLL.DTO.ProductCategory;
using BLL.DTO.ProductRegistration;
using BLL.DTO.User;
using DAL.Data.Models;
using static BLL.DTO.Product.ProductUpdateDTO;

namespace BLL.Helpers;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        //Khi có 2 trường khác tên, ví dụ: studentName và Name
        //CreateMap<StudentDTO, Student>().ForMember(n => n.studentName, opt => opt.MapFrom(x => x.Name)).ReverseMap();

        //Khi muốn map tất cả ngoại trừ studentName
        //CreateMap<StudentDTO, Student>().ReverseMap().ForMember(n => n.studentName, opt => opt.Ignore());

        //Khi giá trị bị null
        //CreateMap<StudentDTO, Student>().ReverseMap()
        //.ForMember(n => n.Address, opt => opt.MapFrom(n => string.IsNullOrEmpty(n.Address) ? "No value found" : n.Address));

        // User mappings
        CreateMap<UserCreateDTO, User>().ReverseMap();
        CreateMap<StaffCreateDTO, User>().ReverseMap();
        CreateMap<UserUpdateDTO, User>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        CreateMap<UserResponseDTO, User>().ReverseMap()
            .ForMember(dest => dest.Addresses, opt => opt.MapFrom(src => src.UserAddresses.Select(ua => ua.Address)));
        CreateMap<UserAddressCreateDTO, Address>().ReverseMap();
        CreateMap<UserAddressUpdateDTO, Address>().ReverseMap();
        CreateMap<UserAddressUpdateDTO, UserAddress>().ReverseMap();

        // FarmProfile mappings
        CreateMap<FarmProfileCreateDto, FarmProfile>().ReverseMap();
        CreateMap<FarmProfile, FarmProfileResponseDTO>().ReverseMap();
        CreateMap<FarmProfileUpdateDTO, FarmProfile>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        // Address mappings
        CreateMap<Address, AddressResponseDTO>().ReverseMap();
        CreateMap<FarmProfileCreateDto, Address>().ReverseMap();
        CreateMap<FarmProfileUpdateDTO, Address>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        // CO2Footprint mappings
        CreateMap<Fertilizer, CO2FootprintCreateDTO>().ReverseMap();
        CreateMap<EnvironmentalDatum, CO2FootprintCreateDTO>().ReverseMap();
        CreateMap<EnergyUsage, CO2FootprintCreateDTO>().ReverseMap();
        CreateMap<EnvironmentalDatum, CO2FootprintResponseDTO>()
            .ForMember(dest => dest.EnergyUsage, opt => opt.MapFrom(src => src.EnergyUsage))
            .ForMember(dest => dest.Fertilizer, opt => opt.MapFrom(src => src.Fertilizer));
        CreateMap<EnergyUsageDTO, EnergyUsage>().ReverseMap();
        CreateMap<FertilizerDTO, Fertilizer>().ReverseMap();

        // ProductCategory mappings
        CreateMap<ProductCategoryCreateDTO, ProductCategory>().ReverseMap();
        CreateMap<ProductCategoryUpdateDTO, ProductCategory>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        CreateMap<ProductCategory, ProductCategoryResponseDTO>().ReverseMap();

        // Product mappings
        CreateMap<BLL.DTO.Product.ProductCreateDTO, Product>().ReverseMap();
        CreateMap<BLL.DTO.Product.ProductUpdateDTO, Product>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        CreateMap<Product, BLL.DTO.Product.ProductResponseDTO>().ReverseMap();

        // Cart mappings
        CreateMap<CartDTO, CartItem>().ReverseMap();
        CreateMap<DAL.Data.Models.Cart, CartResponseDTO>()
            .ForMember(d => d.UserInfo, o => o.MapFrom(s => s.Customer));
        CreateMap<CartItem, CartItemDTO>()
            .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product.ProductName))
            .ForMember(d => d.Slug, o => o.MapFrom(s => s.Product.Slug))
            .ForMember(d => d.Description, o => o.MapFrom(s => s.Product.Description))
            .ForMember(d => d.UnitPrice, o => o.MapFrom(s => s.Product.UnitPrice))
            .ForMember(d => d.IsActive, o => o.MapFrom(s => s.Product.IsActive))
            .ForMember(d => d.SoldCount, o => o.MapFrom(s => s.Product.SoldCount))
            .ForMember(d => d.RatingAverage, o => o.MapFrom(s => s.Product.RatingAverage));
        CreateMap<MediaLink, ImagesDTO>().ReverseMap();

        // Order mappings
        CreateMap<ProductResponseDTO, Product>().ReverseMap();
        CreateMap<RateResponseDTO, ShippingDetailDTO>().ReverseMap();
        CreateMap<OrderDetailResponseDTO, OrderDetail>().ReverseMap();
        CreateMap<OrderPreviewCreateDTO, OrderPreviewResponseDTO>().ReverseMap();
        CreateMap<DAL.Data.Models.Order, OrderPreviewResponseDTO>().ReverseMap();
        CreateMap<DAL.Data.Models.Order, OrderResponseDTO>().ReverseMap();
        CreateMap<OrderDetail, OrderDetailResponseDTO>().ReverseMap();
        CreateMap<Product, ProductResponseDTO>().ReverseMap();
        // Mapping cho ProductRegistration
        CreateMap<ProductRegistrationCreateDTO, ProductRegistration>()
            .ForMember(dest => dest.DimensionsCm, opt => opt.MapFrom(src => new Dictionary<string, object>
            {
                { "Width", src.DimensionsCm.Width },
                { "Height", src.DimensionsCm.Height },
                { "Length", src.DimensionsCm.Length }
            }));
        CreateMap<ProductRegistration, ProductRegistrationReponseDTO>()
            .ForMember(dest => dest.DimensionsCm, opt => opt.MapFrom(src => new DimensionsDTO
            {
                Width = src.DimensionsCm.ContainsKey("Width") ? Convert.ToDecimal(src.DimensionsCm["Width"]) : 0,
                Height = src.DimensionsCm.ContainsKey("Height") ? Convert.ToDecimal(src.DimensionsCm["Height"]) : 0,
                Length = src.DimensionsCm.ContainsKey("Length") ? Convert.ToDecimal(src.DimensionsCm["Length"]) : 0
            }));
        CreateMap<DAL.Data.Models.Order, OrderUpdateDTO>().ReverseMap();

        CreateMap<ProductRegistrationUpdateDTO, ProductRegistration>();

        // ForumCategory mappings
        CreateMap<ForumCategory, ForumCategoryResponseDTO>().ReverseMap();
        CreateMap<ForumCategoryCreateDTO, ForumCategory>().ReverseMap();
        CreateMap<ForumCategoryUpdateDTO, ForumCategory>().ReverseMap();


        // ForumPost mappings 
        CreateMap<ForumPostCreateDTO, ForumPost>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.CreatedAt, o => o.Ignore())
            .ForMember(d => d.UpdatedAt, o => o.Ignore())
            .ForMember(d => d.ViewCount, o => o.Ignore())
            .ForMember(d => d.LikeCount, o => o.Ignore())
            .ForMember(d => d.DislikeCount, o => o.Ignore());

        CreateMap<ForumPostUpdateDTO, ForumPost>()
            .ForMember(d => d.CreatedAt, o => o.Ignore())
            .ForMember(d => d.UpdatedAt, o => o.Ignore())
            .ForMember(d => d.ViewCount, o => o.Ignore())
            .ForMember(d => d.LikeCount, o => o.Ignore())
            .ForMember(d => d.DislikeCount, o => o.Ignore());

        CreateMap<ForumPost, ForumPostResponseDTO>();


        // ForumComment mappings 
        CreateMap<ForumCommentCreateDTO, ForumComment>()
            .ForMember(d => d.ParentId,
                o => o.MapFrom(s => s.ParentId.HasValue && s.ParentId.Value == 0 ? (ulong?)null : s.ParentId));

        CreateMap<ForumComment, ForumCommentResponseDTO>();
    }
}