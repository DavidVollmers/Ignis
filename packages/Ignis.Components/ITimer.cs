namespace Ignis.Components;

// Substitute for System.Threading.ITimer
// https://learn.microsoft.com/en-us/dotnet/api/system.threading.itimer?view=net-8.0
#if !NET8_0
internal interface ITimer : IDisposable
{
}
#endif
