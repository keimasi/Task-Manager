namespace TaskManager.Services;

public class ImageHelper
{
    public static string GetAvatarUrl(string avatarFileName)
    {
        var baseUrl = "/Uploaded/images/";
        return string.IsNullOrEmpty(avatarFileName) ? "Empty" : $"{baseUrl}{avatarFileName}";
    }
}