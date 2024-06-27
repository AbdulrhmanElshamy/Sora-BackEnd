namespace Sofra.Api.Contracts.Favorite
{
    public record FavoriteResponse(int Id,IEnumerable<FavoriteItem> Items);
}
