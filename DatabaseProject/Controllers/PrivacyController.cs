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
            PrivacyModel group_data = new PrivacyModel { groups = new List<string>() };
            QueryController queryCommand = new QueryController();
            group_data.groups = queryCommand.get_friendship((int)HttpContext.Session["userSessionID"]);
            return View(group_data);
        }

        //
        // Post: /Privacy/ViewGroupUsers
        [Authorize]
        [HttpPost]
        public ActionResult ViewGroupUsers(PrivacyModel privacyModel)
        {
            privacyModel.users = new List<KeyValuePair<int, KeyValuePair<string, string>>>();
            QueryController queryCommand = new QueryController();
            privacyModel.users = queryCommand.get_users((int)HttpContext.Session["userSessionID"], privacyModel.group);
            return View(privacyModel);
        }

        //
        // GET: /Privacy/Add
        public ActionResult Add()
        {
            PrivacyModel group_data = new PrivacyModel { groups = new List<string>() };
            QueryController queryCommand = new QueryController();
            group_data.groups = queryCommand.get_friendship((int)HttpContext.Session["userSessionID"]);
            return View(group_data);
        }

        //
        // POST: /Privacy/AddToGroup
        [HttpPost]
        public ActionResult AddToGroup(PrivacyModel privacyModel)
        {
            privacyModel.users = new List<KeyValuePair<int, KeyValuePair<string, string>>>();
            HttpContext.Session["userGroup"] = privacyModel.group;
            QueryController queryCommand = new QueryController();
            privacyModel.users = queryCommand.get_other_users((int)HttpContext.Session["userSessionID"], privacyModel.group);
            return View(privacyModel);
        }

        //
        // POST: /Privacy/AddUserToGroup
        [HttpPost]
        public ActionResult AddUserToGroup(PrivacyModel privacyModel)
        {
            QueryController queryCommand = new QueryController();
            queryCommand.add_to_group((int)HttpContext.Session["userSessionID"], privacyModel.user, (string)HttpContext.Session["userGroup"]);
            return RedirectToAction("Index");
        }
        //
        // GET: /Privacy/Remove/
        public ActionResult Remove()
        {
            PrivacyModel group_data = new PrivacyModel { groups = new List<string>() };
            QueryController queryCommand = new QueryController();
            group_data.groups = queryCommand.get_friendship((int)HttpContext.Session["userSessionID"]);
            return View(group_data);
        }

        //
        // POST: /Privacy/RemoveUser
        [HttpPost]
        public ActionResult RemoveUser(PrivacyModel privacyModel)
        {
            privacyModel.users = new List<KeyValuePair<int, KeyValuePair<string, string>>>();
            HttpContext.Session["userGroup"] = privacyModel.group;
            QueryController queryCommand = new QueryController();
            privacyModel.users = queryCommand.get_users((int)HttpContext.Session["userSessionID"], privacyModel.group);
            return View(privacyModel);
        }

        [HttpPost]
        public ActionResult RemoveUserFromGroup(PrivacyModel privacyModel)
        {
            QueryController queryCommand = new QueryController();
            queryCommand.remove_from_group((int)HttpContext.Session["userSessionID"], privacyModel.user, (string)HttpContext.Session["userGroup"]);
            return RedirectToAction("Index");
        }
    }
}
