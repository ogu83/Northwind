using AutoMapper;

namespace NorthwindApi;

public class MappingProfile: Profile
{
    public MappingProfile()
    {
        CreateMap<DbModels.Category, Models.Category>().ReverseMap();
        CreateMap<DbModels.Product, Models.Product>().ReverseMap();
        CreateMap<DbModels.Supplier, Models.Supplier>().ReverseMap();
        CreateMap<DbModels.Order, Models.Order>().ReverseMap();
        CreateMap<DbModels.OrderDetail, Models.OrderDetail>().ReverseMap();
        CreateMap<DbModels.Customer, Models.Customer>().ReverseMap();
    }
}