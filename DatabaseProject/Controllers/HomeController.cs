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
        //
        // GET: /Home/
        public ActionResult Index()
        {
            ViewBag.Message = "Hello this is a modification!";
            return View();
        }

        //
        // GET: /Home/PublicPosts
        public ActionResult PublicPosts() // Temporary fix just to show the first 5 posts; TODO: allow for all type 0 posts to be shown
        {
            //ViewBag.Message = "Your app description page. Here is the featured section of the About page!";
            QueryController SQL = new QueryController();
<<<<<<< HEAD
            PostListModel post_list = SQL.get_posts(page, "", 0, true); //All posts
=======
            PostListModel post_list = SQL.get_posts();
>>>>>>> 10d3c5252d71672bd28c5b4d7eb321f2c74504ad
            return View(post_list);
        }

        //
        // GET: /Home/Contact
        public ActionResult Contact()
        {
            ViewBag.Message = "How to contact the site's developers.";
            return View();
        }
    }
}
