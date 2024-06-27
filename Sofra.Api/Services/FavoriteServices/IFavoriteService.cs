using Sofra.Api.Abstractions;
using Sofra.Api.Contracts.Favorite;

namespace Sofra.Api.Services.FavoriteServices
{
    public interface IFavoriteService
    {
        Task<Result<FavoriteResponse>> GetAsync(int id, CancellationToken cancellationToken = default!);

        Task<Result<FavoriteResponse>> AddAsync(FavoriteRequest request, CancellationToken cancellationToken = default!);
    }
}
