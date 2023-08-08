using Ignis.Components.Web;

namespace Ignis.Utils;

public static class Css
{
    public static string Class(params string?[] classes)
    {
        return string.Join(" ", classes).Trim();
    }

    public static string Class(params (string?, bool)[] classes)
    {
        return string.Join(" ", classes.Where(x => x.Item2).Select(x => x.Item1)).Trim();
    }

    public static string Class(string? always, params (string?, bool)[] classes)
    {
        return string.Join(" ", classes.Where(x => x.Item2).Select(x => x.Item1).Prepend(always)).Trim();
    }

    public static string Class(params ICssClass[] cssClass)
    {
        return string.Join(" ", cssClass.Select(x => x.CssClass)).Trim();
    }

    public static string Class(string? always, params ICssClass[] cssClass)
    {
        return string.Join(" ", cssClass.Select(x => x.CssClass).Prepend(always)).Trim();
    }
}
