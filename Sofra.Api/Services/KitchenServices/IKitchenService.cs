using Sofra.Api.Abstractions;
using Sofra.Api.Contracts.Address;
using Sofra.Api.Contracts.Kitchen;
using Sofra.Api.Contracts.Meal;

namespace Sofra.Api.Services.KitchenServices
{
    public interface IKitchenService
    {
        Task<IEnumerable<KitchenResponse>> GetAllAsync(AddressRequest request,CancellationToken cancellationToken = default);

        Task<Result<KitchenResponse>> GetAsync(int Id, AddressRequest request, CancellationToken cancellationToken = default);

        Task<Result> SetAddress(AddressRequest request, CancellationToken cancellationToken = default);

        Task<Result> UpdateAsync(KitchenUpdateRequest request, CancellationToken cancellationToken = default);

        Task<Result> ToggleStatus(CancellationToken cancellationToken = default);
    }
}
