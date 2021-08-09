using Microsoft.AspNetCore.Mvc;
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
    public class JudgeViewRankingsController : Controller
    {
        private CompetitionDAL competitionContext = new CompetitionDAL();
        private JudgeViewSubmissionsDAL jvmContext = new JudgeViewSubmissionsDAL();
        private CriteriaDAL criteriaContext = new CriteriaDAL();
        private JudgeViewRankingsDAL jvrContext = new JudgeViewRankingsDAL();
        public IActionResult Index()
        {
            return View();
        }
        public ActionResult ViewCompetitions(int? id)
        {
            int judgeId = (int)HttpContext.Session.GetInt32("judgeId");
            List<JudgeViewSubmissions> competitionList = jvmContext.GetCompetitionJudge(
                competitionContext.GetAreaOfInterest(), judgeId);
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
        public ActionResult Update(int CompetitorId, int CompetitionId)
        {
            JudgeViewRankings jvr = new JudgeViewRankings();
            jvr = jvrContext.GetRankingDetails(CompetitionId,CompetitorId);
            jvr.CompetitionName = criteriaContext.CompetitionName(CompetitionId);
            jvr.CompetitionID = CompetitionId;
            jvr.CompetitorID = CompetitorId;
            Competition rrDate = competitionContext.GetDetails(CompetitionId);
            jvr.ResultReleasedDate = rrDate.ResultReleasedDate;
            jvr.scoresList = jvrContext.scoresList(CompetitionId, CompetitorId);
            jvr.weightageList = jvrContext.WeightageList(CompetitionId);
            jvr.markSet = false;
            foreach(var weightage in jvr.weightageList)
            {
                foreach(var score in jvr.scoresList)
                {
                    if(weightage.CriteriaID==score.CriteriaID)
                    {
                        jvr.totalMark += (Convert.ToDouble(weightage.Weightage) * (Convert.ToDouble(score.Scores) / 10));
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            if(jvr.weightageList.Count == jvr.scoresList.Count)
            {
                jvr.markSet = true;
            }
            if (jvr.ResultReleasedDate <= DateTime.Now)
            {
                ViewData["Done"] = "Competition Has Ended! No editting of scores allowed";
            }
            else
            {
                ViewData["NotDone"] = "Competition Result is finalizing on" + jvr.ResultReleasedDate.ToString();
            }
            return View(jvr);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(JudgeViewRankings jvr)
        {
            if (ModelState.IsValid)
            {
                jvrContext.Update(jvr);
                return RedirectToAction("ViewCompetitions", "JudgeViewRankings", jvr);
            }
            else
            {
                ViewData["ErrorMessage"] = "Error";
                JudgeViewRankings judgeVM = new JudgeViewRankings()
                {
                    CompetitionName = jvr.CompetitionName,
                    CompetitionID = jvr.CompetitionID,
                    CompetitorID = jvr.CompetitorID,
                    CriteriaID = jvr.CriteriaID,
                    totalMark = jvr.totalMark,
                    CriteriaName = jvr.CriteriaName,
                    Ranking = jvr.Ranking,
                    Appeal = jvr.Appeal,
                    VoteCount = jvr.VoteCount
                };
                if (jvr.weightageList.Count == jvr.scoresList.Count)
                {
                    jvr.markSet = true;
                }
                if (jvr.ResultReleasedDate <= DateTime.Now)
                {
                    ViewData["Done"] = "Competition Has Ended! No editting of scores allowed";
                }
                else
                {
                    ViewData["NotDone"] = "Competition Result is finalizing on" + jvr.ResultReleasedDate.ToString();
                }
                return View(jvr);
            }
        }
    }
}
