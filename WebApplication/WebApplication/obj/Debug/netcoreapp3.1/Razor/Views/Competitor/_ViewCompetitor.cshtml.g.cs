#pragma checksum "C:\Users\reshi\Source\Repos\web2021apr_p01_t02\WebApplication\WebApplication\Views\Competitor\_ViewCompetitor.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "9ef0b031bbae5355cb6d1b3f55a978c67b5a7b0a"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Competitor__ViewCompetitor), @"mvc.1.0.view", @"/Views/Competitor/_ViewCompetitor.cshtml")]
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
#line 1 "C:\Users\reshi\Source\Repos\web2021apr_p01_t02\WebApplication\WebApplication\Views\_ViewImports.cshtml"
using WebApplication;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\reshi\Source\Repos\web2021apr_p01_t02\WebApplication\WebApplication\Views\_ViewImports.cshtml"
using WebApplication.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\Users\reshi\Source\Repos\web2021apr_p01_t02\WebApplication\WebApplication\Views\_ViewImports.cshtml"
using Microsoft.AspNetCore.Http;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"9ef0b031bbae5355cb6d1b3f55a978c67b5a7b0a", @"/Views/Competitor/_ViewCompetitor.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"fcac9b5938334f89264800ae18adab0319d63c0b", @"/Views/_ViewImports.cshtml")]
    public class Views_Competitor__ViewCompetitor : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<WebApplication.Models.Competitor>>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "C:\Users\reshi\Source\Repos\web2021apr_p01_t02\WebApplication\WebApplication\Views\Competitor\_ViewCompetitor.cshtml"
 if (Model.ToList().Count > 0)
            {

#line default
#line hidden
#nullable disable
            WriteLiteral(@"                <div class=""table-responsive"">
                    <table id=""viewCompetitor"" class=""table table-striped table-bordered"">
                        <thead class=""thead-dark"">
                            <tr>
                                <th>ID</th>
                                <th>Name</th>
                                <th>Salutation</th>
                                <th>Email Address</th>
                                <th></th>
                            </tr>
                        </thead>
                        <tbody>
");
#nullable restore
#line 17 "C:\Users\reshi\Source\Repos\web2021apr_p01_t02\WebApplication\WebApplication\Views\Competitor\_ViewCompetitor.cshtml"
                             foreach (var item in Model)
                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                    <tr>\r\n\r\n                    </tr>\r\n");
#nullable restore
#line 22 "C:\Users\reshi\Source\Repos\web2021apr_p01_t02\WebApplication\WebApplication\Views\Competitor\_ViewCompetitor.cshtml"
                }

#line default
#line hidden
#nullable disable
            WriteLiteral("                        </tbody>\r\n                    </table>\r\n                </div>\r\n");
#nullable restore
#line 26 "C:\Users\reshi\Source\Repos\web2021apr_p01_t02\WebApplication\WebApplication\Views\Competitor\_ViewCompetitor.cshtml"
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<WebApplication.Models.Competitor>> Html { get; private set; }
    }
}
#pragma warning restore 1591