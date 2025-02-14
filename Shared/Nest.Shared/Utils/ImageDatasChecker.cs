namespace Nest.Shared.Utils;

public static class ImageDatasChecker
{
    public static bool CheckImageType(IFormFile file)
    {
        return file.ContentType.Contains("image");
    }

    public static bool CheckImageSize(IFormFile file)
    {
        return file.Length <= 5000000;
    }
}