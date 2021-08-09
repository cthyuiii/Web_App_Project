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
    public class AreaInterestController : Controller
    {
        private AreaInterestDAL areainterestContext = new AreaInterestDAL();
        private CompetitionDAL competitionContext = new CompetitionDAL();
        private CompetitionSubmissionDAL competitionSubContext = new CompetitionSubmissionDAL();

        // GET: AreaInterest
        public ActionResult Index(int? id)
        {
            // Check if Admin role is logged in before proceeding, else return back login page. This validation is present in all GET Methods.
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Admin"))
            {
                return RedirectToAction("Index", "Home");
            }

            AreaInterestViewModel areaInterestVM = new AreaInterestViewModel();
            areaInterestVM.areaofinterestnameList = areainterestContext.GetAreaOfInterest();
            // Check if AreaInterest (id) presents in the query string
            if (id != null)
            {
                ViewData["selectedAreaInterest"] = id.Value;
                // Get Area of interest of the selected ID
                areaInterestVM.competitionList = areainterestContext.GetAreaInterestCompetition(id.Value);
            }
            else
            {
                ViewData["selectedAreaInterest"] = "";
            }
            return View(areaInterestVM);
        }

        // GET: AreaofInterestController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AreaInterestController/Create
        public ActionResult Create()
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // POST: AreaInterestController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AreaInterest areaInterest, IFormCollection collection)
        {
            // To validate Area of interest when creating
            try
            {
                areaInterest.AreaInterestID = areainterestContext.Add(areaInterest);
                TempData["SuccessMessage"] = "Area of Interest has been successfully created!";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AreaInterestController/Delete/5
        public ActionResult Delete(int? id)
        {
             if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            // If page id doesn't exist or is already deleted, return back to home page.
            if (id == null) 
            {
                //Return to listing page, not allowed to edit
                return RedirectToAction("Index");
            }
            // If area of interest doesn't exist return back to home page.
            AreaInterest areaInterest = areainterestContext.GetDetails(id.Value);
            if (areaInterest == null) 
            {
                //Return to listing page, not allowed to edit
                return RedirectToAction("Index");
            }
            // If selected area of interest already has a past competition record, deleting is prevented.
            int? competitionId = HttpContext.Session.GetInt32("competitionId");
            if(competitionId == null)
            {
                TempData["ErrorMessage"] = "Unable to delete Area Of Interest as there is already an existing competition record";
                ViewData["ButtonState"] = "";
            }
            return View(areaInterest);
        }

        // POST: AreaInterestController/Delete/2
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(AreaInterest areaInterest, IFormCollection collection)
        {
            try
            {
                areainterestContext.Delete(areaInterest.AreaInterestID);
                TempData["SuccessfullyDeletedMsg"] = "Area of Interest has been deleted.";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
