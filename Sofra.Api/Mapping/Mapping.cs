using Mapster;
using Sofra.Api.Contracts.Kitchen;
using Sofra.Api.Contracts.Meal;
using Sofra.Api.Models;

namespace Sofra.Api.Mapping
{
    public class Mapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {

            config
               .NewConfig<Meal, MealResponse>()
               .Map(dest => dest.MealPhotos , src => src.MealPhotos.Select(m => m.Image).ToList());

            config
               .NewConfig<Kitchen, KitchenResponse>()
               .Map(dest => dest.Latitude, src => src.Address.Latitude)
               .Map(dest => dest.Longitude, src => src.Address.Longitude);

        }
    }
}
