using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.DAL;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class CriteriaController : Controller
    {
        private CriteriaDAL criteriaContext = new CriteriaDAL();
        private CompetitionDAL competitionContext = new CompetitionDAL();
        private JudgeDAL judgeContext = new JudgeDAL();
        //get: Criteria
        public IActionResult Index()
        {
            return View();
        }
        //Get: Criteria/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        // GET: Criteria/Create
        public ActionResult ViewCompetitionCriteria(int? id)
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Judge"))
            {
                return RedirectToAction("Index", "Home");
            }
            int judgeId = (int)HttpContext.Session.GetInt32("judgeId");
            List<CompetitionViewModel> competitionList = competitionContext.GetAssignedCompetition(
                judgeContext.InterestNameList(), judgeId);
            JudgeCompetitionCriteriaViewModel competitionCriteriaVM = new JudgeCompetitionCriteriaViewModel();
            competitionCriteriaVM.competitionList = competitionList;
            if (competitionCriteriaVM.competitionList.Count == 0)
            {
                ViewData["noComp"] = "No Competition Assigned Yet\n Please Wait To Be Assigned";
            }
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
        public ActionResult Create(int? competitionId)
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Judge"))
            {
                return RedirectToAction("Index", "Home");
            }
            JudgeCompetitionCriteriaViewModel competitionCriteriaVM = new JudgeCompetitionCriteriaViewModel();
            competitionCriteriaVM.criteriaList = criteriaContext.GetCompetitionCriteria(competitionId.Value);
            List<Criteria> criteriaList = criteriaContext.GetCompetitionCriteria(competitionId.Value);
            foreach (Criteria c in criteriaList)
            {
                if (c.CompetitionID == competitionId.Value)
                {
                    competitionCriteriaVM.TWeightage += c.Weightage;
                }
            }
            if (competitionCriteriaVM.TWeightage == 100)
            {
                HttpContext.Session.SetString("TempData", "true");
                return RedirectToAction("ViewCompetitionCriteria");
            }
            if (competitionId == null)
            {
                //Return to listing page, not allowed to edit
                return RedirectToAction("ViewCompetitionCriteria");
            }
            string CompetitionName = criteriaContext.CompetitionName(competitionId.Value);
            ViewData["Competition"] = CompetitionName;
            return View();
        }
        public ActionResult Update(int? competitionId, int critID)
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Judge"))
            {
                return RedirectToAction("Index", "Home");
            }
            CriteriaViewModel cvm = new CriteriaViewModel();
            List<Criteria> criteriaList = criteriaContext.GetCompetitionCriteria(competitionId.Value);
            cvm.criteriaList = criteriaContext.GetCompetitionCriteria(competitionId.Value);
            List<Criteria> critDetails = criteriaContext.GetCritDetails(competitionId.Value, critID);
            cvm.critDetails = critDetails;
            cvm.CriteriaID = critID;
            foreach (Criteria c in criteriaList)
            {
                if (c.CompetitionID == competitionId.Value)
                {
                    cvm.TWeightage += c.Weightage;
                }
            }
            foreach (Criteria c in critDetails)
            {
                cvm.CriteriaName = c.CriteriaName;
                cvm.Weightage = c.Weightage;
            }
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Judge"))
            {
                return RedirectToAction("Index", "Home");
            }
            if (competitionId == null)
            {
                //Return to listing page, not allowed to edit
                return RedirectToAction("ViewCompetitionCriteria");
            }
            cvm.TWeightage -= cvm.Weightage;
            string CompetitionName = criteriaContext.CompetitionName(competitionId.Value);
            ViewData["Competition"] = CompetitionName;
            return View(cvm);
        }
        public ActionResult Delete(int? competitionId, int critID)
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
               (HttpContext.Session.GetString("Role") != "Judge"))
            {
                return RedirectToAction("Index", "Home");
            }
            CriteriaViewModel cvm = new CriteriaViewModel();
            List<Criteria> criteriaList = criteriaContext.GetCompetitionCriteria(competitionId.Value);
            cvm.criteriaList = criteriaContext.GetCompetitionCriteria(competitionId.Value);
            List<Criteria> critDetails = criteriaContext.GetCritDetails(competitionId.Value, critID);
            cvm.critDetails = critDetails;
            cvm.CriteriaID = critID;
            foreach (Criteria c in criteriaList)
            {
                if (c.CompetitionID == competitionId.Value)
                {
                    cvm.TWeightage += c.Weightage;
                }
            }
            foreach (Criteria c in critDetails)
            {
                cvm.CriteriaName = c.CriteriaName;
                cvm.Weightage = c.Weightage;
            }
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Judge"))
            {
                return RedirectToAction("Index", "Home");
            }
            if (competitionId == null)
            {
                //Return to listing page, not allowed to edit
                return RedirectToAction("ViewCompetitionCriteria");
            }
            cvm.TWeightage -= cvm.Weightage;
            string CompetitionName = criteriaContext.CompetitionName(competitionId.Value);
            ViewData["Competition"] = CompetitionName;
            return View(cvm);
        }
        // POST: Judge/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Criteria criteria)
        {
            List<Criteria> criteriaList = criteriaContext.GetCompetitionCriteria(criteria.CompetitionID);
            double tWeightage = 0;
            foreach (Criteria c in criteriaList)
            {
                if (c.CompetitionID == criteria.CompetitionID)
                {
                    tWeightage += c.Weightage;
                }
            }
            tWeightage += criteria.Weightage;
            if (tWeightage > 100)
            {
                ViewData["ErrorMessage"] = "Total Criteria weightage cannot be more than 100";
                string CompetitionName = criteriaContext.CompetitionName(criteria.CompetitionID);
                ViewData["Competition"] = CompetitionName;
                CriteriaViewModel criteriaViewModel = new CriteriaViewModel()
                {
                    CompetitionID = criteria.CompetitionID,
                    CriteriaName = criteria.CriteriaName,
                    Weightage = criteria.Weightage
                };
                return View(criteriaViewModel);
            }
            else if (criteria.Weightage <= 0)
            {
                ViewData["ErrorMessage"] = "Criteria cannot be Zero or Negative Number";
                string CompetitionName = criteriaContext.CompetitionName(criteria.CompetitionID);
                ViewData["Competition"] = CompetitionName;
                CriteriaViewModel criteriaViewModel = new CriteriaViewModel()
                {
                    CompetitionID = criteria.CompetitionID,
                    CriteriaName = criteria.CriteriaName,
                    Weightage = criteria.Weightage
                };
                return View(criteriaViewModel);
            }
            if (ModelState.IsValid)
            {
                //Add criteria record to database
                criteria.CriteriaID = criteriaContext.Add(criteria);
                TempData["SuccessMessage"] = "Criteria has been successfully created!";
                //Redirect user to Criteria/Index View
                return RedirectToAction("ViewCompetitionCriteria", "Criteria");
            }
            else
            {
                //Input validation fails, return to the Create view
                //to display error message
                return View(criteria);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(Criteria criteria)
        {
            List<Criteria> criteriaList = criteriaContext.GetCompetitionCriteria(criteria.CompetitionID);
            double tWeightage = 0;
            foreach (Criteria c in criteriaList)
            {
                if (c.CompetitionID == criteria.CompetitionID)
                {
                    tWeightage += c.Weightage;
                }
            }
            tWeightage -= criteriaContext.GetWeightage(criteria.CriteriaID);
            tWeightage += criteria.Weightage;
            if (tWeightage > 100)
            {
                ViewData["ErrorMessage"] = "Total Criteria weightage cannot be more than 100";
                string CompetitionName = criteriaContext.CompetitionName(criteria.CompetitionID);
                ViewData["Competition"] = CompetitionName;
                CriteriaViewModel criteriaViewModel = new CriteriaViewModel()
                {
                    CompetitionID = criteria.CompetitionID,
                    CriteriaName = criteria.CriteriaName,
                    Weightage = criteria.Weightage
                };
                return View(criteriaViewModel);
            }
            else if (criteria.Weightage <= 0)
            {
                ViewData["ErrorMessage"] = "Criteria cannot be Zero or Negative Number";
                string CompetitionName = criteriaContext.CompetitionName(criteria.CompetitionID);
                ViewData["Competition"] = CompetitionName;
                CriteriaViewModel criteriaViewModel = new CriteriaViewModel()
                {
                    CompetitionID = criteria.CompetitionID,
                    CriteriaName = criteria.CriteriaName,
                    Weightage = criteria.Weightage
                };
                return View(criteriaViewModel);
            }
            if (ModelState.IsValid)
            {
                //Add criteria record to database
                criteriaContext.Update(criteria);
                TempData["SuccessMessage"] = "Criteria has been successfully created!";
                //Redirect user to Criteria/Index View
                return RedirectToAction("ViewCompetitionCriteria", "Criteria");
            }
            else
            {
                //Input validation fails, return to the Create view
                //to display error message
                return View(criteria);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Criteria criteria)
        {
            if (ModelState.IsValid)
            {
                criteriaContext.Delete(criteria);
                return RedirectToAction("ViewCompetitionCriteria", "Criteria");
            }
            else
            {
                ViewData["Error"] = "Criteria cannot be deleted";
                return View(criteria);
            }
        }
    }
}
