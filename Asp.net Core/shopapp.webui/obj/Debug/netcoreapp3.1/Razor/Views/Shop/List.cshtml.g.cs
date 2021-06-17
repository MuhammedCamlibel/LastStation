#pragma checksum "C:\Users\Muhammed\Desktop\Yeni klasör\Asp.net Core\shopapp.webui\Views\Shop\List.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "e7fcbfd732da057a124b8b3a7f9cbdae4e174e38"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shop_List), @"mvc.1.0.view", @"/Views/Shop/List.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"e7fcbfd732da057a124b8b3a7f9cbdae4e174e38", @"/Views/Shop/List.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"886bf917adc6f3c9b85d77a4f36902e281ecabd3", @"/Views/_ViewImports.cshtml")]
    public class Views_Shop_List : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<ProductListViewModel>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n\r\n<div class=\"row\">\r\n    <div class=\"col-md-3\">\r\n        ");
#nullable restore
#line 6 "C:\Users\Muhammed\Desktop\Yeni klasör\Asp.net Core\shopapp.webui\Views\Shop\List.cshtml"
   Write(await Component.InvokeAsync("Categories"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n    </div>\r\n\r\n    <div class=\"col-md-9\">\r\n           \r\n        <div class=\"row\">\r\n\r\n\r\n");
#nullable restore
#line 14 "C:\Users\Muhammed\Desktop\Yeni klasör\Asp.net Core\shopapp.webui\Views\Shop\List.cshtml"
         foreach (var product in Model.Products)
         {

#line default
#line hidden
#nullable disable
            WriteLiteral("             <div class=\"col-md-4\">\r\n\r\n                ");
#nullable restore
#line 18 "C:\Users\Muhammed\Desktop\Yeni klasör\Asp.net Core\shopapp.webui\Views\Shop\List.cshtml"
           Write(await Html.PartialAsync("_product",product));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n\r\n              </div> \r\n");
#nullable restore
#line 22 "C:\Users\Muhammed\Desktop\Yeni klasör\Asp.net Core\shopapp.webui\Views\Shop\List.cshtml"
        }

#line default
#line hidden
#nullable disable
            WriteLiteral("        </div>\r\n\r\n        <div class=\"row\">\r\n            <div class=\"col\">\r\n                <nav aria-label=\"Page navigation example\">\r\n                    <ul class=\"pagination\">\r\n");
#nullable restore
#line 29 "C:\Users\Muhammed\Desktop\Yeni klasör\Asp.net Core\shopapp.webui\Views\Shop\List.cshtml"
                         for (int i = 0; i <@Model.PageInfo.TotalPages(); i++)

                        {
                            

#line default
#line hidden
#nullable disable
#nullable restore
#line 32 "C:\Users\Muhammed\Desktop\Yeni klasör\Asp.net Core\shopapp.webui\Views\Shop\List.cshtml"
                             if (string.IsNullOrEmpty(Model.PageInfo.CurrentCategory))
                            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                 <li");
            BeginWriteAttribute("class", " class=\"", 864, "\"", 928, 2);
            WriteAttributeValue("", 872, "page-item", 872, 9, true);
#nullable restore
#line 34 "C:\Users\Muhammed\Desktop\Yeni klasör\Asp.net Core\shopapp.webui\Views\Shop\List.cshtml"
WriteAttributeValue(" ", 881, Model.PageInfo.CurrentPage==i+1?"active":"", 882, 46, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral("><a class=\"page-link\"");
            BeginWriteAttribute("href", " href=\"", 950, "\"", 978, 2);
            WriteAttributeValue("", 957, "/products?page=", 957, 15, true);
#nullable restore
#line 34 "C:\Users\Muhammed\Desktop\Yeni klasör\Asp.net Core\shopapp.webui\Views\Shop\List.cshtml"
WriteAttributeValue("", 972, i+1, 972, 6, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">");
#nullable restore
#line 34 "C:\Users\Muhammed\Desktop\Yeni klasör\Asp.net Core\shopapp.webui\Views\Shop\List.cshtml"
                                                                                                                                                    Write(i+1);

#line default
#line hidden
#nullable disable
            WriteLiteral("</a></li>\r\n");
#nullable restore
#line 35 "C:\Users\Muhammed\Desktop\Yeni klasör\Asp.net Core\shopapp.webui\Views\Shop\List.cshtml"
                            }
                            else
                            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                <li");
            BeginWriteAttribute("class", " class=\"", 1128, "\"", 1192, 2);
            WriteAttributeValue("", 1136, "page-item", 1136, 9, true);
#nullable restore
#line 38 "C:\Users\Muhammed\Desktop\Yeni klasör\Asp.net Core\shopapp.webui\Views\Shop\List.cshtml"
WriteAttributeValue(" ", 1145, Model.PageInfo.CurrentPage==i+1?"active":"", 1146, 46, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral("><a class=\"page-link\"");
            BeginWriteAttribute("href", " href=\"", 1214, "\"", 1274, 4);
            WriteAttributeValue("", 1221, "/products/", 1221, 10, true);
#nullable restore
#line 38 "C:\Users\Muhammed\Desktop\Yeni klasör\Asp.net Core\shopapp.webui\Views\Shop\List.cshtml"
WriteAttributeValue("", 1231, Model.PageInfo.CurrentCategory, 1231, 31, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 1262, "?page=", 1262, 6, true);
#nullable restore
#line 38 "C:\Users\Muhammed\Desktop\Yeni klasör\Asp.net Core\shopapp.webui\Views\Shop\List.cshtml"
WriteAttributeValue("", 1268, i+1, 1268, 6, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">");
#nullable restore
#line 38 "C:\Users\Muhammed\Desktop\Yeni klasör\Asp.net Core\shopapp.webui\Views\Shop\List.cshtml"
                                                                                                                                                                                   Write(i+1);

#line default
#line hidden
#nullable disable
            WriteLiteral("</a></li>\r\n");
#nullable restore
#line 39 "C:\Users\Muhammed\Desktop\Yeni klasör\Asp.net Core\shopapp.webui\Views\Shop\List.cshtml"
                            }

#line default
#line hidden
#nullable disable
#nullable restore
#line 39 "C:\Users\Muhammed\Desktop\Yeni klasör\Asp.net Core\shopapp.webui\Views\Shop\List.cshtml"
                             
                           
                        }

#line default
#line hidden
#nullable disable
            WriteLiteral("                         \r\n                         \r\n                    </ul>\r\n                </nav>\r\n        </div>\r\n        </div>\r\n    </div>\r\n</div>\r\n\r\n\r\n    \r\n\r\n\r\n\r\n\r\n");
            DefineSection("Scripts", async() => {
                WriteLiteral("\r\n    \r\n    <script src=\"https://cdn.jsdelivr.net/npm/bootstrap@4.5.3/dist/js/bootstrap.bundle.min.js\" integrity=\"sha384-ho+j7jyWK8fNQe+A12Hb8AhRq26LrZ/JpcUGGOn+Y7RsweNrtN/tE3MoK7ZeZDyx\" crossorigin=\"anonymous\"></script>\r\n");
            }
            );
            WriteLiteral("\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<ProductListViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591