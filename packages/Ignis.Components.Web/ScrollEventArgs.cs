namespace Ignis.Components.Web;

public sealed class ScrollEventArgs
{
    public int ScrollX { get; }

    public int ScrollY { get; }

    internal ScrollEventArgs(int scrollX, int scrollY)
    {
        ScrollX = scrollX;
        ScrollY = scrollY;
    }
}
