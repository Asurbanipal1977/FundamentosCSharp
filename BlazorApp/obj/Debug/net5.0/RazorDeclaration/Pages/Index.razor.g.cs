// <auto-generated/>
#pragma warning disable 1591
#pragma warning disable 0414
#pragma warning disable 0649
#pragma warning disable 0169

namespace BlazorApp.Pages
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
#nullable restore
#line 1 "C:\Users\mame1\source\repos\FundamentosCSharp\FundamentosCSharp\BlazorApp\_Imports.razor"
using System.Net.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\mame1\source\repos\FundamentosCSharp\FundamentosCSharp\BlazorApp\_Imports.razor"
using System.Net.Http.Json;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\mame1\source\repos\FundamentosCSharp\FundamentosCSharp\BlazorApp\_Imports.razor"
using Microsoft.AspNetCore.Components.Forms;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\Users\mame1\source\repos\FundamentosCSharp\FundamentosCSharp\BlazorApp\_Imports.razor"
using Microsoft.AspNetCore.Components.Routing;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "C:\Users\mame1\source\repos\FundamentosCSharp\FundamentosCSharp\BlazorApp\_Imports.razor"
using Microsoft.AspNetCore.Components.Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "C:\Users\mame1\source\repos\FundamentosCSharp\FundamentosCSharp\BlazorApp\_Imports.razor"
using Microsoft.AspNetCore.Components.Web.Virtualization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "C:\Users\mame1\source\repos\FundamentosCSharp\FundamentosCSharp\BlazorApp\_Imports.razor"
using Microsoft.AspNetCore.Components.WebAssembly.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "C:\Users\mame1\source\repos\FundamentosCSharp\FundamentosCSharp\BlazorApp\_Imports.razor"
using Microsoft.JSInterop;

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "C:\Users\mame1\source\repos\FundamentosCSharp\FundamentosCSharp\BlazorApp\_Imports.razor"
using BlazorApp;

#line default
#line hidden
#nullable disable
#nullable restore
#line 10 "C:\Users\mame1\source\repos\FundamentosCSharp\FundamentosCSharp\BlazorApp\_Imports.razor"
using BlazorApp.Shared;

#line default
#line hidden
#nullable disable
    [Microsoft.AspNetCore.Components.RouteAttribute("/")]
    public partial class Index : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
        }
        #pragma warning restore 1998
#nullable restore
#line 30 "C:\Users\mame1\source\repos\FundamentosCSharp\FundamentosCSharp\BlazorApp\Pages\Index.razor"
       
    private ICollection<Cerveza> resultCerveza;
    private ICollection<Post> resultPosts;   

    private swaggerClient client = new swaggerClient("https://localhost:7177", new HttpClient());
    private async Task Cervezas()
    {
        await jsRuntime.InvokeVoidAsync("inicializarDiv", "idCervezas", "Cargando...");
        resultCerveza = await client.CervezasAsync();
    }

    private async Task Peticiones()
    {
        await jsRuntime.InvokeVoidAsync("inicializarDiv", "idPeticiones", "Cargando...");
        resultPosts = await client.PeticionAsync();
    }

#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IJSRuntime jsRuntime { get; set; }
    }
}
#pragma warning restore 1591
