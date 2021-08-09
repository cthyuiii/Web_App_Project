using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication.DAL;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class CompetitionController : Controller
    {
        private CompetitionDAL competitionContext = new CompetitionDAL();
        private JudgeDAL judgeContext = new JudgeDAL();
        private CompetitorDAL competitorContext = new CompetitorDAL();
        private AreaInterestDAL areainterestContext = new AreaInterestDAL();
        private CompetitionSubmissionDAL competitionSubmissionContext = new CompetitionSubmissionDAL();

        // GET: Competitor
        public ActionResult Index()
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
        
        // GET: Competition/Details/5
        public ActionResult Details(int id, CompetitionJudge competitionJudge)
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            // Pull details of competition and the judge of the competition.
            Competition competition = competitionContext.GetDetails(id);
            CompetitionViewModel competitionVM = MapToCompetitionVM(competition, competitionJudge);
            return View(competitionVM);
        }
        
        public CompetitionViewModel MapToCompetitionVM(Competition competition, CompetitionJudge competitionJudge)
        {
            string judgeName = "";
            // Get the Judge's Name who is judging the competition via checking the Judgeids.
            if (competitionJudge.JudgeID != 0)
            {
                List<Judge> judgeList = judgeContext.GetAllJudges();
                foreach (Judge judge in judgeList)
                {
                    if (judge.JudgeID == competitionJudge.JudgeID)
                    {
                        judgeName = judge.JudgeName;
                    }
                }
            }
            // Similar to Judge, the Area of interest Name is returned after checking through a list
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
                        // Unlike the foreach continuous checking for judge, the loop here is broken because there is only one area of interest.
                    }
                }
            }
            CompetitionViewModel competitionVM = new CompetitionViewModel
            {
                CompetitionID = competition.CompetitionID,
                AreaInterest = Name,
                CompetitionName = competition.CompetitionName,
                StartDate = competition.StartDate,
                EndDate = competition.EndDate,
                ResultReleasedDate = competition.ResultReleasedDate,
                JudgeName = judgeName,
            };
            return competitionVM;
        }

        // GET: Competition/Create
        public ActionResult Create(int? areainterestid, Judge judge, Competition competition)
        {
            // Preview all Judges from a judge list
            ViewData["JudgeList"] = GetAllJudges();
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
            AreaInterest areaInterest = areainterestContext.GetDetails(areainterestid.Value);
            // Get Competition Judge Lists to check validation.
            // For this case, a foreach loop will run and check how many judges are present when the admin has clicked on create.
            // If there are less than 2 judges or in this case count, an error message will be displayed and create will fail.
            // The foreach loop will check if a competition judge's id is the same as the current competiion ID
            List<CompetitionJudgeViewModel> CompetitionJudgeList = new List<CompetitionJudgeViewModel>();
            int count = 0;
            for (int i = 0; i < CompetitionJudgeList.Count; i++)
            {
                CompetitionJudgeViewModel CJudge = CompetitionJudgeList[i];
                if (CJudge.CompetitionID == competition.CompetitionID)
                {
                    count++;
                }
            }
            if (count < 2)
            {   
                TempData["ErrorMessage"] = "There are insuffcient Judges, please assign at least 2 Judges!";
            }
            // Validation to check if a Judge's Area of Interest is similar to the Competition's one, else the create will fail and an erro will pop up
            if (judge.AreaInterestID != competition.AreaInterestID)
            {
                TempData["ErrorMessage"] = "Judge is not able to facilitate this Competition as it does not much his/her Area of Interest!";
            }
            // Create a bool value to run through the compeition judge list to check if a
            bool containsJudge = CompetitionJudgeList.Any(CompetitionJudge => CompetitionJudge.CompetitionID == competition.CompetitionID);
            if (containsJudge == false)
            {
                TempData["ErrorMessage"] = "Judge is already judging for another Competition!";
            }
            return View();
        }

        // POST: Competitor/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection, Competition competition)
        {
            if (ModelState.IsValid)
            {
                //Add competitor record to database
                competition.CompetitionID = competitionContext.Add(competition);
                TempData["SuccessMessage"] = "Competition has been successfully created!";
                //Redirect user to Competition/Create view
                return RedirectToAction("Create");
            }
            else
            {           
                //Input validation fails, return to the Create view
                //to display error message
                return View(competition);
            }

        }

        // GET: Competition/Edit/5
        public ActionResult Edit(int? id, Judge judge,  Competitor competitor)
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Admin"))
            {
            return RedirectToAction("Index", "Home");
            }
            if (id == null) { //Query string parameter not provided
            //Return to listing page, not allowed to edit
            return RedirectToAction("Index");
            }
            ViewData["JudgeList"] = GetAllJudges();
            Competition competition = competitionContext.GetDetails(id.Value);
            if (competition == null) {
            //Return to listing page, not allowed to edit
            return RedirectToAction("Index");
            }
            List<CompetitionJudgeViewModel> CompetitionJudgeList = new List<CompetitionJudgeViewModel>();
            int count = 0;
            foreach (CompetitionJudgeViewModel competitionJudge in CompetitionJudgeList)
            {
                if (competitionJudge.CompetitionID == competition.CompetitionID)
                {
                    count++;
                }
            }
            if (count < 2)
            {
                TempData["ErrorMessage"] = "There are insuffcient Judges, please assign at least 2 Judges!";
            }
            if (judge.AreaInterestID != competition.AreaInterestID)
            {
                TempData["ErrorMessage"] = "Judge is not able to facilitate this Competition as it does not much his/her Area of Interest!";
            }
            bool containsJudge = CompetitionJudgeList.Any(CompetitionJudge => CompetitionJudge.JudgeID == judge.JudgeID);
            if (containsJudge== true)
            {
                TempData["ErrorMessage"] = "Judge is already facilitating for another Competition!";
            }
            TimeSpan difference = (TimeSpan)(competition.EndDate - DateTime.Now);
            if (difference < TimeSpan.Zero)
            {
                TempData["ErrorMessage"] = "End Date has already passed, no adding or removal of Judges is allowed.";
            }
            if (competitor.CompetitorID != 0)
            {
                TempData["ErrorMessage"] = "There are already existing participants, editing is no longer allowed.";
            }
            return View(competition);
        }

        // POST: Competition/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Competition competition,Judge judge, IFormCollection collection)
        {
            ViewData["JudgeList"]=GetAllJudges();
            try
            {
                competitionContext.Update(competition,judge);
                TempData["SuccessfullyEditMsg"] = "Changes have been saved";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Competition/Delete/5
        public ActionResult Delete(int? id, Competitor competitor)
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Admin"))
            {
            return RedirectToAction("Index", "Home");
            }
            if (id == null) {
            //Return to listing page, not allowed to edit
            return RedirectToAction("Index");
            }
            Competition competition = competitionContext.GetDetails(id.Value);
            if (competition == null) {
            //Return to listing page, not allowed to edit
            return RedirectToAction("Index");
            }
            if (competitor.CompetitorID != 0)
            {
                TempData["ErrorMessage"] = "There are already existing participants, deleting is no longer allowed.";
            }
            return View(competition);
        }

        // POST: Competition/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Competition competition, IFormCollection collection)
        {
            try
            {
                competitionContext.Delete(competition.CompetitionID);
                TempData["SuccessfullyDeletedMsg"] = "Competition has been deleted.";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        private List<Judge> GetAllJudges()
        {
            // Get a list of judges from database
            List<Judge> judgeList = judgeContext.GetAllJudges();
            // Adding a select prompt at the first row of the branch list
            judgeList.Insert(0, new Judge {
                    JudgeID = 0,
                    JudgeName = "--Select--"
            });
            return judgeList;
        }

        public ActionResult Rankings(int id)
        {
            List<CompetitionSubmission> rankingList = 
                competitionContext.Rankings(competitionSubmissionContext.GetCompetitorName(id) ,id);
            return View(rankingList);
        }
    }
}
