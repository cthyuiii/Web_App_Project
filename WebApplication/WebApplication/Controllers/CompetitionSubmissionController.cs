using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication.DAL;
using WebApplication.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace WebApplication.Controllers
{
    public class CompetitionSubmissionController : Controller
    {
        private CompetitionSubmissionDAL competitorContext = new CompetitionSubmissionDAL();
        private CompetitionDAL competitionContext = new CompetitionDAL();

        // GET: CompetitorController
        public ActionResult ViewCompetition()
        {
            // Get all competitions in database
            List<CompetitionViewModel> competitionList =
                competitionContext.GetAllCompetition(competitionContext.GetAreaOfInterest());
            return View(competitionList);
        }

        public ActionResult ViewCompetitionSubmission(int id)
        {
            // Get all competition submissions for the selected competition from database
            List<CompetitionSubmission> competitorsubmissionList =
                competitorContext.GetAllCompetitionSubmission(
                    competitorContext.GetCompetitorName(id), id);
            return View(competitorsubmissionList);
        }

        public ActionResult ViewFile(int competitionid, int competitorid)
        {
            // Get all possible file names and store in it's variable as relative path
            string jpgfilename = @"wwwroot\CompetitionWork\" + "File_" + competitorid + "_" + competitionid + ".jpg";
            string mp4filename = @"wwwroot\CompetitionWork\" + "File_" + competitorid + "_" + competitionid + ".mp4";
            string pdffilename = @"wwwroot\CompetitionWork\" + "File_" + competitorid + "_" + competitionid + ".pdf";
            // If the competitor have a "jpg" file in the competition
            if (System.IO.File.Exists(jpgfilename) == true)
            {
                // Init file
                Files file = new Files
                {
                    Extension = "jpg",
                    FileName = jpgfilename,
                };
                return View(file);
            }
            // If the competitor have a "mp4" file in the competition
            else if (System.IO.File.Exists(mp4filename) == true)
            {
                Files file = new Files
                {
                    CompetitionID = competitionid,
                    Extension = "mp4",
                    FileName = mp4filename,
                };
                return View(file);
            }
            // If the competitor have a "pdf" file in the competition
            else if (System.IO.File.Exists(pdffilename) == true)
            {
                Files file = new Files
                {
                    CompetitionID = competitionid,
                    Extension = "pdf",
                    FileName = pdffilename,
                };
                return View(file);
            }
            // If the competitor does not have any files uploaded in the competition
            else
            {
                Files file = new Files
                {
                    CompetitionID = competitionid,
                    Extension = "none"
                };
                return View(file);
            }
        }

        public ActionResult ViewFileFromRanking(int competitionid, int competitorid)
        {
            // Basically the same codes from method "ViewFile"
            string jpgfilename = @"wwwroot\CompetitionWork\" + "File_" + competitorid + "_" + competitionid + ".jpg";
            string mp4filename = @"wwwroot\CompetitionWork\" + "File_" + competitorid + "_" + competitionid + ".mp4";
            string pdffilename = @"wwwroot\CompetitionWork\" + "File_" + competitorid + "_" + competitionid + ".pdf";
            if (System.IO.File.Exists(jpgfilename) == true)
            {
                Files file = new Files
                {
                    Extension = "jpg",
                    FileName = jpgfilename,
                };
                return View(file);
            }
            else if (System.IO.File.Exists(mp4filename) == true)
            {
                Files file = new Files
                {
                    CompetitionID = competitionid,
                    Extension = "mp4",
                    FileName = mp4filename,
                };
                return View(file);
            }
            else if (System.IO.File.Exists(pdffilename) == true)
            {
                Files file = new Files
                {
                    CompetitionID = competitionid,
                    Extension = "pdf",
                    FileName = pdffilename,
                };
                return View(file);
            }
            else
            {
                Files file = new Files
                {
                    CompetitionID = competitionid,
                    Extension = "none"
                };
                return View(file);
            }
        }

        public ActionResult Vote(int competitorid, int competitionid)
        {
            // Get all the details from the selected competition
            Competition competition = competitionContext.GetDetails(competitionid);
            // Get the session string stored in the user's browser to check if the user had voted for
            // anyone in the selected competition
            string voteStatus = HttpContext.Session.GetString(competitionid.ToString());
            // If the session string is "Yes", means that the user had already voted for somebody in the
            // competition
            if (voteStatus == "Yes")
            {
                // Tell user that they had voted
                TempData["Message"] = "You have already voted!";
                // Pass vote's competitionID in view so that "Back to List" will redirect user to the
                // correct competition submission page
                Vote vote = new Vote
                {
                    CompetitionID = competitionid
                };
                return View(vote);
            }
            // If the competition have not even started yet
            else if (competition.StartDate > DateTime.Now)
            {
                // Tell user that voting have not started yet
                TempData["Message"] = "Voting have not started yet!";
                Vote vote = new Vote
                {
                    CompetitionID = competitionid
                };
                return View(vote);
            }
            else
            {
                // If competition is ongoing
                if (competition.StartDate < DateTime.Now && competition.EndDate > DateTime.Now)
                {
                    // Vote competitor in database
                    competitorContext.Vote(competitionid, competitorid);
                    // Set the session string stored in the user's browser
                    HttpContext.Session.SetString(competitionid.ToString(), "Yes");
                    // Tell user that the voting is successful
                    TempData["Message"] = "Voting successful!";
                    Vote vote = new Vote
                    {
                        CompetitionID = competitionid
                    };
                    return View(vote);
                }
                else
                {
                    // Tell user that the voting have already ended
                    TempData["Message"] = "Voting have already ended!";
                    Vote vote = new Vote
                    {
                        CompetitionID = competitionid
                    };
                    return View(vote);
                }
            }
        }

        // GET: CompetitorSubmissionController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CompetitorSubmissionController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CompetitorSubmissionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: CompetitorSubmissionController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CompetitorSubmissionController/Edit/5
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

        // GET: CompetitorSubmissionController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CompetitorSubmissionController/Delete/5
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
