namespace Nest.Shared.Utils;

public class InternetChecker
{
    public static async Task<bool> IsInternetAvailable()
    {
        try
        {
            using (var httpClient = new HttpClient())
            {
                HttpResponseMessage response = await httpClient.GetAsync("http://one.one.one.one");
                return response.IsSuccessStatusCode;
            }
        }
        catch
        {
            return false;
        }
    }

    public static async Task<bool> IsServerAvailable(string url)
    {
        try
        {
            using (Ping pinger = new Ping())
            {
                PingReply reply = await pinger.SendPingAsync(url);
                return reply.Status == IPStatus.Success;
            }
        }
        catch
        {
            return false;
        }
    }
}