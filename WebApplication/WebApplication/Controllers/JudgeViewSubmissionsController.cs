using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.DAL;
using WebApplication.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text.RegularExpressions;

namespace WebApplication.Controllers
{
    public class JudgeViewSubmissionsController : Controller
    {
        private CompetitionDAL competitionContext = new CompetitionDAL();
        private JudgeViewSubmissionsDAL jvmContext = new JudgeViewSubmissionsDAL();
        private CriteriaDAL criteriaContext = new CriteriaDAL();
        private JudgeDAL judgeContext = new JudgeDAL();

        private IWebHostEnvironment Environment;
        public JudgeViewSubmissionsController(IWebHostEnvironment _environment)
        {
            Environment = _environment;
        }
        public IActionResult Index()
        {
            return View();
        }
        public ActionResult Details(int id)
        {
            return View();
        }
        public ActionResult ViewCompetitions(int? id)
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Judge"))
            {
                return RedirectToAction("Index", "Home");
            }
            int judgeId = (int)HttpContext.Session.GetInt32("judgeId");
            List<JudgeViewSubmissions> competitionList = jvmContext.GetCompetitionJudge(
                judgeContext.InterestNameList(), judgeId);
            JudgeViewSubmissions competitionCriteriaVM = new JudgeViewSubmissions();
            competitionCriteriaVM.competitionList = competitionList;
            if (competitionCriteriaVM.competitionList.Count == 0)
            {
                ViewData["noComp"] = "No Competition Assigned Yet\n Please Wait To Be Assigned";
            }
            if (id != null)
            {
                ViewData["selectedCompetitionNo"] = id.Value;
                List<CompetitionCompetitorViewModel> competitorList = jvmContext.GetAllCompetitor(id.Value);
                competitionCriteriaVM.competitorList = competitorList;
                string CompetitionName = criteriaContext.CompetitionName(id.Value);
                ViewData["Competition"] = CompetitionName;

            }
            else
            {
                ViewData["selectedCompetitionNo"] = "";
            }
            return View(competitionCriteriaVM);
        }
        public ActionResult ViewSubmissions(int CompetitorId, int CompetitionId)
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Judge"))
            {
                return RedirectToAction("Index", "Home");
            }
            JudgeViewSubmissions jVM = jvmContext.GetCompetitorSubmission(CompetitorId, CompetitionId);
            string CompetitionName = criteriaContext.CompetitionName(CompetitionId);
            ViewData["Competition"] = CompetitionName;
            jVM.CompetitionID = CompetitionId;
            jVM.CompetitorID = CompetitorId;
            Competition resultReleasedDate = competitionContext.GetDetails(CompetitionId);
            jVM.ResultReleasedDate = resultReleasedDate.ResultReleasedDate;
            jVM.criteriaList = criteriaContext.GetCompetitionCriteria(CompetitionId);
            jVM.critCheckList = criteriaContext.GetCompetitionCriteria(CompetitionId);
            List<JudgeViewSubmissions> Vmcheck = jvmContext.VMCheckList(CompetitionId, CompetitorId);
            jVM.VMCheckList = Vmcheck;
            foreach (var item in jVM.critCheckList.ToList())
            {
                foreach (var Item in jVM.VMCheckList)
                {
                    if (item.CriteriaID == Item.CriteriaID)
                    {
                        jVM.critCheckList.Remove(item);
                    }
                }
            }
            if (jVM.ResultReleasedDate <= DateTime.Now)
            {
                ViewData["Date"] = "Competition Has Ended! No editting of scores allowed";
            }
            else
            {
                ViewData["Date"] = "Competition Result is finalizing on" + jVM.ResultReleasedDate.ToString();
            }
            //Fetch all files in the Folder (Directory).
            //string[]filePaths = Directory.GetFiles(Path.Combine(this.Environment.WebRootPath, "files\\"));
            if (jVM.FileSubmitted != null)
            {
                jVM.FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\CompetitionWork", jVM.FileSubmitted);
            }
            else if (jVM.FileSubmitted == null)
            {
                ViewData["FileSubmittedFalse"] = "No File Submitted";
            }
            jVM.scoreList = jvmContext.GetScores(CompetitionId, CompetitorId);
            return View(jVM);
        }
        public FileResult DownloadFile(string fileName)
        {
            //Build the File Path.
            string path = Path.Combine(this.Environment.WebRootPath, "CompetitionWork\\") + fileName;

            //Read the File data into Byte Array.
            byte[] bytes = System.IO.File.ReadAllBytes(path);
            return File(bytes, "application/octet-stream", fileName);
            //Send the File to Download.
        }
        public ActionResult Create(int CompetitorId, int CompetitionId, int CriteriaId)
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Judge"))
            {
                return RedirectToAction("Index", "Home");
            }
            JudgeViewSubmissions jVM = new JudgeViewSubmissions();
            jVM = jvmContext.GetCompetitorSubmission(CompetitorId, CompetitionId);
            string CompetitionName = criteriaContext.CompetitionName(CompetitionId);
            jVM.CompetitionName = CompetitionName;
            jVM.CompetitionID = CompetitionId;
            jVM.CompetitorID = CompetitorId;
            jVM.CriteriaID = CriteriaId;
            jVM.CriteriaName = criteriaContext.GetCritName(CriteriaId);
            Competition resultReleasedDate = competitionContext.GetDetails(CompetitionId);
            jVM.ResultReleasedDate = resultReleasedDate.ResultReleasedDate;
            List<Criteria> criteriaList = criteriaContext.GetCompetitionCriteria(CompetitionId);
            foreach (Criteria c in criteriaList)
            {
                if (c.CompetitionID == CompetitionId)
                {
                    jVM.TWeightage += c.Weightage;
                }
            }
            if (jVM.TWeightage != 100)
            {
                HttpContext.Session.SetString("TempData", "true");
                JudgeViewSubmissions judgeVM = new JudgeViewSubmissions()
                {
                    CompetitionName = jVM.CompetitionName,
                    CompetitionID = jVM.CompetitionID,
                    CompetitorID = jVM.CompetitorID,
                    CriteriaID = jVM.CriteriaID,
                    CriteriaName = jVM.CriteriaName,
                    ResultReleasedDate = jVM.ResultReleasedDate
                };
                return RedirectToAction("ViewSubmissions", judgeVM);
            }
            //Fetch all files in the Folder (Directory).
            //string[]filePaths = Directory.GetFiles(Path.Combine(this.Environment.WebRootPath, "files\\"));
            if (jVM.FileSubmitted != null)
            {
                jVM.FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\CompetitionWork", jVM.FileSubmitted);
            }
            else if (jVM.FileSubmitted == null)
            {
                ViewData["FileSubmittedFalse"] = "No File Submitted";
            }
            return View(jVM);
        }
        public ActionResult Update(int CompetitorId, int CompetitionId, int CriteriaId)
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Judge"))
            {
                return RedirectToAction("Index", "Home");
            }
            JudgeViewSubmissions jVM = new JudgeViewSubmissions();
            jVM = jvmContext.GetCompetitorSubmission(CompetitorId, CompetitionId);
            string CompetitionName = criteriaContext.CompetitionName(CompetitionId);
            jVM.CompetitionName = CompetitionName;
            jVM.CompetitionID = CompetitionId;
            jVM.CompetitorID = CompetitorId;
            jVM.CriteriaID = CriteriaId;
            jVM.CriteriaName = criteriaContext.GetCritName(CriteriaId);
            jVM.Score = jvmContext.GetScore(jVM);
            Competition resultReleasedDate = competitionContext.GetDetails(CompetitionId);
            jVM.ResultReleasedDate = resultReleasedDate.ResultReleasedDate;
            //Fetch all files in the Folder (Directory).
            //string[]filePaths = Directory.GetFiles(Path.Combine(this.Environment.WebRootPath, "files\\"));
            if (jVM.FileSubmitted != null)
            {
                jVM.FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files", jVM.FileSubmitted);
            }
            else if (jVM.FileSubmitted == null)
            {
                ViewData["FileSubmittedFalse"] = "No File Submitted";
            }
            return View(jVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(JudgeViewSubmissions jvm)
        {
            if (ModelState.IsValid)
            {
                jvmContext.Add(jvm);
                return RedirectToAction("ViewSubmissions", "JudgeViewSubmissions", jvm);
            }
            else
            {
                ViewData["ErrorMessage"] = "Error";
                JudgeViewSubmissions judgeVM = new JudgeViewSubmissions()
                {
                    FileSubmitted = jvm.FileSubmitted,
                    CompetitionName = jvm.CompetitionName,
                    CompetitionID = jvm.CompetitionID,
                    CompetitorID = jvm.CompetitorID,
                    CriteriaID = jvm.CriteriaID,
                    Score = jvm.Score,
                    CriteriaName = jvm.CriteriaName,
                    ResultReleasedDate = jvm.ResultReleasedDate
                };
                if (judgeVM.FileSubmitted != null)
                {
                    judgeVM.FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files", judgeVM.FileSubmitted);
                }
                else if (judgeVM.FileSubmitted == null)
                {
                    ViewData["FileSubmittedFalse"] = "No File Submitted";
                }
                if (judgeVM.ResultReleasedDate <= DateTime.Now)
                {
                    ViewData["Done"] = "Competition Has Ended! No editting of scores allowed";
                }
                else
                {
                    ViewData["NotDone"] = "Competition Result is finalizing on" + judgeVM.ResultReleasedDate.ToString();
                }
                return View(judgeVM);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(JudgeViewSubmissions jvm)
        {
            if (ModelState.IsValid)
            {
                jvmContext.Update(jvm);
                return RedirectToAction("ViewSubmissions", "JudgeViewSubmissions", jvm);
            }
            else
            {
                JudgeViewSubmissions judgeVM = new JudgeViewSubmissions()
                {
                    FileSubmitted = jvm.FileSubmitted,
                    CompetitionName = jvm.CompetitionName,
                    CompetitionID = jvm.CompetitionID,
                    CompetitorID = jvm.CompetitorID,
                    CriteriaID = jvm.CriteriaID,
                    Score = jvm.Score,
                    CriteriaName = jvm.CriteriaName,
                    ResultReleasedDate = jvm.ResultReleasedDate
                };
                if (judgeVM.FileSubmitted != null)
                {
                    judgeVM.FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files", judgeVM.FileSubmitted);
                }
                else if (judgeVM.FileSubmitted == null)
                {
                    ViewData["FileSubmittedFalse"] = "No File Submitted";
                }
                if (judgeVM.ResultReleasedDate <= DateTime.Now)
                {
                    ViewData["Date"] = "Competition Has Ended! No editting of scores allowed";
                }
                else
                {
                    ViewData["Date"] = "Competition Result is finalizing on" + judgeVM.ResultReleasedDate.ToString();
                }
                return View(judgeVM);
            }
        }
    }
}
