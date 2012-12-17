using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DatabaseProject.Models;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace DatabaseProject.Controllers
{
    public class ContentItemController : Controller
    {

        //
        // GET: /ContentItem/
        [Authorize]
        public ActionResult Index(int page = 0)
        {
            Session["currentPage"] = page;
            PostListModel post_list = new PostListModel();
            if (Session["searchTopic"] != null)
            {
                //IncludeSearchedPosts(ref post_list, page);
                QueryController queryCommand = new QueryController();
                post_list = queryCommand.get_posts((int)HttpContext.Session["userSessionID"], false, 1, page, (string)Session["searchTopic"]);
                Session["totalPages"] = (int)Math.Ceiling((double)post_list.total_posts/(double)5);
                Session["isSearchedPosts"] = true; //used to preserve the number of pages that correlate to searched or regular posts
            }
            else
            {
                QueryController queryCommand = new QueryController();
                post_list = queryCommand.get_posts((int)HttpContext.Session["userSessionID"], false, 1, page);
                if (Session["isSearchedPosts"] == null) Session["isSearchedPosts"] = false; //For initialization purposes
                if (Session["totalPages"] == null || !(bool)Session["isSearchedPosts"])
                {
                    Session["totalPages"] = (int)Math.Ceiling((double)post_list.total_posts / (double)5); // Get the number of pages needed for pagination in the Content/Read page
                    Session["isSearchPosts"] = false;
                    Session["searchTopic"] = ""; // Resetting search topic
                }
            }
            return View(post_list);
        }
        
        //
        // POST: /ContentItem/
        [HttpPost]
        public ActionResult Index(int id, PostListModel postList) // This is for user input on the rating, NOT on page changing
        {                                                         // If page parameter, URL may not know which to go to and throw an error
            QueryController queryCommand = new QueryController();
            queryCommand.rate_post((int)HttpContext.Session["userSessionID"], id, postList.rating.rate); // id is in reference to the CID (post ID)
            //PostListModel post_list = queryCommand.get_posts((int)HttpContext.Session["userSessionID"]);
            //return View(post_list);
            return RedirectToAction("Index");
        }

        //
        // GET: /ContentItem/Write
        public ActionResult Write()
        {
            NewPost_UI_Data post_data = new NewPost_UI_Data { new_post = new NewPostModel(), friendships = new List<string>() };
            QueryController queryCommand = new QueryController();
            post_data.friendships = queryCommand.get_friendship((int)HttpContext.Session["userSessionID"]);
            return View(post_data);
        }
        
        //
        // POST: /ContentItem/Write
        [HttpPost]
        public ActionResult Write(NewPost_UI_Data post_data)
        {
            QueryController queryCommand = new QueryController();
            post_data.new_post.pid = (int)HttpContext.Session["userSessionID"]; 
            queryCommand.new_post(post_data.new_post);
            return RedirectToAction("Index");
        }

        //
        // GET: /ContentItem/Search
        public ActionResult Search()
        {
            return View();
        }

        //
        // POST: /ContentItem/Search/topic
        [HttpPost]
        public ActionResult Search(SearchModel topic)
        {
            try
            {
                if (topic.searchTopic == null) // User doesn't provide any text input
                {
                    return View();
                }

                if (topic.searchTopic != "all") // "all" will reset to default post types
                {
                    //QueryController queryCommand = new QueryController();
                    //PostListModel post_list = queryCommand.get_posts((int)HttpContext.Session["userSessionID"], false, 1, 0, topic.searchTopic);
                    Session["searchTopic"] = topic.searchTopic;
                    //Session["searchedPosts"] = post_list;
                    return RedirectToAction("Index");
                }
                else
                {

                    //Session["searchedPosts"] = null; // if "all": clear for future searches
                    Session["searchTopic"] = null;
                    Session["totalPages"] = null;
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                return View();
            }
        }
    }
}