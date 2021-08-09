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

namespace WebApplication.Controllers
{
    public class CompetitionSubmissionController : Controller
    {
        private CompetitionSubmissionDAL competitorContext = new CompetitionSubmissionDAL();
        private CompetitionDAL competitionContext = new CompetitionDAL();

        // GET: CompetitorController
        public ActionResult ViewCompetition()
        {
            List<CompetitionViewModel> competitionList =
                competitionContext.GetAllCompetition(competitionContext.GetAreaOfInterest());
            return View(competitionList);
        }

        public ActionResult ViewCompetitionSubmission(int id)
        {
            List<CompetitionSubmission> competitorsubmissionList = 
                competitorContext.GetAllCompetitionSubmission(
                    competitorContext.GetCompetitorName(id), id);
            return View(competitorsubmissionList);
        }

        public ActionResult ViewFile(int competitionid, int competitorid)
        {
            CompetitionSubmission competitionSubmission = new CompetitionSubmission
            {
                CompetitionID = competitionid,
                CompetitorID = competitorid,
            };
            string jpgfilename = "File_" + competitorid + "_" + competitionid + ".jpg";
            string mp4filename = "File_" + competitorid + "_" + competitionid + ".mp4";
            string pdffilename = "File_" + competitorid + "_" + competitionid + ".pdf";
            if (System.IO.File.Exists(jpgfilename))
            {
                Files file = new Files 
                { 
                    Extension = "jpg",
                    FileName = jpgfilename,
                };
                return View(file);
            }
            else if (System.IO.File.Exists(mp4filename))
            {
                Files file = new Files
                {
                    CompetitionID = competitionid,
                    Extension = "mp4",
                    FileName = mp4filename,
                };
                return View(file);
            }
            else if (System.IO.File.Exists(pdffilename))
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

        public ActionResult ViewFileFromRanking(int competitionid, int competitorid)
        {
            CompetitionSubmission competitionSubmission = new CompetitionSubmission
            {
                CompetitionID = competitionid,
                CompetitorID = competitorid,
            };
            string jpgfilename = "File_" + competitorid + "_" + competitionid + ".jpg";
            string mp4filename = "File_" + competitorid + "_" + competitionid + ".mp4";
            string pdffilename = "File_" + competitorid + "_" + competitionid + ".pdf";
            if (System.IO.File.Exists(jpgfilename))
            {
                Files file = new Files
                {
                    Extension = "jpg",
                    FileName = jpgfilename,
                };
                return View(file);
            }
            else if (System.IO.File.Exists(mp4filename))
            {
                Files file = new Files
                {
                    CompetitionID = competitionid,
                    Extension = "mp4",
                    FileName = mp4filename,
                };
                return View(file);
            }
            else if (System.IO.File.Exists(pdffilename))
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
            Competition competition = competitionContext.GetDetails(competitionid);
            string voteStatus = HttpContext.Session.GetString("Vote");
            if (voteStatus == "Yes")
            {
                TempData["Message"] = "You have already voted!";
                Vote vote = new Vote
                {
                    CompetitionID = competitionid
                };
                return View(vote);
            }
            else if (competition.StartDate > DateTime.Now)
            {
                TempData["Message"] = "Voting have not starting yet!";
                Vote vote = new Vote
                {
                    CompetitionID = competitionid
                };
                return View(vote);
            }
            else
            {
                if (competition.StartDate < DateTime.Now && competition.EndDate > DateTime.Now)
                {
                    competitorContext.Vote(competitionid, competitorid);
                    HttpContext.Session.SetString("Vote", "Yes");
                    TempData["Message"] = "Voting successful!";
                    Vote vote = new Vote
                    {
                        CompetitionID = competitionid
                    };
                    return View(vote);
                }
                else
                {
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
