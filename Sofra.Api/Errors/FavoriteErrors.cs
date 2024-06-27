using Sofra.Api.Abstractions;

namespace Sofra.Api.Errors
{
    public static class FavoriteErrors
    {
        public static readonly Error FavoriteNotFound =
        new("Favorite.NotFound", "No Favorite was found with the given ID", StatusCodes.Status404NotFound);
    }
}
