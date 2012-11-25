using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MySql.Data.MySqlClient;
using System.Configuration;
using DatabaseProject.Models;

namespace DatabaseProject.Controllers
{
    public class QueryController : Controller
    {
        /*****************************************************************************************
         *                              RATING RELATED ACTIONS                                   *
         *****************************************************************************************/
        public ActionResult rate_post(int SESSION_PID, int RATED_CID, int RATING)
        {   //Rate a post using the SQL REPLACE command with a given CID by the specified user, PID 
            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySqlConnString"].ConnectionString))
                {
                    if (connection.State != System.Data.ConnectionState.Open)
                        connection.Open();
                    MySqlCommand command = new MySqlCommand("REPLACE INTO rate SET pid = " + SESSION_PID + ", cid = " + RATED_CID +
                                                            ", rtg = " + RATING + "');", connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                return Content("An error occured: " + ex.Message);
            }
            return View();
        }

        /*****************************************************************************************
         *                          POST-FETCHING RELATED ACTIONS                                *
         *****************************************************************************************/
        public ActionResult get_topics()
        {   //Get the topics available for the user to choose
            List<string> topic_lst = new List<string>();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySqlConnString"].ConnectionString))
                {
                    if (connection.State != System.Data.ConnectionState.Open)
                        connection.Open();
                    MySqlCommand command = new MySqlCommand("SELECT * FROM contopic;", connection);
                    MySqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        if (!(topic_lst.Contains(dr.GetString(1))))  //Make sure the topics do not repeat
                            topic_lst.Add(dr.GetString(1));
                    }
                    dr.Close();
                    connection.Close();//Added close because it was always open
                }
            }
            catch (Exception ex)
            {
                return Content("An error occured: " + ex.Message);
            }

            var viewModel = new TopicModel { topics = topic_lst };

            return View(viewModel); //Return the topics via a Topic Model
        }

        public PostListModel get_posts(int PAGE, string TOPIC = "", int ctype = 0)
        {   //Get the post based on supplied data (like page, topic, and if everyone/only logged in people can see it)
            int total = -1;
            List<PostModel> fetched_Posts = new List<PostModel>();

            //The following makes a custom query that changes depending on the data available
            string SQL_Query = "SELECT * FROM content";
            if (TOPIC != "")
                SQL_Query += (" NATURAL JOIN contopic WHERE topic = '" + TOPIC + "' AND ctype = " + ctype);
            else
                SQL_Query += (" WHERE ctype = " + ctype);
            SQL_Query += (" LIMIT 5 OFFSET " + (PAGE * 5) + ";");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySqlConnString"].ConnectionString))
                {
                    if (connection.State != System.Data.ConnectionState.Open)
                        connection.Open();

                    MySqlCommand command = new MySqlCommand(SQL_Query, connection);
                    MySqlDataReader dr = command.ExecuteReader();

                    while (dr.Read())
                    {// Get the pid, ctype, ctext, pid and ptime for the fetched posts
                        fetched_Posts.Add(new PostModel
                        {
                            cid = dr.GetInt32("cid"),
                            ctype = dr.GetInt32("ctype"),
                            ctext = dr.GetString("ctext"),
                            pid = dr.GetInt32("poster"),
                            ptime = dr.GetString("ptime"),
                            post_Topics = new List<string>()
                        });
                    }
                    dr.Close();

                    foreach (PostModel x in fetched_Posts)
                    {//For the fetched posts, add the authors name into the Model
                        command = new MySqlCommand("SELECT fname FROM person WHERE pid = " + x.pid + ";", connection);
                        dr = command.ExecuteReader();
                        if (dr.Read())
                            x.fname = dr.GetString("fname");
                        dr.Close();
                    }

                    foreach (PostModel x in fetched_Posts)
                    {//For the fetched posts, grab the average rating the post has
                        command = new MySqlCommand("SELECT AVG(rtg) FROM rate WHERE cid = " + x.cid + ";", connection);
                        dr = command.ExecuteReader();
                        if (dr.Read())
                            x.avg_rtg = dr.GetInt32(0);
                        dr.Close();
                    }

                    if (TOPIC == "")
                        foreach (PostModel x in fetched_Posts)
                        {//If were not looking for posts with specific topics, fetch the topics of each post (if any)
                            command = new MySqlCommand("SELECT topic FROM contopic WHERE cid = " + x.cid + ";", connection);
                            dr = command.ExecuteReader();
                            if (dr.Read())
                                x.post_Topics.Add(dr.GetString("topic"));
                            dr.Close();
                        }

                    command = new MySqlCommand("SELECT COUNT(cid) FROM content;", connection);
                    total = (Convert.ToInt32(command.ExecuteScalar())) / 5;
                    connection.Close(); //Added close because it was always open
                }

            }
            catch (Exception ex)
            {
                //return Content("An error occured: " + ex.Message);
            }

            PostListModel post_list = new PostListModel
            {
                posts = fetched_Posts,
                total_posts = total
            };

            return post_list;
        }
        /*****************************************************************************************
         *                                POSTING RELATED ACTIONS                                *
         *****************************************************************************************/
        [HttpPost]
        public ActionResult new_post(NewPostModel new_post)
        {   //Create a new post using data from the page and add the new post 
            int cid = 0;
            string SQL_Query = "INSERT INTO content(ctype, ctext, poster, ptime) VALUES("
                               + new_post.ctype + "," + new_post.ctext + "," + new_post.pid + "," + new_post.ptime + ");";
            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySqlConnString"].ConnectionString))
                {
                    if (connection.State != System.Data.ConnectionState.Open)
                        connection.Open();
                    MySqlCommand command = new MySqlCommand(SQL_Query, connection);
                    command.ExecuteNonQuery();

                    if (new_post.topic.Count > 0)   //We have a topic to add
                    {//First get the latest post we added to get the CID
                        SQL_Query = "SELECT MAX(cid) FROM contopic;";
                        command = new MySqlCommand(SQL_Query, connection);
                        MySqlDataReader dr = command.ExecuteReader();
                        
                        if (dr.Read())
                            cid = dr.GetInt32("cid");
                        dr.Close();
                        //Make Query to add new topic
                        SQL_Query = "INSERT INTO contopic(cid, topic) VALUES(" + cid + "," + new_post.topic[0] + ");";
                        command = new MySqlCommand(SQL_Query, connection);
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                return Content("An error occured: " + ex.Message);
            }
            
            return View();
        }
    }
}
