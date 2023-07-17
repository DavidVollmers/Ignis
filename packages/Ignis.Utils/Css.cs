namespace Ignis.Utils;

public static class Css
{
    public static string Class(params string[] classes)
    {
        return string.Join(" ", classes).Trim();
    }

    public static string Class(params (string, bool)[] classes)
    {
        return string.Join(" ", classes.Where(x => x.Item2).Select(x => x.Item1)).Trim();
    }

    public static string Class(string always, params (string, bool)[] classes)
    {
        return string.Join(" ", classes.Where(x => x.Item2).Select(x => x.Item1).Prepend(always)).Trim();
    }
}
