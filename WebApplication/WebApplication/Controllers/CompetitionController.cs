using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication.DAL;
using WebApplication.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WebApplication.Controllers
{
    public class CompetitionController : Controller
    {
        private CompetitionDAL competitionContext = new CompetitionDAL();
        private JudgeDAL judgeContext = new JudgeDAL();
        private CompetitorDAL competitorContext = new CompetitorDAL();
        private AreaInterestDAL areainterestContext = new AreaInterestDAL();
        private CompetitionSubmissionDAL competitionSubmissionContext = new CompetitionSubmissionDAL();

        // BASIC FEATURES: Create Competition, Edit Competition, Delete Competition

        // GET: Competitions
        public ActionResult ViewCompetitions()
        {
            // Check if Admin role is logged in before proceeding, else return back login page. This validation is present in all GET Methods.
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            List<CompetitionViewModel> competitionList = competitionContext.GetAllCompetition(competitionContext.GetAreaOfInterest());
            return View(competitionList);
        }
        // Mapping competition view model
        public CompetitionViewModel MapToCompetitionVM(Competition competition)
        {
            // The Area of interest Name is returned after checking through a list
            string Name = "";
            if (competition.AreaInterestID != 0)
            {
                List<AreaInterest> areaofinterestnameList = areainterestContext.GetAreaOfInterest();
                foreach (AreaInterest areaInterest in areaofinterestnameList)
                {
                    if (areaInterest.AreaInterestID == competition.AreaInterestID)
                    {
                        Name = areaInterest.Name;
                        break;
                        // The loop here is broken because there is only one area of interest.
                    }
                }
            }
            CompetitionViewModel competitionVM = new CompetitionViewModel
            {
                CompetitionID = competition.CompetitionID,
                AreaInterestID = competition.AreaInterestID,
                Name = Name,
                CompetitionName = competition.CompetitionName,
                StartDate = competition.StartDate,
                EndDate = competition.EndDate,
                ResultReleasedDate = competition.ResultReleasedDate,
            };
            return competitionVM;
        }

        // GET: Competition/Create
        public ActionResult Create(int? areainterestid, Competition competition)
        {
            ViewData["ButtonState"] = "<input type='submit' value='Create and Assign Judges' class='myButton' />";
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            if (areainterestid == null)
            {
                //Query string parameter not provided
                //Return to listing page, not allowed to edit
                return RedirectToAction("Index");
            }
            // To get area of interest details from id.
            AreaInterest areaInterest = areainterestContext.GetDetails(areainterestid.Value);
            CompetitionViewModel competitionVM = MapToCompetitionVM(competition);
            return View(competitionVM);
        }

        // POST: Competitor/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection, Competition competition)
        {
            // Validation to check start date, end date and results date
            // Previously used timespan for all 3 datetime values
            // But was tedious as many validations were needed for all 3 data values
            // Decided to use model to try and catch for invalid data input
            if (!ModelState.IsValid)
            {
                ModelStateEntry startDate = null;
                ModelStateEntry endDate = null;
                ModelStateEntry resultsDate = null;
                if (ModelState.TryGetValue("StartDate", out startDate))
                {
                    if (startDate != null && startDate.Errors.Count > 0)
                    {
                        ModelState.Remove("StartDate");
                        ModelState.AddModelError("StartDate", "Start Date is invalid");
                    }
                }

                if (ModelState.TryGetValue("EndDate", out endDate))
                {
                    if (endDate != null && endDate.Errors.Count > 0)
                    {
                        ModelState.Remove("EndDate");
                        ModelState.AddModelError("EndDate", "End Date is invalid");
                    }
                }

                if (ModelState.TryGetValue("ResultsReleaseDate", out resultsDate))
                {
                    if (resultsDate != null && resultsDate.Errors.Count > 0)
                    {
                        ModelState.Remove("ResultsReleaseDate");
                        ModelState.AddModelError("ResultsReleaseDate", "Result Release Date is invalid");
                    }
                }
                return View();
            }

            //Add competition record to database
            competition.CompetitionID = competitionContext.Add(competition);
            TempData["SuccessMessage"] = "Competition has been successfully created!";
            //Redirect user to Competition/Create view
            return Redirect("~/Competition/ViewJudges");
        }


        // GET: Competition/Edit/5
        public ActionResult Edit(int? id)
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            { //Query string parameter not provided
              //Return to listing page, not allowed to edit
                return RedirectToAction("Index");
            }
            // Viewdata of button
            ViewData["ButtonState"] = "<input type='submit' value='Save' class='myButton' />";
            // Get competition details
            Competition competition = competitionContext.GetDetails(id.Value);
            if (competition == null)
            {
                //Return to listing page, not allowed to edit
                return RedirectToAction("Index");
            }
            CompetitionViewModel competitionVM = MapToCompetitionVM(competition);
            return View(competitionVM);
        }

        // POST: Competition/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Competition competition, IFormCollection collection)
        {
            // Validation to check start date, end date and results date
            // Previously used timespan for all 3 datetime values
            // But was tedious as many validations were needed for all 3 data values
            // Decided to use model to try and catch for invalid data input
            if (!ModelState.IsValid)
            {
                ModelStateEntry startDate = null;
                ModelStateEntry endDate = null;
                ModelStateEntry resultsDate = null;
                if (ModelState.TryGetValue("StartDate", out startDate))
                {
                    if (startDate != null && startDate.Errors.Count > 0)
                    {
                        ModelState.Remove("StartDate");
                        ModelState.AddModelError("StartDate", "Start Date is invalid");
                    }
                }

                if (ModelState.TryGetValue("EndDate", out endDate))
                {
                    if (endDate != null && endDate.Errors.Count > 0)
                    {
                        ModelState.Remove("EndDate");
                        ModelState.AddModelError("EndDate", "End Date is invalid");
                    }
                }

                if (ModelState.TryGetValue("ResultsReleaseDate", out resultsDate))
                {
                    if (resultsDate != null && resultsDate.Errors.Count > 0)
                    {
                        ModelState.Remove("ResultsReleaseDate");
                        ModelState.AddModelError("ResultsReleaseDate", "Result Release Date is invalid");
                    }
                }
                return View();
            }
            competitionContext.Update(competition);
            TempData["SuccessfullyEditMsg"] = "Changes have been saved";
            return RedirectToAction(nameof(Edit));
        }

        // GET: Competition/Delete/5
        public ActionResult Delete(int? id)
        {
            // View data of button which can be changed from setting it later
            ViewData["ButtonState"] = "<input type='submit' value='Delete Competition' class='myButton'/>";
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                //Return to listing page, not allowed to edit
                return RedirectToAction("Index");
            }
            Competition competition = competitionContext.GetDetails(id.Value);
            if (competition == null)
            {
                //Return to listing page, not allowed to edit
                return RedirectToAction("Index");
            }
            return View(competition);
        }

        // POST: Competition/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Competition competition, IFormCollection collection, int? id)
        {
            // If competitor resides in a competition, deleting is prevented
            bool exists = competitionContext.CheckIfCompetitionHasParticipants(id);
            if (exists == true)
            {
                TempData["ErrorMessage"] = "Unable to delete Competition as there is already an existing competition record!";
                ViewData["ButtonState"] = "";
            }
            // Passes data into DAL and deletes the record
            if (ModelState.IsValid)
            {
                competitionContext.Delete(competition.CompetitionID);
                TempData["SuccessfullyDeletedMsg"] = "Competition has been deleted.";
                return RedirectToAction(nameof(Delete));
            }
            else
            {
                return View();
            }
        }

        public ActionResult Rankings(int id)
        {
            // Get all rankings for the selected competition
            List<CompetitionSubmission> rankingList =
                competitionContext.Rankings(competitionSubmissionContext.GetCompetitorName(id), id);
            return View(rankingList);
        }
        public ActionResult ViewJudges()
        {
            // Get all Judges, theier area of interests and Assigned competitions if they are assigned
            List<Judge> j = judgeContext.GetAllJudges();
            foreach (Judge item in j)
            {
                item.Name = areainterestContext.GetAreaInterestName(item.AreaInterestID);
                item.AssignedCompetition = judgeContext.GetJudgesUpcomingCompetition(item.JudgeID);
            }
            // Get all upcoming competitions
            ViewData["UpcomingCompetition"] = competitionContext.GetUpcomingCompetition();
            return View(j);
        }

        // ADVANCED FEATURES: Assigning + Unassigning Judges

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignJudgeToCompetition()
        {
            // Linked to Jquery function in ViewJudges Views
            List<string> competitionIds = Request.Query["competitionid"].ToString().Split('-').ToList();
            List<string> judgeIds = Request.Query["judgeid"].ToString().Split('-').ToList();

            // Foreach loop to loop through each competitionid with every available competition
            // and to loop through all judges
            // Passes the judgeid and competition id into judgedal function to assign
            foreach (string competitionid in competitionIds)
            {
                foreach (string judgeid in judgeIds)
                {
                    judgeContext.AssignJudgeToCompetition(int.Parse(judgeid), int.Parse(competitionid));
                }
            }
            // Success message of assigning
            // Returns back to original view judge view
            TempData["JqueryViewJudges"] = "AssignJudgeSuccessMessage();";
            return RedirectToAction("ViewJudges");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UnassignJudgeFromCompetition()
        {
            // Linked to Jquery function in ViewJudges Views
            string competitionId = Request.Query["competitionid"].ToString();
            string judgeId = Request.Query["judgeid"].ToString();
            // Passes the judgeid and competition id into judgedal function for unassigning
            judgeContext.UnassignJudgeFromCompetition(int.Parse(judgeId), int.Parse(competitionId));

            // Success message of unassigning
            // Returns back to original view judge view
            TempData["JqueryViewJudges"] = string.Format("UnassignJudgeSuccessMessage();");
            return RedirectToAction("ViewJudges");
        }
    }
}


