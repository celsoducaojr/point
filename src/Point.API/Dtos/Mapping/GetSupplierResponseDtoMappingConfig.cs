using Mapster;
using Point.Core.Domain.Entities;

namespace Point.API.Dtos.Mapping
{
    public class GetSupplierResponseDtoMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Supplier, GetSupplierResponseDto>()
                  .Map(dest => dest.Tags, 
                    src => src.Tags != null 
                        ? src.Tags.Select(t => t.TagId).ToList()
                        : new List<int>());
        }
    }
}
