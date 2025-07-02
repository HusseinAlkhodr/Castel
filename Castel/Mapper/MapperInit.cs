using Castel.DTO.Pagination;
using Castel.DTO.Result;
using Castel.Models.Authentication;
using Castel.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using AutoMapper;

namespace Castel.Mapper
{
    public class MapperInit : Profile
    {
        public MapperInit()
        {
            CreateMap<Dictionary<string, string>, string>().ConvertUsing((src, dest) => JsonConvert.SerializeObject(src));
            CreateMap<DateTime, string>().ConvertUsing(src => src.ToString("yyyy-MM-dd HH:mm"));
            CreateMap<DateTime?, string>().ConvertUsing(src => src == null ? "" : src.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm"));
            CreateMap<DateTime?, string?>().ConvertUsing(src => src == null ? null : src.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm"));
            CreateMap(typeof(PagedList<>), typeof(PagedList<>));
            CreateMap(typeof(ListItem<>), typeof(ListItem<>));

            CreateMap<StoreRole, GetRoleDTO>();
            CreateMap<StoreUser, GetUserDTO>()
                .ForMember(dest => dest.Roles, opt =>
                    opt.MapFrom(src =>
                        (src.UserRoles == null)
                            ? null
                            : src.UserRoles.Select(x => x.Role)));

            CreateMap<ItemDTO, Item>().ReverseMap();
            CreateMap<Invoice, Item>().ReverseMap();
            CreateMap<Invoice, GetInvoice>().ReverseMap();
            CreateMap<AddItemDTO, Item>().ReverseMap();
            CreateMap<UpdateItemDTO, Item>().ReverseMap();
            CreateMap<Item, GetItemDTO>().ReverseMap();
            CreateMap<Item, DeleteItemDTO>().ReverseMap();
            CreateMap<Item, EditPrice>().ReverseMap();


            CreateMap<DivisionDTO, Division>().ReverseMap();
            CreateMap<AddDivisionDTO, Division>()
                .ForMember(x => x.CreatedById, opt => opt.MapFrom<CurrentAccountId_IValueResolver>())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
            CreateMap<UpdateDivisionDTO, Division>()
                .ForMember(dest => dest.UpdatedById, opt => opt.MapFrom<CurrentAccountId_IValueResolver>())
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
            CreateMap<Division, GetDivisionDTO>().ReverseMap();

            CreateMap<VendorDTO, Vendor>().ReverseMap();
            CreateMap<AddVendorDTO, Vendor>()
                .ForMember(x => x.CreatedById, opt => opt.MapFrom<CurrentAccountId_IValueResolver>())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
            CreateMap<UpdateVendorDTO, Vendor>()
                .ForMember(dest => dest.UpdatedById, opt => opt.MapFrom<CurrentAccountId_IValueResolver>())
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
            CreateMap<Vendor, GetVendorDTO>().ReverseMap();
        }
    }
}
