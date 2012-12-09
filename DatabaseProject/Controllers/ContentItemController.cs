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
            if (Session["searchedPosts"] != null)
            {
                //IncludeSearchedPosts(ref post_list, page);
                QueryController queryCommand = new QueryController();
                post_list = queryCommand.get_posts(false, 1, page, (string)Session["searchTopic"]);
                Session["totalPages"] = (int)Math.Ceiling((double)post_list.total_posts/(double)5);
                Session["isSearchedPosts"] = true; //used to preserve the number of pages that correlate to searched or regular posts
            }
            else
            {
                QueryController queryCommand = new QueryController();
                post_list = queryCommand.get_posts(false, 1, page);
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
            PostListModel post_list = queryCommand.get_posts();
            return View(post_list);
        }

        //
        // GET: /ContentItem/Write
        public ActionResult Write()
        {
            return View();
        }
        
        //
        // POST: /ContentItem/Write
        [HttpPost]
        public ActionResult Write(NewPostModel new_post)
        {
            QueryController queryCommand = new QueryController();
            new_post.pid = (int)HttpContext.Session["userSessionID"]; 
            queryCommand.new_post(new_post);
            return RedirectToAction("Index");
        }

        //
        // GET: /ContentItem/Edit/
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
                    QueryController queryCommand = new QueryController();
                    PostListModel post_list = queryCommand.get_posts(false, 1, 0, topic.searchTopic);
                    Session["searchTopic"] = topic.searchTopic;
                    Session["searchedPosts"] = post_list;
                    return RedirectToAction("Index");
                }

                Session["searchedPosts"] = null; // if "all": clear for future searches
                Session["searchTopic"] = "";
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //public void IncludeSearchedPosts(ref PostListModel post_list, int page = 0)
        //{
        //    PostListModel searchedPosts = (PostListModel)Session["searchedPosts"]; // Type cast to get posts from Global object "Session"
        //    post_list.total_posts += searchedPosts.total_posts;
        //    post_list.posts = searchedPosts.posts;
        //    post_list.cid_list = searchedPosts.cid_list;
        //    Session["searchedPosts"] = null; // Allow for new search posts to be stored
        //    QueryController queryCommand = new QueryController();
        //    PostListModel regularPosts = queryCommand.get_posts(page);
        //    PostEqualityComparer postComparator = new PostEqualityComparer();
        //    CIDEqualityComparer CIDComparator = new CIDEqualityComparer();
        //    foreach (var x in regularPosts.posts) // N^2 running time, but acceptable due to max of 5 posts within the list anyway...
        //    {
        //        if(!post_list.posts.Contains(x,postComparator)) // Prevent duplicates
        //        {
        //            post_list.posts.Add(x);
        //            post_list.total_posts++;
        //        }
        //    }
        //    foreach (var x in regularPosts.cid_list) // N^2 running time, but acceptable due to max of 5 posts within the list anyway...
        //    {
        //        if(!post_list.cid_list.Contains(x, CIDComparator)) post_list.cid_list.Add(x); // Prevent duplicates
        //    }
        //}
    }
}