namespace Ignis.Components.HeadlessUI;

public interface IOpenClose
{
    bool IsOpen { get; set; }

    void Open();

    void Close();
}
