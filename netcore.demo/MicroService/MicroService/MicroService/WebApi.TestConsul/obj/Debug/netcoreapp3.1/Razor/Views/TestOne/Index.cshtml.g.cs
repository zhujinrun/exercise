#pragma checksum "C:\Users\lzx\source\repos\MicroService\WebApi.TestConsul\Views\TestOne\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "fcef0bb6d26ab84e01a6e0a4ce1c1c2906374820"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_TestOne_Index), @"mvc.1.0.view", @"/Views/TestOne/Index.cshtml")]
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
#line 1 "C:\Users\lzx\source\repos\MicroService\WebApi.TestConsul\Views\_ViewImports.cshtml"
using WebApi.TestConsul;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\lzx\source\repos\MicroService\WebApi.TestConsul\Views\_ViewImports.cshtml"
using WebApi.TestConsul.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"fcef0bb6d26ab84e01a6e0a4ce1c1c2906374820", @"/Views/TestOne/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"2709e41851d884eb7fae37821fda535d6dc0afe6", @"/Views/_ViewImports.cshtml")]
    public class Views_TestOne_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 1 "C:\Users\lzx\source\repos\MicroService\WebApi.TestConsul\Views\TestOne\Index.cshtml"
  
    ViewData["Title"] = "Index";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<h1>TestOne</h1>\r\n<h3>");
#nullable restore
#line 6 "C:\Users\lzx\source\repos\MicroService\WebApi.TestConsul\Views\TestOne\Index.cshtml"
Write(base.ViewBag.Url);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h3>\r\n\r\n\r\n\r\n    <h4>********************</h4>\r\n    <h4>");
#nullable restore
#line 11 "C:\Users\lzx\source\repos\MicroService\WebApi.TestConsul\Views\TestOne\Index.cshtml"
   Write(ViewBag.User.Account);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h4>\r\n    <h4>");
#nullable restore
#line 12 "C:\Users\lzx\source\repos\MicroService\WebApi.TestConsul\Views\TestOne\Index.cshtml"
   Write(ViewBag.User.Email);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h4>\r\n    <h4>");
#nullable restore
#line 13 "C:\Users\lzx\source\repos\MicroService\WebApi.TestConsul\Views\TestOne\Index.cshtml"
   Write(ViewBag.User.Name);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h4>\r\n\r\n<h3>");
#nullable restore
#line 15 "C:\Users\lzx\source\repos\MicroService\WebApi.TestConsul\Views\TestOne\Index.cshtml"
Write(base.ViewBag.Message);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h3>\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591