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
            foreach (AreaInterest ai in aiList)
            {
                areaIntList.Add(new SelectListItem
                {
                    Value = ai.AreaInterestID.ToString(),
                    Text = ai.Name
                });
            }
            return areaIntList;
        }
        public JudgeViewModel MapToJudgeVM(Judge judge)
        {
            string areaInterest = "";
            if (judge.AreaInterestID != default(int))
            {
                List<AreaInterest> areaIntList = judgeContext.GetAreaOfInterest();
                foreach (AreaInterest ai in areaIntList)
                {
                    if (ai.AreaInterestID == judge.AreaInterestID)
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
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Judge"))
            {
                return RedirectToAction("Index", "Home");
            }
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
        public ActionResult ViewProfile()
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Judge"))
            {
                return RedirectToAction("Index", "Home");
            }
            Judge judge = new Judge();
            judge.JudgeID = (int)HttpContext.Session.GetInt32("judgeId");
            judge = judgeContext.GetJudgeDetails(judge.JudgeID);
            return View(judge);
        }
        public ActionResult Update()
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Judge"))
            {
                return RedirectToAction("Index", "Home");
            }
            ViewData["Salutations"] = salutDropDownList;
            ViewData["AreaInterest"] = GetAreaInterest();
            Judge judge = new Judge();
            judge.JudgeID = (int)HttpContext.Session.GetInt32("judgeId");
            judge = judgeContext.GetJudgeDetails(judge.JudgeID);
            return View(judge);
        }
        public ActionResult Delete()
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Judge"))
            {
                return RedirectToAction("Index", "Home");
            }
            Judge judge = new Judge();
            judge.JudgeID = (int)HttpContext.Session.GetInt32("judgeId");
            judge = judgeContext.GetJudgeDetails(judge.JudgeID);
            return View(judge);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(Judge judge)
        {

            //Add judge record to database
            judgeContext.Update(judge);
            //Redirect user to Judge/Create View
            return RedirectToAction("Index", "Judge");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Judge judge)
        {
            judgeContext.Delete(judge);
            HttpContext.Session.Remove("Role");
            //Redirect user to Judge/ Create View
            return RedirectToAction("Index", "Home");
        }

    }
}
