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
        // POST: /Privacy/AddToGroup
        [HttpPost]
        public ActionResult AddToGroup(PrivacyModel privacyModel)
        {
            try
            {
                // TODO: Add insert logic here that presents users NOT in selected group
                return View();
            }
            catch
            {
                return RedirectToAction("Add"); // User list query fails, ask again group
            }
        }

        //
        // POST: /Privacy/AddUserToGroup
        [HttpPost]
        public ActionResult AddUserToGroup(PrivacyModel privacyModel)
        {
            try
            {
                // TODO: Add insert logic there that includes selected user into selected group
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("AddToGroup"); // Insert query fails, ask again for user 
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
                //TODO: Use query here that pulls in relevant group to remove from
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
                // TODO: User query here that removes user from current group
                return RedirectToAction("Index"); // Returns to main page of Privacy section after successful deletion
            }
            catch
            {
                return RedirectToAction("RemoveUser"); // Query failed for some reason...Ask for user again
            }
        }
    }
}
