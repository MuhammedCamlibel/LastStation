#pragma checksum "C:\Users\Muhammed\Desktop\Yeni klasör\Asp.net Core\shopapp.webui\Views\Cart\Detail.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "983e2ae23bf7b5e862200dff4f0517a350dd11fc"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Cart_Detail), @"mvc.1.0.view", @"/Views/Cart/Detail.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 2 "C:\Users\Muhammed\Desktop\Yeni klasör\Asp.net Core\shopapp.webui\Views\_ViewImports.cshtml"
using shopapp.entity;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\Muhammed\Desktop\Yeni klasör\Asp.net Core\shopapp.webui\Views\_ViewImports.cshtml"
using shopapp.webui.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\Users\Muhammed\Desktop\Yeni klasör\Asp.net Core\shopapp.webui\Views\_ViewImports.cshtml"
using Newtonsoft.Json;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "C:\Users\Muhammed\Desktop\Yeni klasör\Asp.net Core\shopapp.webui\Views\_ViewImports.cshtml"
using shopapp.webui.Extensions;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "C:\Users\Muhammed\Desktop\Yeni klasör\Asp.net Core\shopapp.webui\Views\_ViewImports.cshtml"
using Microsoft.AspNetCore.Identity;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "C:\Users\Muhammed\Desktop\Yeni klasör\Asp.net Core\shopapp.webui\Views\_ViewImports.cshtml"
using shopapp.webui.Identity;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"983e2ae23bf7b5e862200dff4f0517a350dd11fc", @"/Views/Cart/Detail.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"886bf917adc6f3c9b85d77a4f36902e281ecabd3", @"/Views/_ViewImports.cshtml")]
    public class Views_Cart_Detail : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<string>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n<div class=\"alert alert-warning\" role=\"alert\">\r\n  ");
#nullable restore
#line 4 "C:\Users\Muhammed\Desktop\Yeni klasör\Asp.net Core\shopapp.webui\Views\Cart\Detail.cshtml"
Write(Model);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
</div>
<hr>
<a  href=""/Cart/Index"">Sepete Dön</a>

<h2>Bize Ulaşın</h2>

<iframe src=""https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d3152.4449374948836!2d30.535252185130748!3d37.80304597303757!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x14c5b604e49939f3%3A0x9f4d230ba1a30554!2zSXNwYXJ0YSwgw4fDvG7DvHIsIDMyMjAwIElzcGFydGEgTWVya2V6L0lzcGFydGE!5e0!3m2!1str!2str!4v1623954731268!5m2!1str!2str"" width=""600"" height=""450"" style=""border:0;""");
            BeginWriteAttribute("allowfullscreen", " allowfullscreen=\"", 528, "\"", 546, 0);
            EndWriteAttribute();
            WriteLiteral(" loading=\"lazy\"></iframe>");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<string> Html { get; private set; }
    }
}
#pragma warning restore 1591
