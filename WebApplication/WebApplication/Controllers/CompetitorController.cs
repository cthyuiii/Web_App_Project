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

        // GET: Competitor/Create
        public ActionResult Create()
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Competitor"))
            {
                return RedirectToAction("Index", "Home");
            }
            ViewData["Salutations"] = salutDropDownList;
            return View();
        }

        // GET: Competitor/ViewCompetitionCriteria/5
        public ActionResult ViewCompetitionCriteria(int? id)
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Competitor"))
            {
                return RedirectToAction("Index", "Home");
            }
            List<CompetitionViewModel> competitionList = competitionContext.GetAllCompetition(
                competitionContext.GetAreaOfInterest());
            CompetitionCriteriaViewModel competitionCriteriaVM = new CompetitionCriteriaViewModel();
            competitionCriteriaVM.competitionList = competitionList;

            if (id != null)
            {
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
            Competition competition = competitionContext.GetDetails(competitionId.Value);
            TempData["CompetitionStartDate"] = competition.StartDate;
            TimeSpan difference = (TimeSpan)(competition.StartDate - DateTime.Now);
            int competitorId = (int)HttpContext.Session.GetInt32("competitorId");
            int competitorIdFromRecord = competitionSubContext.IsCompetitorInCompetition((int)competitionId, competitorId).CompetitorID;
            ViewData["ButtonState"] = "<input type='submit' value='Join Competition' class='myButton' />";
            if (difference.Days >= 0 && difference.Days < 3)
            {
                TempData["ErrorMessage"] = "Sorry, you are not allowed to join this competition as it is starting in less than 3 days.";
                ViewData["ButtonState"] = "";
            }
            else if (difference.Days < 0) 
            {
                TempData["ErrorMessage"] = "Sorry, you are not allowed to join this competition as it has already started.";
                ViewData["ButtonState"] = "";
            }
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

        // POST: Competitor/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Competitor competitor)
        {
            ViewData["Salutations"] = salutDropDownList;
            if (ModelState.IsValid)
            {
                //Add competitor record to database
                competitor.CompetitorID = competitorContext.Add(competitor);
                TempData["SuccessMessage"] = "Competitor Profile has been successfully created!";
                //Redirect user to Competitor/Create view
                return RedirectToAction("Create");
            }
            else
            {             
                //Input validation fails, return to the Create view
                //to display error message
                return View(competitor);
            }

        }

        public ActionResult ViewJoinedCompetitions()
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Competitor"))
            {
                return RedirectToAction("Index", "Home");
            }
            int competitorId = (int)HttpContext.Session.GetInt32("competitorId");
            List<CompetitionViewModel> competitionJoinedList = new List<CompetitionViewModel>();
            List<int> competitionIdJoinedList = competitionSubContext.competitionsJoinedByCompetitor(competitorId);

            foreach (int competitionId in competitionIdJoinedList)
            {
                competitionJoinedList.Add(competitionContext.GetJoinedCompetition(competitionContext.GetAreaInterestJoined(competitionId), competitionId));
            }

            return View(competitionJoinedList);
        }

        public ActionResult SubmitWork(int competitionId)
        {
            // Stop accessing the action if not logged in
            // or account not in the "Competitor" role
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Competitor"))
            {
                return RedirectToAction("Index", "Home");
            }
            int competitorId = (int)HttpContext.Session.GetInt32("competitorId");
            CompetitionSubmission compSub = competitionSubContext.GetDetails(competitionId, competitorId);
            CompetitionSubmissionViewModel compSubVM = MapToCompSubVM(compSub);
            Competition competition = competitionContext.GetDetails(competitionId);
            ViewData["UploadWorkBtn"] = "<input type='submit' value='Upload' class='myButton' />";
            if (DateTime.Now < competition.StartDate || DateTime.Now > competition.EndDate)
            {
                TempData["ErrorSubmitWorkMessage"] = "Sorry, you are not allowed to submit your competition work as the competition has not started or has already ended.";
                ViewData["UploadWorkBtn"] = "";
            }
            return View(compSubVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitWork(CompetitionSubmissionViewModel compSubVM)
        {
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
                    compSubVM.FileSubmitted = uploadedFile;
                    compSubVM.DateTimeFileUpload = DateTime.Now;
                    competitionSubContext.UpdateFileNameAndDT(compSubVM);
                    TempData["Message"] = "File uploaded successfully.";
                }
                catch (IOException)
                {
                    //File IO error, could be due to access rights denied
                    ViewData["Message"] = "File uploading fail!";
                }
                catch (Exception ex) //Other type of error
                {
                    ViewData["Message"] = ex.Message;
                }
            }
            return View(compSubVM);
        }

        public CompetitionSubmissionViewModel MapToCompSubVM(CompetitionSubmission compSub)
        {
            string competitionName = "";
            List<Competition> competitionList = competitionContext.GetCompetitions();
            foreach (Competition competition in competitionList)
            {
                if (competition.CompetitionID == compSub.CompetitionID)
                {
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

        public ActionResult ViewScores(int competitionId)
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Competitor"))
            {
                return RedirectToAction("Index", "Home");
            }
            int competitorId = (int)HttpContext.Session.GetInt32("competitorId");
            List<CompetitionScore> compScoreList = compScoreContext.GetScoresOfJoinedComp(competitionId, competitorId);
            List<CriteriaScoreViewModel> critScoreList = MapToCritScoreVMList(compScoreList);
            ViewData["CompetitionName"] = competitionContext.GetDetails(competitionId).CompetitionName;
            ViewData["CompetitionId"] = competitionId;

            return View(critScoreList);
        }

        public List<CriteriaScoreViewModel> MapToCritScoreVMList(List<CompetitionScore> compScoreList)
        {
            List<CriteriaScoreViewModel> critScoreList = new List<CriteriaScoreViewModel>();

            foreach (CompetitionScore compScore in compScoreList)
            {
                critScoreList.Add(
                new CriteriaScoreViewModel
                {
                    CriteriaName = criteriaContext.GetCritName(compScore.CriteriaID),
                    Score = compScore.Score
                }
                );                                    
            }
            return critScoreList;
        }

        public ActionResult SubmitAppeal(int competitionId)
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Competitor"))
            {
                return RedirectToAction("Index", "Home");
            }
            int competitorId = (int)HttpContext.Session.GetInt32("competitorId");
            CompetitionSubmission compSub = competitionSubContext.GetDetails(competitionId, competitorId);
            CompetitionSubmissionViewModel compSubVM = MapToCompSubVM(compSub);
            Competition competition = competitionContext.GetDetails(competitionId);
            ViewData["SubmitAppealBtn"] = "<input type='submit' value='Submit Appeal' class='myButton' />";
            if (DateTime.Now >= competition.ResultReleasedDate)
            {
                TempData["ErrorAppealMessage"] = "Sorry, you are can only submit an appeal before the date of results released.";
                ViewData["SubmitAppealBtn"] = "";
            }
            if (compSubVM.Appeal != null)
            {
                TempData["ErrorAppealMessage"] = "Sorry, you are only allowed to submit ONE appeal.";
                ViewData["SubmitAppealBtn"] = "";
            }

            return View(compSubVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitAppeal(CompetitionSubmissionViewModel compSubVM)
        {
            if (ModelState.IsValid)
            {
                //Add appeal record to database
                int competitorId = (int)HttpContext.Session.GetInt32("competitorId");
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
