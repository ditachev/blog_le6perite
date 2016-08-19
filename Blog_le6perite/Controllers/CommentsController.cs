﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Blog_le6perite.Models;
using Blog_le6perite.Extensions;

namespace Blog_le6perite.Controllers
{
    public class CommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //// GET: Comments
        //public ActionResult Index()
        //{
        //    var comments = db.Comments.Include(c => c.Author).Include(c => c.Post);
        //    return View(comments.ToList());
        //}

        //// GET: Comments/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Comment comment = db.Comments.Find(id);
        //    if (comment == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(comment);
        //}

        //// GET: Comments/Create
        //public ActionResult Create()
        //{
        //    ViewBag.AuthorId = new SelectList(db.ApplicationUsers, "Id", "FullName");
        //    ViewBag.PostId = new SelectList(db.Posts, "Id", "Title");
        //    return View();
        //}

        //// POST: Comments/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,Text,Date,AuthorId,PostId")] Comment comment)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Comments.Add(comment);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.AuthorId = new SelectList(db.ApplicationUsers, "Id", "FullName", comment.AuthorId);
        //    ViewBag.PostId = new SelectList(db.Posts, "Id", "Title", comment.PostId);
        //    return View(comment);
        //}

        //// GET: Comments/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Comment comment = db.Comments.Find(id);
        //    if (comment == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.AuthorId = new SelectList(db.ApplicationUsers, "Id", "FullName", comment.AuthorId);
        //    ViewBag.PostId = new SelectList(db.Posts, "Id", "Title", comment.PostId);
        //    return View(comment);
        //}

        //// POST: Comments/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,Text,Date,AuthorId,PostId")] Comment comment)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(comment).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.AuthorId = new SelectList(db.ApplicationUsers, "Id", "FullName", comment.AuthorId);
        //    ViewBag.PostId = new SelectList(db.Posts, "Id", "Title", comment.PostId);
        //    return View(comment);
        //}

        // GET: Comments/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null || (comment.Author.UserName != User.Identity.Name && !User.IsInRole("Administrators")))
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            Comment comment = db.Comments.Find(id);
            var postId = comment.PostId;
            db.Comments.Remove(comment);
            db.SaveChanges();
            this.AddNotification("Comment deleted", NotificationType.SUCCESS);
            return RedirectToAction($"../Posts/Details/{postId}");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}