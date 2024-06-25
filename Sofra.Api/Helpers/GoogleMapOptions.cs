using System.ComponentModel.DataAnnotations;

namespace Sofra.Api.Helpers
{
    public class GoogleMapOptions
    {
        public static string SectionName = "GoogleMap";

        [Required]
        public string Key { get; init; } = string.Empty;

        [Required]
        public string BaseUrl { get; init; } = string.Empty;
    }
}
