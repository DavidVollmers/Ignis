using System.Reflection;
using Ignis.Components;
using Ignis.Website.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Ignis.Website.Components.ApiDocumentation;

public abstract class TypeDocumentationBase : IgnisAsyncComponentBase, IHandleAfterRender
{
    [Parameter, EditorRequired] public TypeInfo Type { get; set; } = null!;

    [Inject] public IStaticFileService StaticFileService { get; set; } = null!;

    // ReSharper disable once InconsistentNaming
    [Inject] public IJSRuntime JSRuntime { get; set; } = null!;

    protected override async Task OnInitializedAsync(CancellationToken cancellationToken)
    {
        var path = $"/_api/{Type.Assembly.GetName().Name}.xml";

        var xml = await StaticFileService.GetFileContentAsXmlAsync(path, cancellationToken);
    }

    public async Task OnAfterRenderAsync()
    {
        await JSRuntime.InvokeVoidAsync("OnPageLoad");
    }
}
