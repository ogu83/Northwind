using AutoMapper;

namespace NorthwindApi;

public class MappingProfile: Profile
{
    public MappingProfile()
    {
        CreateMap<DbModels.Category, Models.Category>();
        CreateMap<DbModels.Product, Models.Product>();
        CreateMap<DbModels.Supplier, Models.Supplier>();
        CreateMap<DbModels.Order, Models.Order>();
        CreateMap<DbModels.OrderDetail, Models.OrderDetail>();
        CreateMap<DbModels.Customer, Models.Customer>();
    }
}