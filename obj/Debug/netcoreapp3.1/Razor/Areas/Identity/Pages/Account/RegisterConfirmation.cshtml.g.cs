#pragma checksum "C:\Users\saurabh.bhushan\Desktop\BMS\BMS\Areas\Identity\Pages\Account\RegisterConfirmation.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "92d48cce0b1ab6bf690880ebd5df7a2db75fa655"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Areas_Identity_Pages_Account_RegisterConfirmation), @"mvc.1.0.razor-page", @"/Areas/Identity/Pages/Account/RegisterConfirmation.cshtml")]
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
#line 1 "C:\Users\saurabh.bhushan\Desktop\BMS\BMS\Areas\Identity\Pages\_ViewImports.cshtml"
using Microsoft.AspNetCore.Identity;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\saurabh.bhushan\Desktop\BMS\BMS\Areas\Identity\Pages\_ViewImports.cshtml"
using BMS.Areas.Identity;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\saurabh.bhushan\Desktop\BMS\BMS\Areas\Identity\Pages\_ViewImports.cshtml"
using BMS.Areas.Identity.Pages;

#line default
#line hidden
#nullable disable
#nullable restore
#line 1 "C:\Users\saurabh.bhushan\Desktop\BMS\BMS\Areas\Identity\Pages\Account\_ViewImports.cshtml"
using BMS.Areas.Identity.Pages.Account;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"92d48cce0b1ab6bf690880ebd5df7a2db75fa655", @"/Areas/Identity/Pages/Account/RegisterConfirmation.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"2c6bd395ae404306e7f57282de4163f8b70db440", @"/Areas/Identity/Pages/_ViewImports.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"0ecd81195ad991dd63425e37e656694a480a94ba", @"/Areas/Identity/Pages/Account/_ViewImports.cshtml")]
    public class Areas_Identity_Pages_Account_RegisterConfirmation : global::Microsoft.AspNetCore.Mvc.RazorPages.Page
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 3 "C:\Users\saurabh.bhushan\Desktop\BMS\BMS\Areas\Identity\Pages\Account\RegisterConfirmation.cshtml"
  
    ViewData["Title"] = "Register confirmation";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<h1>");
#nullable restore
#line 7 "C:\Users\saurabh.bhushan\Desktop\BMS\BMS\Areas\Identity\Pages\Account\RegisterConfirmation.cshtml"
Write(ViewData["Title"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h1>\r\n");
#nullable restore
#line 8 "C:\Users\saurabh.bhushan\Desktop\BMS\BMS\Areas\Identity\Pages\Account\RegisterConfirmation.cshtml"
  
    if (@Model.DisplayConfirmAccountLink)
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("<p>\r\n");
            WriteLiteral(" <a id=\"confirm-link\"");
            BeginWriteAttribute("href", " href=\"", 419, "\"", 453, 1);
#nullable restore
#line 13 "C:\Users\saurabh.bhushan\Desktop\BMS\BMS\Areas\Identity\Pages\Account\RegisterConfirmation.cshtml"
WriteAttributeValue("", 426, Model.EmailConfirmationUrl, 426, 27, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">Click here to confirm your account</a>\r\n</p>\r\n");
#nullable restore
#line 15 "C:\Users\saurabh.bhushan\Desktop\BMS\BMS\Areas\Identity\Pages\Account\RegisterConfirmation.cshtml"
    }
    else
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("<p>\r\n        Please check your email to confirm your account.\r\n</p>\r\n");
#nullable restore
#line 21 "C:\Users\saurabh.bhushan\Desktop\BMS\BMS\Areas\Identity\Pages\Account\RegisterConfirmation.cshtml"
    }

#line default
#line hidden
#nullable disable
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<RegisterConfirmationModel> Html { get; private set; }
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<RegisterConfirmationModel> ViewData => (global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<RegisterConfirmationModel>)PageContext?.ViewData;
        public RegisterConfirmationModel Model => ViewData.Model;
    }
}
#pragma warning restore 1591
