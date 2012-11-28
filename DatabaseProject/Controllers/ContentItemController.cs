﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DatabaseProject.Models;

namespace DatabaseProject.Controllers
{
    public class ContentItemController : Controller
    {
        //
        // GET: /ContentItem/
        [Authorize]
        public ActionResult Index(int page = 0)
        {
            QueryController SQL = new QueryController();
            PostListModel post_list = SQL.get_posts(page);
            return View(post_list);
            //return Content("Hello, World!");
        }

        //
        // GET: /ContentItem/Details/5

        public ActionResult Write(/*int id*/)
        {
            return View();
        }

        //
        // GET: /ContentItem/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /ContentItem/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /ContentItem/Edit/5

        public ActionResult Edit(/*int id*/)
        {
            return View();
        }

        //
        // POST: /ContentItem/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /ContentItem/Delete/5

        public ActionResult Delete(/*int id*/)
        {
            return View();
        }

        //
        // POST: /ContentItem/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}