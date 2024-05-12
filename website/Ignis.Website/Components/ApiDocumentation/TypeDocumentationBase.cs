using Doki;
using Ignis.Components;
using Ignis.Website.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Ignis.Website.Components.ApiDocumentation;

public abstract class TypeDocumentationBase : IgnisAsyncComponentBase, IHandleAfterRender
{
    private readonly Dictionary<string, AssemblyDocumentation> _assemblies = new(StringComparer.Ordinal);

    [Parameter, EditorRequired] public Type Type { get; set; } = null!;

    [Inject] public IStaticFileService StaticFileService { get; set; } = null!;

    // ReSharper disable once InconsistentNaming
    [Inject] public IJSRuntime JSRuntime { get; set; } = null!;

    [Inject] public NavigationManager Router { get; set; } = null!;

    protected AssemblyDocumentation AssemblyDocumentation { get; private set; } = null!;

    protected NamespaceDocumentation NamespaceDocumentation { get; private set; } = null!;

    protected TypeDocumentation TypeDocumentation { get; private set; } = null!;

    protected override async Task OnInitializedAsync(CancellationToken cancellationToken)
    {
        await LoadAssemblyDocumentationAsync(cancellationToken);
    }

    protected override async Task OnUpdateAsync(CancellationToken cancellationToken)
    {
        if (string.Equals(TypeDocumentation.FullName, Type.FullName, StringComparison.Ordinal)) return;

        var assemblyName = Type.Assembly.GetName().Name;
        if (string.Equals(AssemblyDocumentation.Name, assemblyName, StringComparison.Ordinal))
        {
            LoadTypeDocumentation();
            return;
        }

        await LoadAssemblyDocumentationAsync(cancellationToken);
    }

    private async Task LoadAssemblyDocumentationAsync(CancellationToken cancellationToken)
    {
        var assemblyName = Type.Assembly.GetName().Name!;

        if (_assemblies.TryGetValue(assemblyName, out var existing))
        {
            AssemblyDocumentation = existing;
            LoadTypeDocumentation();
            return;
        }

        var path = $"/_api/{assemblyName}.json";

        var assembly =
            await StaticFileService.GetFileContentAsJsonAsync<AssemblyDocumentation>(path, cancellationToken);
        if (assembly == null)
        {
            Router.NavigateTo("/");
            return;
        }

        AssemblyDocumentation = assembly;

        _assemblies[assemblyName] = assembly;

        LoadTypeDocumentation();
    }

    private void LoadTypeDocumentation()
    {
        var @namespace = AssemblyDocumentation.Namespaces.FirstOrDefault(n =>
            string.Equals(n.Name, Type.Namespace, StringComparison.Ordinal));
        if (@namespace == null)
        {
            Router.NavigateTo("/");
            return;
        }

        NamespaceDocumentation = @namespace;

        var type = NamespaceDocumentation.Types.FirstOrDefault(t =>
            string.Equals(t.Id, Type.FullName, StringComparison.Ordinal));
        if (type == null)
        {
            Router.NavigateTo("/");
            return;
        }

        TypeDocumentation = type;
    }

    public async Task OnAfterRenderAsync()
    {
        await JSRuntime.InvokeVoidAsync("OnPageLoad", cancellationToken: CancellationToken);
    }
}
