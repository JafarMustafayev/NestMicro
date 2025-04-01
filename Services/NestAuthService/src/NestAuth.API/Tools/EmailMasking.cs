namespace NestAuth.API.Tools;

public static class EmailMasking
{
    public static string Mask(string email)
    {
        if (string.IsNullOrEmpty(email) || !email.Contains("@"))
            return email;

        string[] parts = email.Split('@');
        string username = parts[0];
        string domain = parts[1];

        string maskedUsername = username.Length > 2
            ? username.Substring(0, 2) + new string('*', username.Length - 3) + username[^1]
            : username;

        string[] domainParts = domain.Split('.');
        string domainName = domainParts[0];
        string domainExtension = domainParts.Length > 1 ? "." + domainParts[1] : "";

        string maskedDomain = domainName.Length > 2
            ? domainName.Substring(0, 2) + new string('*', domainName.Length - 3) + domainName[^1]
            : domainName;

        return $"{maskedUsername}@{maskedDomain}{domainExtension}";
    }
}