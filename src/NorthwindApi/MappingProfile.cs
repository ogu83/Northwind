using AutoMapper;

namespace NorthwindApi;

public class MappingProfile: Profile
{
    public MappingProfile()
    {
        CreateMap<DbModels.Category, Models.Category>();
        CreateMap<DbModels.Product, Models.Product>();
    }
}