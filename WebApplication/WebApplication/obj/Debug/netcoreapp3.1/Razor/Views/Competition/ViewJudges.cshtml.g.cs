#pragma checksum "C:\Users\tanji\source\repos\web2021apr_p01_t02_1\WebApplication\WebApplication\Views\Competition\ViewJudges.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "f75b5aec526623329148585626b140fc909c4a79"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Competition_ViewJudges), @"mvc.1.0.view", @"/Views/Competition/ViewJudges.cshtml")]
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
#line 1 "C:\Users\tanji\source\repos\web2021apr_p01_t02_1\WebApplication\WebApplication\Views\_ViewImports.cshtml"
using WebApplication;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\tanji\source\repos\web2021apr_p01_t02_1\WebApplication\WebApplication\Views\_ViewImports.cshtml"
using WebApplication.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\Users\tanji\source\repos\web2021apr_p01_t02_1\WebApplication\WebApplication\Views\_ViewImports.cshtml"
using Microsoft.AspNetCore.Http;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"f75b5aec526623329148585626b140fc909c4a79", @"/Views/Competition/ViewJudges.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"fcac9b5938334f89264800ae18adab0319d63c0b", @"/Views/_ViewImports.cshtml")]
    public class Views_Competition_ViewJudges : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<WebApplication.Models.Judge>>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("id", new global::Microsoft.AspNetCore.Html.HtmlString("Assign"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("method", "post", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("id", new global::Microsoft.AspNetCore.Html.HtmlString("Unassign"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 2 "C:\Users\tanji\source\repos\web2021apr_p01_t02_1\WebApplication\WebApplication\Views\Competition\ViewJudges.cshtml"
  
    ViewData["Title"] = "ViewJudges";
    List<Competition> UpcomingCompetition = (List<Competition>)ViewData["UpcomingCompetition"];

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<!-- jQuery library -->\r\n<script src=\"https://ajax.googleapis.com/ajax/libs/jquery/3.4.1/jquery.min.js\">\r\n</script>\r\n\r\n");
            DefineSection("Scripts", async() => {
                WriteLiteral("\r\n    <script>\r\n        // Definiing variables\r\n        var model = ");
#nullable restore
#line 14 "C:\Users\tanji\source\repos\web2021apr_p01_t02_1\WebApplication\WebApplication\Views\Competition\ViewJudges.cshtml"
               Write(Html.Raw(Json.Serialize(Model)));

#line default
#line hidden
#nullable disable
                WriteLiteral(";\r\n        var futureComp = ");
#nullable restore
#line 15 "C:\Users\tanji\source\repos\web2021apr_p01_t02_1\WebApplication\WebApplication\Views\Competition\ViewJudges.cshtml"
                    Write(Html.Raw(Json.Serialize(UpcomingCompetition)));

#line default
#line hidden
#nullable disable
                WriteLiteral(@";
        var selectedJudge = [];
        var compArray = [];

        function AssignToCompetition() {
            selectedJudge = [];
            $('#judgeCheck input:checked').each(function () {
                selectedJudge.push($(this).attr('name'));
            });
            // if not judge is selected
            if (selectedJudge.length == 0) {
                alert(""Please select a Judge"");
            }
            // Validation + creating Html
            else {
                var areaInterestIdList = [];
                selectedJudge.forEach((JudgeID) => {
                    model.forEach((m) => {
                        if (JudgeID == m.JudgeID) {
                            areaInterestIdList.push(m.areaInterestID);
                        }
                    });
                });
                // Validation section
                if (areaInterestIdList.every((val, i, arr) => val === arr[0])) {
                    var areaInterestId = areaInterestIdList[0];
 ");
                WriteLiteral(@"                   var count = 0;
                    model.forEach((m) => {
                        if (m.areaInterestID == areaInterestId && m.AssignedCompetition.length == 1) {
                            count++;
                        }
                    });
                    // If the count of selected judges via using areainterstid and assigned competition is less than two, throw errormessage.
                    if (count < 2) {
                        if (selectedJudge.length != 2) {
                            alert(""Please assign at least 2 judges"");
                            return;
                        }
                    }
                    // Validation to check if future competitions have the same area interest
                    var validCompetition = [];
                    futureComp.forEach((c) => {
                        if (areaInterestId == c.areaInterestId) {
                            validCompetition.push(c);
                        }
             ");
                WriteLiteral(@"       });
                    // Similarly, Validation to check if current competitions have the same area interest
                    var haveCompetition = false;
                    haveCompetition.forEach((c) => {
                        if (areaInterestId == c.areaInterestId) {
                            haveCompetition = true;
                        }
                    });
                    // Else error message displays none competitions for area interest
                    if (!haveCompetition) {
                        alert('There are no competitions created for this area of interest');
                        return;
                    }
                    // Inserting Html section
                    var html = """";
                    for (var i = 0; i < validCompetition.length; i++) {
                        var startdate = new Date(Date.parse(validCompetition[i].startDate));
                        var startDateString = formatDatetime(startdate, true);
               ");
                WriteLiteral(@"         var enddate = new Date(Date.parse(validCompetition[i].endDate));
                        var endDateString = formatDatetime(enddate, false); html += ""<tr>"";
                        html += ""<td><div class=\""form-group\"">"";
                        html += ""<input type=\""checkbox\"" name=\"""" + validCompetition[i].competitionID + ""\"" class=\""form-control\"""" + i + ""\"">"";
                        html += ""<label class=\""control-label\"""" + i + ""\""></label>"";
                        html += ""</div></td>"";
                        html += ""<td>"";
                        html += ""<p>"" + validCompetition[i].competitionName + ""</p>"";
                        html += ""</div></td>"";
                        html += ""<td>"";
                        html += ""<p>"" + startDateString + "" - "" + endDateString + ""</p>"";
                        html += ""</td>"";
                    }
                    $('#checkCompetition').html(html);
                    $('#checkCompetition tr td').on('click', '.cbCheck', funct");
                WriteLiteral(@"ion () {
                        if ($(this).is(':checked')) {
                            compArray.push($(this).attr('name'));
                        }
                        else {
                            compArray.splice(compArray.indexOf($(this).attr('name')), 1);
                        }
                    });
                    $('#AssignCompetitionModal').modal('show');
                }
                else {
                    alert('Both Area of Interest of Judges and Competitions must MATCH to assign!');
                }
            }
        }
        // Function to unassign judge from competition.
        function UnassignCompetition(judgeId, compId, compName) {
            var count = 0;
            model.forEach((judge) => {
                if (judge.AssignedCompetition.length == 1) {
                    if (judge.AssignedCompetition[0].competitionID == compId) {
                        count++;
                    }
                }
            });
       ");
                WriteLiteral(@"     if (count == 2) {
                alert('There must be at least 2 judges to judge this competition!');
            }
            else {
                $('#UnassignModalBody').html(compName);
                $('#UnassignedJudgeBtn').attr('onclick', 'UnassignJudge(' + judgeId + ', ' + compId + ')');
                $('#UnassignModal').modal('show');
            }
        }
        // Connect to Controller method to parse judge id and competition id
        function unassignJudge(judgeid, competitionid) {
            $(""#Unassign"").attr('action', '/Competition/UnassignJudgeFromCompetition/?judgeid=' + judgeid + '&competitionid=' + competitionid);
            $(""#Unassign"").submit();
        }
        // Unassigned message
        function UnassignJudgeSuccessMessage() {
            alert('Unassigned Judge from competition');
        }
        // Function to assign judge
        function AssignJudge() {
            var compString = '';
            compArray.forEach((c) => {
           ");
                WriteLiteral(@"     compString += c + '-';
            });
            compString = compString.substring(0, compString.length - 1);
            if (compString.length == 0) {
                alert(""Please select a competition to Assign"");
            } else {
                var judgeStr = '';
                selectedJudge.forEach((judge) => {
                    judgeStr += judge + '-';
                });
                judgeStr = judgeStr.substring(0, judgeStr.length - 1);
                // Connect to Controller method to parse judge name and competition name
                $(""#Assign"").attr('action', '/Competition/AssignJudgeToCompetition/?judgeid=' + judgeStr + '&competitionid=' + compString);
                $(""#Assign"").submit();
            }
        }

        function AssignJudgeSuccessMessage() {
            alert('Successfully Assigned Judge');
        }

        $(document).ready(function () {
");
#nullable restore
#line 161 "C:\Users\tanji\source\repos\web2021apr_p01_t02_1\WebApplication\WebApplication\Views\Competition\ViewJudges.cshtml"
             if (TempData["JqueryViewJudges"] != null)
            {
                

#line default
#line hidden
#nullable disable
#nullable restore
#line 163 "C:\Users\tanji\source\repos\web2021apr_p01_t02_1\WebApplication\WebApplication\Views\Competition\ViewJudges.cshtml"
           Write(Html.Raw(TempData["JqueryViewJudges"]));

#line default
#line hidden
#nullable disable
#nullable restore
#line 163 "C:\Users\tanji\source\repos\web2021apr_p01_t02_1\WebApplication\WebApplication\Views\Competition\ViewJudges.cshtml"
                                                       
            }

#line default
#line hidden
#nullable disable
                WriteLiteral("        });\r\n    </script>\r\n");
            }
            );
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("body", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "f75b5aec526623329148585626b140fc909c4a7914237", async() => {
                WriteLiteral(@"
    <h2>Select Judges</h2>
    <div class=""table-responsive"">
        <input type=""submit"" onclick=""AssignToCompetition()"" ;"" />
        <table class=""table table-striped table-bordered"">
            <thead class=""thead-dark"">
                <tr>
                    <th></th>
                    <th>Salutation</th>
                    <th>Name</th>
                    <th>Area of Interest</th>
                    <th>Email Address</th>
                    <th>Competition Assigned</th>
                </tr>
            </thead>
            <tbody>
");
#nullable restore
#line 184 "C:\Users\tanji\source\repos\web2021apr_p01_t02_1\WebApplication\WebApplication\Views\Competition\ViewJudges.cshtml"
                 for (int i = 0; i < Model.Count(); i++)
                {
                    string text = "";
                    foreach (Competition c in Model.ElementAt(i).AssignedCompetition)
                    {
                        text += c.CompetitionName;
                    }
                    if (Model.ElementAt(i).AssignedCompetition.Count() == 0)
                    {
                        text = "Not Assigned";
                    }
                    else
                    {
                        text = Model.ElementAt(i).AssignedCompetition.ElementAt(0).CompetitionName;
                    }

#line default
#line hidden
#nullable disable
                WriteLiteral("                    <tr>\r\n                        <td>\r\n");
#nullable restore
#line 201 "C:\Users\tanji\source\repos\web2021apr_p01_t02_1\WebApplication\WebApplication\Views\Competition\ViewJudges.cshtml"
                             if (Model.ElementAt(i).AssignedCompetition.Count() == 0)
                            {

#line default
#line hidden
#nullable disable
                WriteLiteral("                                <input type=\"checkbox\" class=\"form-control\"");
                BeginWriteAttribute("name", " name=\"", 9224, "\"", 9258, 1);
#nullable restore
#line 203 "C:\Users\tanji\source\repos\web2021apr_p01_t02_1\WebApplication\WebApplication\Views\Competition\ViewJudges.cshtml"
WriteAttributeValue("", 9231, Model.ElementAt(i).JudgeID, 9231, 27, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(">\r\n                                <label class=\"control-label\"></label>\r\n");
#nullable restore
#line 205 "C:\Users\tanji\source\repos\web2021apr_p01_t02_1\WebApplication\WebApplication\Views\Competition\ViewJudges.cshtml"
                            }
                            else
                            {

#line default
#line hidden
#nullable disable
                WriteLiteral("                                <input type=\"checkbox\" class=\"form-control\" disabled>\r\n                                <label class=\"control-label\"></label>\r\n");
#nullable restore
#line 210 "C:\Users\tanji\source\repos\web2021apr_p01_t02_1\WebApplication\WebApplication\Views\Competition\ViewJudges.cshtml"
                            }

#line default
#line hidden
#nullable disable
                WriteLiteral("                        </td>\r\n                        <td><p>");
#nullable restore
#line 212 "C:\Users\tanji\source\repos\web2021apr_p01_t02_1\WebApplication\WebApplication\Views\Competition\ViewJudges.cshtml"
                          Write(Model.ElementAt(i).Salutation);

#line default
#line hidden
#nullable disable
                WriteLiteral("</p></td>\r\n                        <td><p>");
#nullable restore
#line 213 "C:\Users\tanji\source\repos\web2021apr_p01_t02_1\WebApplication\WebApplication\Views\Competition\ViewJudges.cshtml"
                          Write(Model.ElementAt(i).Name);

#line default
#line hidden
#nullable disable
                WriteLiteral("</p></td>\r\n                        <td><p>");
#nullable restore
#line 214 "C:\Users\tanji\source\repos\web2021apr_p01_t02_1\WebApplication\WebApplication\Views\Competition\ViewJudges.cshtml"
                          Write(Model.ElementAt(i).AreaInterestName);

#line default
#line hidden
#nullable disable
                WriteLiteral("</p></td>\r\n                        <td><p>");
#nullable restore
#line 215 "C:\Users\tanji\source\repos\web2021apr_p01_t02_1\WebApplication\WebApplication\Views\Competition\ViewJudges.cshtml"
                          Write(Model.ElementAt(i).EmailAddr);

#line default
#line hidden
#nullable disable
                WriteLiteral("</p></td>\r\n                        <td><p>");
#nullable restore
#line 216 "C:\Users\tanji\source\repos\web2021apr_p01_t02_1\WebApplication\WebApplication\Views\Competition\ViewJudges.cshtml"
                          Write(Html.Raw(text));

#line default
#line hidden
#nullable disable
                WriteLiteral("</p></td>\r\n                        <td>\r\n");
#nullable restore
#line 218 "C:\Users\tanji\source\repos\web2021apr_p01_t02_1\WebApplication\WebApplication\Views\Competition\ViewJudges.cshtml"
                             if (Model.ElementAt(i).AssignedCompetition.Count() == 0)
                            {

#line default
#line hidden
#nullable disable
                WriteLiteral("                                <p>");
#nullable restore
#line 220 "C:\Users\tanji\source\repos\web2021apr_p01_t02_1\WebApplication\WebApplication\Views\Competition\ViewJudges.cshtml"
                              Write(Html.Raw(text));

#line default
#line hidden
#nullable disable
                WriteLiteral("</p>\r\n");
#nullable restore
#line 221 "C:\Users\tanji\source\repos\web2021apr_p01_t02_1\WebApplication\WebApplication\Views\Competition\ViewJudges.cshtml"
                            }
                            else
                            {

#line default
#line hidden
#nullable disable
                WriteLiteral("                                <input type=\'submit\' value=\'Delete Competition\' class=\'myButton\'");
                BeginWriteAttribute("onclick", "\r\n                                       onclick=\"", 10389, "\"", 10769, 9);
                WriteAttributeValue("", 10439, "UnassignCompetition(\'", 10439, 21, true);
#nullable restore
#line 225 "C:\Users\tanji\source\repos\web2021apr_p01_t02_1\WebApplication\WebApplication\Views\Competition\ViewJudges.cshtml"
WriteAttributeValue("", 10460, Model.ElementAt(i).JudgeID, 10460, 27, false);

#line default
#line hidden
#nullable disable
                WriteAttributeValue("", 10487, "\',", 10487, 2, true);
                WriteAttributeValue("\r\n                                                                    ", 10489, "\'", 10559, 71, true);
#nullable restore
#line 226 "C:\Users\tanji\source\repos\web2021apr_p01_t02_1\WebApplication\WebApplication\Views\Competition\ViewJudges.cshtml"
WriteAttributeValue("", 10560, Model.ElementAt(i).AssignedCompetition.ElementAt(0).CompetitionID, 10560, 66, false);

#line default
#line hidden
#nullable disable
                WriteAttributeValue("", 10626, "\',", 10626, 2, true);
                WriteAttributeValue("\r\n                                                                    ", 10628, "\'", 10698, 71, true);
#nullable restore
#line 227 "C:\Users\tanji\source\repos\web2021apr_p01_t02_1\WebApplication\WebApplication\Views\Competition\ViewJudges.cshtml"
WriteAttributeValue("", 10699, Model.ElementAt(i).AssignedCompetition.ElementAt(0).CompetitionName, 10699, 68, false);

#line default
#line hidden
#nullable disable
                WriteAttributeValue("", 10767, "\')", 10767, 2, true);
                EndWriteAttribute();
                WriteLiteral(" />\r\n");
#nullable restore
#line 228 "C:\Users\tanji\source\repos\web2021apr_p01_t02_1\WebApplication\WebApplication\Views\Competition\ViewJudges.cshtml"
                            }

#line default
#line hidden
#nullable disable
                WriteLiteral("                        </td>\r\n                    </tr>\r\n");
#nullable restore
#line 231 "C:\Users\tanji\source\repos\web2021apr_p01_t02_1\WebApplication\WebApplication\Views\Competition\ViewJudges.cshtml"
                }

#line default
#line hidden
#nullable disable
                WriteLiteral(@"            </tbody>
        </table>
    </div>
    <div class=""modal"" id=""AssignCompetitionModal"">
        <h4>Assign Judge</h4>
        <input type=""submit"" class=""myButton"" data-dismiss=""modal"" value='Exit' />
        <div class=""table-responsive"">
            <table class=""table table-striped table-bordered"">
                <thead class=""thead-dark"">
                    <tr>
                        <th></th>
                        <th>Name of Competition</th>
                        <th>Date of Competition</th>
                    </tr>
                </thead>
                <tbody id=""checkCompetition"">
                </tbody>
            </table>
        </div>
        ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "f75b5aec526623329148585626b140fc909c4a7923587", async() => {
                    WriteLiteral("\r\n            <button type=\"button\" onclick=\"AssignJudge()\">Assign</button>\r\n        ");
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
                __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Method = (string)__tagHelperAttribute_1.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral(@"
    </div>

    <div class=""modal"" id=""UnassignModal"">
        <h4>Unassign Judge</h4>
        <input type=""submit"" class=""myButton"" data-dismiss=""modal"" value='Exit' />
        <div class=""modal-body"">
            Name of Competition:
            <div id=""UnassignModalBody"">
            </div>
        </div>
        ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "f75b5aec526623329148585626b140fc909c4a7925609", async() => {
                    WriteLiteral("\r\n            <button type=\"button\" id=\"UnassignedJudgeBtn\" class=\"myButton\">Unassign Competition</button>\r\n        ");
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
                __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Method = (string)__tagHelperAttribute_1.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n    </div>\r\n");
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<WebApplication.Models.Judge>> Html { get; private set; }
    }
}
#pragma warning restore 1591
