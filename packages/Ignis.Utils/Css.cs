namespace Ignis.Utils;

public static class Css
{
    public static string Class(params string[] classes)
    {
        return string.Join(" ", classes).Trim();
    }
}
