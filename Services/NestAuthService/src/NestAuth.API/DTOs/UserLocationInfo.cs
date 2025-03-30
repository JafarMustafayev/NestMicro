using Newtonsoft.Json;

namespace NestAuth.API.DTOs;

public class UserLocationInfo
{
    [JsonProperty("country_name")]
    public string CountryName { get; set; }

    [JsonProperty("region_name")]
    public string RegionName { get; set; }

    [JsonProperty("city_name")]
    public string CityName { get; set; }

    [JsonProperty("latitude")]
    public double Latitude { get; set; }

    [JsonProperty("longitude")]
    public double Longitude { get; set; }

    [JsonProperty("time_zone")]
    public string TimeZone { get; set; }
}