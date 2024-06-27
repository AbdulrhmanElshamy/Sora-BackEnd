namespace Sofra.Api.Contracts.Favorite
{
    public record FavoriteRequest(IEnumerable<FavoriteItem> Items);
}
