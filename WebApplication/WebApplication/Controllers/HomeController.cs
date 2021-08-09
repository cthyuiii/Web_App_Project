using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebApplication.DAL;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private CompetitionDAL competitionContext = new CompetitionDAL();
        private CompetitorDAL competitorContext = new CompetitorDAL();
        private JudgeDAL judgeContext = new JudgeDAL();
        private List<SelectListItem> language = new List<SelectListItem>();
        // List that stores the salutations
        private List<string> salutList = new List<string> { "Mr", "Mrs", "Mdm", "Dr" };
        // A list for populating drop-down list
        private List<SelectListItem> salutDropDownList = new List<SelectListItem>();

        public HomeController(ILogger<HomeController> logger)
        {
            int i = 1;
            foreach (string salut in salutList)
            {
                salutDropDownList.Add(
                    new SelectListItem
                    {
                        Text = salut,
                    });
                i++;
            }
            _logger = logger;
        }

        //Get area of interest list and pass it into dropdown list to create judge
        private List<SelectListItem> GetAreaInterest()
        {
            List<SelectListItem> areaIntList = new List<SelectListItem>();
            List<AreaInterest> aiList = judgeContext.GetAreaOfInterest();
            foreach (AreaInterest ai in aiList)
            {
                areaIntList.Add(new SelectListItem
                {
                    Value = ai.AreaInterestID.ToString(),
                    Text = ai.Name
                });
            }
            return areaIntList;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult HomeLogin(IFormCollection formData)
        {
            // Read inputs from textboxes
            // Email address converted to lowercase
            string email = formData["txtEmail"].ToString().Trim();
            string password = formData["txtPassword"].ToString();
            Competitor competitor = competitorContext.LoginCompetitor(email);
            Judge judge = judgeContext.LoginJudge(email);
            if (email == "admin1@lcu.edu.sg" && password == "p@55Admin")
            {
                // Store user role “Admin” as a string in session with the key “Role”
                HttpContext.Session.SetString("Role", "Admin");

                // Redirect user to the "create" view through an action
                return RedirectToAction("Index", "AreaInterest");
            }
            if (email == competitor.EmailAddr && password == competitor.Password)
            {
                // Store user role “Competitor” as a string in session with the key “Role”
                HttpContext.Session.SetInt32("competitorId", competitor.CompetitorID);
                HttpContext.Session.SetString("Name", competitor.CompetitorName);
                HttpContext.Session.SetString("Role", "Competitor");

                // Redirect user to the "create" view through an action
                return RedirectToAction("ViewCompetitionCriteria", "Competitor");
            }
            else if (email == judge.EmailAddr && password == judge.Password)
            {
                // Store user role “Judge” as a string in session with the key “Role”
                HttpContext.Session.SetInt32("judgeId", judge.JudgeID);
                HttpContext.Session.SetString("Name", judge.JudgeName);
                HttpContext.Session.SetString("Role", "Judge");

                return RedirectToAction("Index", "Judge");
            }
            else
            {
                // Store an error message in TempData for display at the index view
                TempData["Message"] = "Invalid Login Credentials!";
                // Redirect user back to the login view through an action
                return RedirectToAction("Login");
            }
        }

        // GET: Home/CreateCompetitor
        public ActionResult CreateCompetitor()
        {
            ViewData["Salutations"] = salutDropDownList;
            return View();
        }

        // POST: Home/CreateCompetitor
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCompetitor(Competitor competitor)
        {
            ViewData["Salutations"] = salutDropDownList;
            if (ModelState.IsValid)
            {
                //Add competitor record to database
                competitor.CompetitorID = competitorContext.Add(competitor);
                TempData["SuccessMessage"] = "Competitor Profile has been successfully created!";
                //Redirect user to Home/CreateCompetitor view
                return RedirectToAction("CreateCompetitor");
            }
            else
            {
                //Input validation fails, return to the Create view
                //to display error message
                return View(competitor);
            }

        }

        // GET: Home/CreateJudge
        public ActionResult CreateJudge()
        {
            ViewData["Salutations"] = salutDropDownList;
            ViewData["AreaInterest"] = GetAreaInterest();
            return View();
        }

        //POST: Home/CreateJudge
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateJudge(Judge judge)
        {
            ViewData["Salutations"] = salutDropDownList;
            ViewData["AreaInterest"] = GetAreaInterest();
            string[] emailCheck = judge.EmailAddr.Split('@');
            if ((judge.EmailAddr.Split('@')[1] != "lcu.edu.sg"))
            {
                ViewData["ErrorMessage"] = "Only Staff of Lion City University Can Register to become a judge!";
                Judge judgevm = new Judge()
                {
                    JudgeName = judge.JudgeName,
                    Salutation = judge.Salutation,
                    AreaInterestID = judge.AreaInterestID,
                    EmailAddr = judge.EmailAddr,
                    Password = judge.Password
                };
                return View();
            }
            if (ModelState.IsValid)
            {
                //Add judge record to database
                judge.JudgeID = judgeContext.Add(judge);
                //Redirect user to Judge/Create View
                return RedirectToAction("Index");
            }
            else
            {
                //Input validation fails, return to the Create view
                //to display error message
                return View(judge);
            }
        }

        public IActionResult ContactUs()
        {
            return View();
        }

        public IActionResult ViewCompetition()
        {
            // Get all competitions in database
            List<CompetitionViewModel> competitionList = competitionContext.GetAllCompetition(
                competitionContext.GetAreaOfInterest());
            return View(competitionList);
        }

        public async Task<IActionResult> Weather()
        {
            // Build HttpClient
            var client = new HttpClient();
            // Add request for HttpRequest
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                // Uri for API page
                RequestUri = new Uri("https://community-open-weather-map.p.rapidapi.com/weather?q=Singapore&lat=0&lon=0&id=2172797&lang=null&units=%22metric%22%20or%20%22imperial%22&mode=xml%2C%20html"),
                // API keys for authentication
                Headers =
                {
                    { "x-rapidapi-key", "00e46b3abdmsh2e2722f17f16ad5p122e50jsn4869d686578b" },
                    { "x-rapidapi-host", "community-open-weather-map.p.rapidapi.com" },
                },
            };
            // Send asynchronous request
            using (var response = await client.SendAsync(request))
            {
                // Ensure that there is a successful response
                response.EnsureSuccessStatusCode();
                // Read response as a string
                var body = await response.Content.ReadAsStringAsync();
                // Deserialize JSON string to Temperatures object
                Temperatures weatherData = JsonConvert.DeserializeObject<Temperatures>(body);
                // As the temperture is in Kelvin, convert it into Celsius and display in 2 decimal places
                TempData["Minimum Temp"] = (weatherData.Main.TempMin - 273.15).ToString("#.##");
                TempData["Maximum Temp"] = (weatherData.Main.TempMax - 273.15).ToString("#.##");
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public ActionResult LogOut()
        {
            // Clear all key-values pairs stored in session state
            HttpContext.Session.Clear();
            // Call the Index action of Home controller
            return RedirectToAction("Login");
        }
    }
}
