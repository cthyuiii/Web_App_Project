using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication.DAL;
using WebApplication.Models;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

namespace WebApplication.Controllers
{
    public class CompetitorController : Controller
    {
        private CompetitorDAL competitorContext = new CompetitorDAL();
        private CompetitionDAL competitionContext = new CompetitionDAL();
        private CompetitionSubmissionDAL competitionSubContext = new CompetitionSubmissionDAL();
        private CriteriaDAL criteriaContext = new CriteriaDAL();
        private CompetitionScoreDAL compScoreContext = new CompetitionScoreDAL();

        // List that stores the salutations
        private List<string> salutList = new List<string> { "Mr", "Mrs", "Mdm", "Dr" };
        // A list for populating drop-down list
        private List<SelectListItem> salutDropDownList = new List<SelectListItem>();

        public CompetitorController()
        {
            int i = 1;
            // Populating salutation dropdown list
            foreach (string salut in salutList)
            {
                salutDropDownList.Add(
                    new SelectListItem
                    {
                        Text = salut,
                    });
                i++;
            }
        }

        // GET: Competitor
        public ActionResult Index()
        {
            return View();
        }

        // GET: Competitor/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Competitor/ViewCompetitionCriteria/5
        public ActionResult ViewCompetitionCriteria(int? id)
        {
            // If current user's role is not Competitor, redirect to home/index action
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Competitor"))
            {
                return RedirectToAction("Index", "Home");
            }

            // Getting list of all competitions
            // Stored as Competition View Model to display competition's area of interest name
            List<CompetitionViewModel> competitionList = competitionContext.GetAllCompetition(
                competitionContext.GetAreaOfInterest());

            CompetitionCriteriaViewModel competitionCriteriaVM = new CompetitionCriteriaViewModel();

            // Setting competition criteria view model's competition list 
            competitionCriteriaVM.competitionList = competitionList;

            if (id != null)
            {
                // Selected competition no ViewData used to highlight row of selected competition in the table
                ViewData["selectedCompetitionNo"] = id.Value;

                // Get list of criteria for the competition
                competitionCriteriaVM.criteriaList = criteriaContext.GetCompetitionCriteria(id.Value);
            }
            else
            {
                ViewData["selectedCompetitionNo"] = "";
            }

            return View(competitionCriteriaVM);
        }

        // GET: CompetitorController/JoinCompetition/5
        public ActionResult JoinCompetition(int? competitionId)
        {
            // If current user's role is not Competitor, redirect to home/index action
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Competitor"))
            {
                return RedirectToAction("Index", "Home");
            }
            if (competitionId == null)
            {
                //Return to listing page, not allowed to edit
                return RedirectToAction("ViewCompetitionCriteria");
            }

            // Getting details of selected competition 
            // Selected competition's Id is passed from ViewCompetitionCritiera view
            Competition competition = competitionContext.GetDetails(competitionId.Value);

            // Difference between current date time and competition start date stored as TimeSpan
            TimeSpan difference = (TimeSpan)(competition.StartDate - DateTime.Now);

            // Competitor's Id retrieved from session state which was set when a user first logs in
            int competitorId = (int)HttpContext.Session.GetInt32("competitorId");

            // Getting the competitor's Id who has already joined the competition
            int competitorIdFromRecord = competitionSubContext.IsCompetitorInCompetition((int)competitionId, competitorId).CompetitorID;

            // Setting default state of join competition button using ViewData
            ViewData["ButtonState"] = "<input type='submit' value='Join Competition' class='myButton' />";

            // If competition is starting in less than 3 days
            if (difference.Days >= 0 && difference.Days < 3)
            {
                TempData["ErrorMessage"] = "Sorry, you are not allowed to join this competition as it is starting in less than 3 days.";
                ViewData["ButtonState"] = "";
            }
            // If competition has already started
            else if (difference.Days < 0)
            {
                TempData["ErrorMessage"] = "Sorry, you are not allowed to join this competition as it has already started.";
                ViewData["ButtonState"] = "";
            }
            // If competitor has already joined the selected competition
            if (competitorId == competitorIdFromRecord)
            {
                TempData["ErrorMessage"] = "You have already joined this competition.";
                ViewData["ButtonState"] = "";
            }
            if (competition == null)
            {
                //Return to listing page, not allowed to edit
                return RedirectToAction("ViewCompetitionCriteria");
            }

            return View(competition);
        }

        // POST: StaffController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult JoinCompetition(Competition competition)
        {
            if (ModelState.IsValid)
            {
                // Competitor's Id retrieved from session state which was set when a user first logs in
                int competitorId = (int)HttpContext.Session.GetInt32("competitorId");

                // Add competition submission record to database
                competitionSubContext.Add(competition.CompetitionID, competitorId);
                TempData["SuccessfullyJoinedMsg"] = "You have successfully joined this competition";
                return View(competition);

            }
            else
            {
                //Input validation fails, return to the JoinCompetition view
                //to display error message
                return View(competition);
            }
        }

        public ActionResult ViewJoinedCompetitions()
        {
            // If current user's role is not Competitor, redirect to home/index action
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Competitor"))
            {
                return RedirectToAction("Index", "Home");
            }

            // Competitor's Id retrieved from session state which was set when a user first logs in
            int competitorId = (int)HttpContext.Session.GetInt32("competitorId");

            List<CompetitionViewModel> competitionJoinedList = new List<CompetitionViewModel>();
            // Getting list of competition ids of competitions joined by competitor
            List<int> competitionIdJoinedList = competitionSubContext.competitionsJoinedByCompetitor(competitorId);

            foreach (int competitionId in competitionIdJoinedList)
            {
                // Add all joined competitions into competition view model list               
                competitionJoinedList.Add(competitionContext.GetJoinedCompetition(competitionContext.GetAreaInterestJoined(competitionId), competitionId));
            }

            return View(competitionJoinedList);
        }

        public ActionResult SubmitWork(int competitionId)
        {
            // If current user's role is not Competitor, redirect to home/index action
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Competitor"))
            {
                return RedirectToAction("Index", "Home");
            }

            // Competitor's Id retrieved from session state which was set when a user first logs in
            int competitorId = (int)HttpContext.Session.GetInt32("competitorId");

            // Getting details of competition submission record
            CompetitionSubmission compSub = competitionSubContext.GetDetails(competitionId, competitorId);
            // Mapping competition submission to competition submission view model
            CompetitionSubmissionViewModel compSubVM = MapToCompSubVM(compSub);
            // Getting competition details of selected competition
            Competition competition = competitionContext.GetDetails(competitionId);

            // Setting default state of upload button using ViewData
            ViewData["UploadWorkBtn"] = "<input type='submit' value='Upload' class='myButton' />";

            // If Competition has not started
            if (DateTime.Now < competition.StartDate)
            {
                TempData["ErrorSubmitWorkMessage"] = "Sorry, you are not allowed to submit your competition work as the competition has not started.";
                ViewData["UploadWorkBtn"] = "";
            }
            // If competition has already ended
            else if (DateTime.Now > competition.EndDate)
            {
                TempData["ErrorSubmitWorkMessage"] = "Sorry, you are not allowed to submit your competition work as the competition has already ended.";
                ViewData["UploadWorkBtn"] = "";
            }

            return View(compSubVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitWork(CompetitionSubmissionViewModel compSubVM)
        {
            // If uploaded file is not null and has length larger than 0
            if (compSubVM.filetoUpload != null &&
            compSubVM.filetoUpload.Length > 0)
            {
                try
                {
                    // Find the filename extension of the file to be uploaded.
                    string fileExt = Path.GetExtension(
                     compSubVM.filetoUpload.FileName);
                    // Rename the uploaded file with the competitor's id and competition's id.
                    string uploadedFile = "File_" + compSubVM.CompetitorID + "_" + compSubVM.CompetitionID + fileExt;
                    // Get the complete path to the images folder in server
                    string savePath = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot\\CompetitionWork", uploadedFile);
                    // Upload the file to server
                    using (var fileSteam = new FileStream(
                        savePath, FileMode.Create))
                    {
                        await compSubVM.filetoUpload.CopyToAsync(fileSteam);
                    }
                    // Setting name of file submitted
                    compSubVM.FileSubmitted = uploadedFile;
                    // Setting date time of file upload
                    compSubVM.DateTimeFileUpload = DateTime.Now;
                    // Updating competition submission record with name of file submitted and date time of file upload
                    competitionSubContext.UpdateFileNameAndDT(compSubVM);
                    TempData["Message"] = "File uploaded successfully.";
                }
                catch (IOException)
                {
                    //File IO error, could be due to access rights denied
                    TempData["Message"] = "File uploading fail!";
                }
                catch (Exception ex) //Other type of error
                {
                    TempData["Message"] = ex.Message;
                }
            }

            return View(compSubVM);
        }

        public ActionResult ViewScores(int competitionId)
        {
            // If current user's role is not Competitor, redirect to home/index action
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Competitor"))
            {
                return RedirectToAction("Index", "Home");
            }
            // Competitor's Id retrieved from session state which was set when a user first logs in
            int competitorId = (int)HttpContext.Session.GetInt32("competitorId");

            // Getting list of competition scores of competitor's joined competition 
            List<CompetitionScore> compScoreList = compScoreContext.GetScoresOfJoinedComp(competitionId, competitorId);
            // Mapping List of competition scores to list of criteria score view model  
            List<CriteriaScoreViewModel> critScoreList = MapToCritScoreVMList(compScoreList);

            // ViewData for competition name to display in view
            ViewData["CompetitionName"] = competitionContext.GetDetails(competitionId).CompetitionName;
            // ViewData for competition id to pass to view to be used as query string 
            ViewData["CompetitionId"] = competitionId;

            return View(critScoreList);
        }

        public List<CriteriaScoreViewModel> MapToCritScoreVMList(List<CompetitionScore> compScoreList)
        {
            List<CriteriaScoreViewModel> critScoreList = new List<CriteriaScoreViewModel>();

            foreach (CompetitionScore compScore in compScoreList)
            {
                // Adding criteria name and scores for each criteria to list of criteria scores
                critScoreList.Add(
                new CriteriaScoreViewModel
                {
                    // Getting criteria name from database using CriteriaDAL
                    CriteriaName = criteriaContext.GetCritName(compScore.CriteriaID),
                    // Getting competition score from list of competiton scores
                    Score = compScore.Score
                }
                );
            }

            return critScoreList;
        }

        public ActionResult SubmitAppeal(int competitionId)
        {
            // If current user's role is not Competitor, redirect to home/index action
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Competitor"))
            {
                return RedirectToAction("Index", "Home");
            }
            // Competitor's Id retrieved from session state which was set when a user first logs in
            int competitorId = (int)HttpContext.Session.GetInt32("competitorId");

            // Getting details of competition submission
            CompetitionSubmission compSub = competitionSubContext.GetDetails(competitionId, competitorId);
            // Mapping competition submission to competition submission view model
            CompetitionSubmissionViewModel compSubVM = MapToCompSubVM(compSub);
            // Getting competition details
            Competition competition = competitionContext.GetDetails(competitionId);

            // Setting default state of submit appeal button
            ViewData["SubmitAppealBtn"] = "<input type='submit' value='Submit Appeal' class='myButton' />";

            // If current date has passed competition result release date 
            if (DateTime.Now >= competition.ResultReleasedDate)
            {
                TempData["ErrorAppealMessage"] = "Sorry, you are can only submit an appeal before the date of results released.";
                ViewData["SubmitAppealBtn"] = "";
            }
            // If competition submission record has existing appeal
            if (compSubVM.Appeal != null)
            {
                TempData["ErrorAppealMessage"] = "Sorry, you are only allowed to submit ONE appeal.";
                ViewData["SubmitAppealBtn"] = "";
            }

            return View(compSubVM);
        }

        public CompetitionSubmissionViewModel MapToCompSubVM(CompetitionSubmission compSub)
        {
            string competitionName = "";
            // Getting list of all competitions
            List<Competition> competitionList = competitionContext.GetCompetitions();
            foreach (Competition competition in competitionList)
            {
                // If competition's Id matches competition submission's competition Id
                if (competition.CompetitionID == compSub.CompetitionID)
                {
                    // Set name of competition
                    competitionName = competition.CompetitionName;
                    //Exit the foreach loop once the name is found
                    break;
                }
            }
            CompetitionSubmissionViewModel compSubVM = new CompetitionSubmissionViewModel
            {
                CompetitionID = compSub.CompetitionID,
                CompetitorID = compSub.CompetitorID,
                CompetitonName = competitionName,
                FileSubmitted = compSub.FileSubmitted,
                DateTimeFileUpload = compSub.DateTimeFileUpload,
                Appeal = compSub.Appeal,
                VoteCount = compSub.VoteCount,
                Ranking = compSub.Ranking
            };

            return compSubVM;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitAppeal(CompetitionSubmissionViewModel compSubVM)
        {
            if (ModelState.IsValid)
            {
                // Competitor's Id retrieved from session state which was set when a user first logs in
                int competitorId = (int)HttpContext.Session.GetInt32("competitorId");

                //Add appeal record to database
                compSubVM.CompetitorID = competitorId;
                competitionSubContext.UpdateAppealRecord(compSubVM);
                TempData["Message"] = "Appeal submitted successfully.";

                return View(compSubVM);
            }
            else
            {
                //Input validation fails, return to the view
                //to display error message
                return View(compSubVM);
            }
        }

        // GET: Competitor/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Competitor/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Competitor/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Competitor/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
