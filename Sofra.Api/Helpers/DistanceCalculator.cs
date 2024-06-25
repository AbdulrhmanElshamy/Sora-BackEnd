using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Sofra.Api.Helpers
{
    public class DistanceCalculator(IOptions<GoogleMapOptions> options)
    {
        private readonly static string BaseUrl = "https://maps.googleapis.com/maps/api/distancematrix/json";
        private readonly static string Key = "AIzaSyAYkhN7gr513MozaWiLvx9irJ5hfsU1FJk";


        public static async Task<double> GetDrivingDistanceAsync(double originLat, double originLon, double destinationLat, double destinationLon)
        {
            using (HttpClient client = new HttpClient())
            {
                string requestUrl = $"{BaseUrl}?origins={originLat},{originLon}&destinations={destinationLat},{destinationLon}&key={Key}";
                HttpResponseMessage response = await client.GetAsync(requestUrl);

               

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    var jsonResponse = JsonConvert.DeserializeObject<Response>(responseContent);

                    if (jsonResponse.Rows!= null && jsonResponse.Rows.Select(x => x.Elements).FirstOrDefault()!= null && jsonResponse.Rows.Select(x => x.Elements.Length).FirstOrDefault() != null)
                    {
                        double distanceInMeters = (double)jsonResponse.Rows.Select(x => x.Elements.Length).FirstOrDefault();
                        return distanceInMeters / 1000; 
                    }
                }

                throw new Exception("Failed to get driving distance.");
            }
        }

        public class Response
        {
            public string Status { get; set; }

            [JsonProperty(PropertyName = "origin_addresses")]
            public string[] OriginAddresses { get; set; }

            [JsonProperty(PropertyName = "destination_addresses")]
            public string[] DestinationAddresses { get; set; }

            public Row[] Rows { get; set; }

            public class Data
            {
                public int Value { get; set; }
                public string Text { get; set; }
            }

            public class Element
            {
                public string Status { get; set; }
                public Data Duration { get; set; }
                public Data Distance { get; set; }
            }

            public class Row
            {
                public Element[] Elements { get; set; }
            }
        }
    }
}
