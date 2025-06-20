namespace Nest.Shared.Utils;

public class InternetChecker
{
    public static async Task<bool> IsInternetAvailable()
    {
        try
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync("http://one.one.one.one");
                return response.IsSuccessStatusCode;
            }
        }
        catch
        {
            return false;
        }
    }

    public static bool PingServer(string serverAddress, int timeoutMs = 3000)
    {
        try
        {
            using var ping = new Ping();
            var reply = ping.Send(serverAddress, timeoutMs);
            return reply.Status == IPStatus.Success;
        }
        catch
        {
            return false;
        }
    }
}