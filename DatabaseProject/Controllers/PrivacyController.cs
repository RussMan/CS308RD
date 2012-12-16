using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DatabaseProject.Models;

namespace DatabaseProject.Controllers
{
    public class PrivacyController : Controller
    {
        //
        // GET: /Privacy/
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        //
        // Post: /Privacy/ViewGroupUsers
        [Authorize]
        [HttpPost]
        public ActionResult ViewGroupUsers(PrivacyModel privacyModel)
        {
            //Add your query command here using the 'group' parameter
            //return Content(privacyModel.group);
            return View();
        }

        //
        // GET: /Privacy/Add
        public ActionResult Add()
        {
            return View();
        }

        //
        // POST: /Privacy/Add
        [HttpPost]
        public ActionResult Add(PrivacyModel privacyModel)
        {
            try
            {
                // TODO: Add insert logic here
                return Content(privacyModel.group + " , " + privacyModel.user);
                //return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Privacy/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Privacy/Edit/5

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
        // GET: /Privacy/Remove/
        public ActionResult Remove()
        {
            return View(); // Presents user with groups to delet user from
        }

        //
        // POST: /Privacy/RemoveUser
        [HttpPost]
        public ActionResult RemoveUser(PrivacyModel privacyModel)
        {
            try
            {
                //Use query here that pulls in relevant group to remove from
                return View(); // Presents users in selected group
            }
            catch
            {
                return RedirectToAction("Remove"); // Query failed...Ask for group again
            }
        }

        [HttpPost]
        public ActionResult RemoveUserFromGroup(PrivacyModel privacyModel)
        {
            try
            {
                // User query here that removes user from current group
                return RedirectToAction("Index"); // Returns to main page of Privacy section after successful deletion
            }
            catch
            {
                return RedirectToAction("RemoveUser"); // Query failed for some reason...Ask for user again
            }
        }
    }
}
