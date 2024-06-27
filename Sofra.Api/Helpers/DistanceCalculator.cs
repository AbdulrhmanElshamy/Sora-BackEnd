using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Sofra.Api.Helpers
{
    public class DistanceCalculator(IOptions<GoogleMapOptions> options)
    {
        private readonly static string BaseUrl = "https://distance-calculator.p.rapidapi.com";
        private readonly static string Key = "dde95cf623mshca5e69f315d7d77p16b90djsnb282727a0111";


        public static async Task<double> GetDrivingDistanceAsync(double originLat, double originLon, double destinationLat, double destinationLon)
        {
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"{BaseUrl}//distance/simple"),
                    Headers =
                    {
                        { "x-rapidapi-key", Key },
                        { "x-rapidapi-host", "distance-calculator8.p.rapidapi.com" },
                    },
                };


                request.RequestUri = new Uri($"{request.RequestUri}?lat_1={originLat}&long_1={originLon}&lat_2={destinationLat}&long_2={destinationLon}");

                var response = await client.SendAsync(request);

                response.EnsureSuccessStatusCode();
                var jsonString = await response.Content.ReadAsStringAsync();

                JObject jsonResponse = JObject.Parse(jsonString);

                double distanceKm = (double)jsonResponse["distance"];

                return distanceKm;
            }
        }
    }
}
