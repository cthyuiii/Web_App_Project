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
    public class JudgeController : Controller
    {
        private JudgeDAL judgeContext = new JudgeDAL();
        private CompetitionDAL competitionContext = new CompetitionDAL();
        // List that stores the salutations
        private List<string> salutList = new List<string> { "Mr", "Mrs", "Mdm", "Dr" };
        private List<SelectListItem> salutDropDownList = new List<SelectListItem>();
        public JudgeController()
        {
            foreach (string salut in salutList)
            {
                salutDropDownList.Add(
                    new SelectListItem
                    {
                        Text = salut,
                    });
            }
        }
        private List<SelectListItem> GetAreaInterest()
        {
            List<SelectListItem> areaIntList = new List<SelectListItem>();
            List<AreaInterest> aiList = judgeContext.GetAreaOfInterest();
            foreach(AreaInterest ai in aiList)
            {
                areaIntList.Add(new SelectListItem
                {
                    Value = ai.AreaInterestID.ToString(),
                    Text = ai.Name
                }) ;
            }
            return areaIntList;
        }
        public JudgeViewModel MapToJudgeVM(Judge judge)
        {
            string areaInterest = "";
            if (judge.AreaInterestID != default(int))
            {
                List<AreaInterest> areaIntList = judgeContext.GetAreaOfInterest();
                foreach(AreaInterest ai in areaIntList)
                {
                    if(ai.AreaInterestID== judge.AreaInterestID)
                    {
                        areaInterest = ai.Name;
                        break;
                    }
                }
            }
            JudgeViewModel judgeVM = new JudgeViewModel
            {
                JudgeID = judge.JudgeID,
                JudgeName = judge.JudgeName,
                Salutation = judge.Salutation,
                AreaInterest = areaInterest,
                EmailAddr = judge.EmailAddr
            };
            return judgeVM;
        }

        //GET: judge
        public IActionResult Index(Judge judge)
        {
            List<JudgeViewModel> jList = new List<JudgeViewModel>();
            List<Judge> judgeList = judgeContext.GetAllJudges();
            foreach (Judge j in judgeList)
            {
                jList.Add(MapToJudgeVM(j));
            }
            return View(jList);
        }
        // GET: Judge/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Judge/Create
        public ActionResult Create()
        {
            ViewData["Salutations"] = salutDropDownList;
            ViewData["AreaInterest"] = GetAreaInterest();
            return View();
        }

        //POST: Judge/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Judge judge)
        {
            ViewData["Salutations"] = salutDropDownList;
            ViewData["AreaInterest"] = GetAreaInterest();
            if (ModelState.IsValid)
            {
                //Add judge record to database
                judge.JudgeID = judgeContext.Add(judge);
                TempData["SuccessMessage"] = "Judge Profile has been successfully created!";
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

    }
}
