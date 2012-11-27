using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DatabaseProject.Models;

namespace DatabaseProject.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Hello this is a modification!";

            return View();
        }

        public ActionResult PublicPosts(int page = 0) //Temporary fix just to show the first 5 posts; TODO: allow for all type 0 posts to be shown
        {
            //ViewBag.Message = "Your app description page. Here is the featured section of the About page!";
            QueryController SQL = new QueryController();
            PostListModel post_list = SQL.get_posts(page);
            return View(post_list);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
