using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication.DAL;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class CommentController : Controller
    {
        private CommentDAL commentContext = new CommentDAL();
        private CompetitionDAL competitionContext = new CompetitionDAL();

        // GET: CommentController
        public ActionResult ViewComment(int id)
        {
            List<Comment> commentList = commentContext.GetAllComment(id);
            if (commentList.Count() == 0)
            {
                return RedirectToAction("ViewNoRecords", new { id = id});
            }
            else
            {
                return View(commentList);
            }
        }

        public ActionResult ViewCompetition()
        {
            List<CompetitionViewModel> competitionList =
                competitionContext.GetAllCompetition(competitionContext.GetAreaOfInterest());
            return View(competitionList);
        }

        // GET: CommentController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult ViewNoRecords(int id)
        {
            Comment comment = new Comment
            {
                CompetitionID = id,
            };
            return View(comment);
        }

        // GET: CommentController/Create
        public ActionResult Create(int id)
        {
            Comment comment = new Comment
            {
                CompetitionID = id,
            };
            return View(comment);
        }

        // POST: CommentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Comment comment, int id)
        {
            if (ModelState.IsValid)
            {
                //Add comment record to database
                comment.CommentID = commentContext.Add(comment, id);
                //Redirect user to Comment/ViewComment
                return RedirectToAction("ViewComment", new { id = id});
            }
            else
            {
                //Input validation fails, return to the Create view
                //to display error message
                return View(comment);
            }
        }

        // GET: CommentController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CommentController/Edit/5
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

        // GET: CommentController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CommentController/Delete/5
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
