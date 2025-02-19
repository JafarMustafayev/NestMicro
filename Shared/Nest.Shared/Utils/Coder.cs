namespace Nest.Shared.Utils;

public static class Coder
{
    public static string Encode(this string value)
    {
        try
        {
            byte[] bytes = Encoding.UTF8.GetBytes(value);
            return WebEncoders.Base64UrlEncode(bytes);
        }
        catch (Exception)
        {
            throw new OperationFailedException();
        }
    }

    public static string Decode(this string value)
    {
        try
        {
            byte[] bytes = WebEncoders.Base64UrlDecode(value);
            return Encoding.UTF8.GetString(bytes);
        }
        catch (Exception)
        {
            throw new OperationFailedException();
        }
    }
}