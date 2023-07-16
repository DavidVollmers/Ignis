namespace Ignis.Components.HeadlessUI;

public interface IOpenClose
{
    bool IsOpen { get; }

    void Open();

    void Close();
}
