using AutoMapper;
using ShopSharedLibrary.DBContextOperation.Models;
using ShopSharedLibrary.DTO_Operation.DTO;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ShopServer.Services.Extentions
{
    public static class ConfigureMappingExtention
    {
        public static IServiceCollection ConfigureMapping(this IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
            return services;
        }
    }

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            AllowNullDestinationValues = true;
            AllowNullCollections = true;

            CreateMap<Suppliers, SupplierDTO>()
                .ReverseMap();

            CreateMap<Users, UserDTO>()
               .ReverseMap();

            CreateMap<Orders, OrderDTO>()
               .ReverseMap();


            CreateMap<OrderItems, OrderItemsDTO>() // Bunlarda Sorun Olabilir.  6. Video DK 8
               .ReverseMap();
        }
    }
}
