#pragma checksum "D:\sch\2.1\web\assignment\YES\WebApplication\WebApplication\Views\Competitor\ViewCompetitionCriteria.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "740fa5bea59cacc594057296f44444b9153a798a"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Competitor_ViewCompetitionCriteria), @"mvc.1.0.view", @"/Views/Competitor/ViewCompetitionCriteria.cshtml")]
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
#line 1 "D:\sch\2.1\web\assignment\YES\WebApplication\WebApplication\Views\_ViewImports.cshtml"
using WebApplication;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\sch\2.1\web\assignment\YES\WebApplication\WebApplication\Views\_ViewImports.cshtml"
using WebApplication.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "D:\sch\2.1\web\assignment\YES\WebApplication\WebApplication\Views\_ViewImports.cshtml"
using Microsoft.AspNetCore.Http;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"740fa5bea59cacc594057296f44444b9153a798a", @"/Views/Competitor/ViewCompetitionCriteria.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"fcac9b5938334f89264800ae18adab0319d63c0b", @"/Views/_ViewImports.cshtml")]
    public class Views_Competitor_ViewCompetitionCriteria : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<WebApplication.Models.CompetitionCriteriaViewModel>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "ViewCompetitionCriteria", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-controller", "Competitor", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "JoinCompetition", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "D:\sch\2.1\web\assignment\YES\WebApplication\WebApplication\Views\Competitor\ViewCompetitionCriteria.cshtml"
  
    ViewData["Title"] = "View Competition Criteria";

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
<h4 class=""PageTitle"">Join a Competition</h4>

<!-- Display a list of Competitions -->
<div class=""table-responsive"">
    <table id=""viewCompetition"" class=""table table-striped table-bordered"">
        <thead class=""thead-dark"">
            <tr>
                <th>ID</th>
                <th>Name</th>
                <th>Area of Interest</th>
                <th>Start Date</th>
                <th>End Date</th>
                <th>Result Released Date</th>
                <th></th>
            </tr>
        </thead>
        <tbody>
");
#nullable restore
#line 24 "D:\sch\2.1\web\assignment\YES\WebApplication\WebApplication\Views\Competitor\ViewCompetitionCriteria.cshtml"
             foreach (var item in Model.competitionList)
            {
                string selectedRow = "";
                if (ViewData["selectedCompetitionNo"].ToString()
                 == item.CompetitionID.ToString())
                {
                    // Highlight the selected row
                    selectedRow = "class='table-primary'";
                }

#line default
#line hidden
#nullable disable
            WriteLiteral("                <tr ");
#nullable restore
#line 33 "D:\sch\2.1\web\assignment\YES\WebApplication\WebApplication\Views\Competitor\ViewCompetitionCriteria.cshtml"
               Write(Html.Raw(selectedRow));

#line default
#line hidden
#nullable disable
            WriteLiteral(">\r\n                    <td>");
#nullable restore
#line 34 "D:\sch\2.1\web\assignment\YES\WebApplication\WebApplication\Views\Competitor\ViewCompetitionCriteria.cshtml"
                   Write(item.CompetitionID.ToString());

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                    <td>");
#nullable restore
#line 35 "D:\sch\2.1\web\assignment\YES\WebApplication\WebApplication\Views\Competitor\ViewCompetitionCriteria.cshtml"
                   Write(item.CompetitionName);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                    <td>");
#nullable restore
#line 36 "D:\sch\2.1\web\assignment\YES\WebApplication\WebApplication\Views\Competitor\ViewCompetitionCriteria.cshtml"
                   Write(item.Name);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                    <td>");
#nullable restore
#line 37 "D:\sch\2.1\web\assignment\YES\WebApplication\WebApplication\Views\Competitor\ViewCompetitionCriteria.cshtml"
                   Write(item.StartDate);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                    <td>");
#nullable restore
#line 38 "D:\sch\2.1\web\assignment\YES\WebApplication\WebApplication\Views\Competitor\ViewCompetitionCriteria.cshtml"
                   Write(item.EndDate);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                    <td>");
#nullable restore
#line 39 "D:\sch\2.1\web\assignment\YES\WebApplication\WebApplication\Views\Competitor\ViewCompetitionCriteria.cshtml"
                   Write(item.ResultReleasedDate);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                    <td>\r\n                        ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "740fa5bea59cacc594057296f44444b9153a798a7977", async() => {
                WriteLiteral("View Criteria");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Controller = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-id", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
            }
            BeginWriteTagHelperAttribute();
#nullable restore
#line 42 "D:\sch\2.1\web\assignment\YES\WebApplication\WebApplication\Views\Competitor\ViewCompetitionCriteria.cshtml"
                             WriteLiteral(item.CompetitionID);

#line default
#line hidden
#nullable disable
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-id", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral(" |\r\n                        ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "740fa5bea59cacc594057296f44444b9153a798a10394", async() => {
                WriteLiteral("Join");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_2.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_2);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Controller = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-CompetitionId", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
            }
            BeginWriteTagHelperAttribute();
#nullable restore
#line 44 "D:\sch\2.1\web\assignment\YES\WebApplication\WebApplication\Views\Competitor\ViewCompetitionCriteria.cshtml"
                                        WriteLiteral(item.CompetitionID);

#line default
#line hidden
#nullable disable
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["CompetitionId"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-CompetitionId", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["CompetitionId"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n                    </td>\r\n                </tr>\r\n");
#nullable restore
#line 47 "D:\sch\2.1\web\assignment\YES\WebApplication\WebApplication\Views\Competitor\ViewCompetitionCriteria.cshtml"
            }

#line default
#line hidden
#nullable disable
            WriteLiteral("        </tbody>\r\n    </table>\r\n</div>\r\n\r\n<!-- Display a list of criteria for each competition -->\r\n");
#nullable restore
#line 53 "D:\sch\2.1\web\assignment\YES\WebApplication\WebApplication\Views\Competitor\ViewCompetitionCriteria.cshtml"
 if (ViewData["selectedCompetitionNo"].ToString() != "")
{
    if (Model.criteriaList.Count != 0)
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <h5>\r\n            Criteria for Competition\r\n            ");
#nullable restore
#line 59 "D:\sch\2.1\web\assignment\YES\WebApplication\WebApplication\Views\Competitor\ViewCompetitionCriteria.cshtml"
       Write(ViewData["selectedCompetitionNo"].ToString());

#line default
#line hidden
#nullable disable
            WriteLiteral(" :\r\n        </h5>\r\n");
#nullable restore
#line 61 "D:\sch\2.1\web\assignment\YES\WebApplication\WebApplication\Views\Competitor\ViewCompetitionCriteria.cshtml"
         if (Model.criteriaList.Count > 0)
        {

#line default
#line hidden
#nullable disable
            WriteLiteral(@"            <div class=""table-responsive"">
                <table id=""viewCriteria"" class=""table table-striped table-bordered"">
                    <thead class=""thead-dark"">
                        <tr>
                            <th>ID</th>
                            <th>Criterion Name</th>
                            <th>Weightage</th>
                        </tr>
                    </thead>
                    <tbody>
");
#nullable restore
#line 73 "D:\sch\2.1\web\assignment\YES\WebApplication\WebApplication\Views\Competitor\ViewCompetitionCriteria.cshtml"
                         foreach (var item in Model.criteriaList)
                        {

#line default
#line hidden
#nullable disable
            WriteLiteral("                            <tr>\r\n                                <td>");
#nullable restore
#line 76 "D:\sch\2.1\web\assignment\YES\WebApplication\WebApplication\Views\Competitor\ViewCompetitionCriteria.cshtml"
                               Write(item.CriteriaID.ToString());

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                                <td>");
#nullable restore
#line 77 "D:\sch\2.1\web\assignment\YES\WebApplication\WebApplication\Views\Competitor\ViewCompetitionCriteria.cshtml"
                               Write(item.CriteriaName);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                                <td>");
#nullable restore
#line 78 "D:\sch\2.1\web\assignment\YES\WebApplication\WebApplication\Views\Competitor\ViewCompetitionCriteria.cshtml"
                               Write(item.Weightage);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                            </tr>\r\n");
#nullable restore
#line 80 "D:\sch\2.1\web\assignment\YES\WebApplication\WebApplication\Views\Competitor\ViewCompetitionCriteria.cshtml"
                        }

#line default
#line hidden
#nullable disable
            WriteLiteral("                    </tbody>\r\n                </table>\r\n            </div>\r\n");
#nullable restore
#line 84 "D:\sch\2.1\web\assignment\YES\WebApplication\WebApplication\Views\Competitor\ViewCompetitionCriteria.cshtml"
        }
        else
        {

#line default
#line hidden
#nullable disable
            WriteLiteral("            <span style=\"color:red\">No record found!</span>\r\n");
#nullable restore
#line 88 "D:\sch\2.1\web\assignment\YES\WebApplication\WebApplication\Views\Competitor\ViewCompetitionCriteria.cshtml"
        }

#line default
#line hidden
#nullable disable
#nullable restore
#line 88 "D:\sch\2.1\web\assignment\YES\WebApplication\WebApplication\Views\Competitor\ViewCompetitionCriteria.cshtml"
         
    }
    else
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <h5>No criteria for selected competition!</h5>\r\n");
#nullable restore
#line 93 "D:\sch\2.1\web\assignment\YES\WebApplication\WebApplication\Views\Competitor\ViewCompetitionCriteria.cshtml"
    }
}

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<WebApplication.Models.CompetitionCriteriaViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
