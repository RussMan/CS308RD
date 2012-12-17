using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MySql.Data.MySqlClient;
using System.Configuration;
using DatabaseProject.Models;
using System.Security.Cryptography;

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
                                                            ", rtg = " + RATING + ";", connection);
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

        public PostListModel get_posts(int reader, bool all = true, int ctype = 0, int PAGE = 0, string TOPIC = "")
        {   //Get the post based on supplied data (like page, topic, and if everyone/only logged in people can see it)
            int total = -1;
            List<PostModel> fetched_Posts = new List<PostModel>();
            List<int> post_cid_list = new List<int>();

            //The following makes a custom query that changes depending on the data available
            string SQL_Query = "SELECT * FROM content";

            if (reader != 0)
            {
                if (TOPIC != "")
                {
                    SQL_Query += (" NATURAL JOIN contopic WHERE (cid IN (SELECT cid FROM friend NATURAL JOIN visible WHERE reader = " + reader + ") OR cid NOT IN (SELECT cid FROM visible) OR poster = " + reader + ") AND topic = '" + TOPIC + "';");
                }
                else
                {
                    SQL_Query += (" WHERE (cid IN (SELECT cid FROM friend NATURAL JOIN visible WHERE reader = " + reader + ") OR cid NOT IN (SELECT cid FROM visible) OR poster = " + reader + ")");
                }
            }
            else
            {
                SQL_Query += (" WHERE ctype = 0");
            }

            if (all)
                SQL_Query += ";";
            else
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
                        post_cid_list.Add(dr.GetInt32("cid"));
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
                            if (!dr.IsDBNull(0))
                                x.avg_rtg = dr.GetInt32(0);
                            else
                                x.avg_rtg = int.MinValue;
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

                    if (TOPIC == "")
                    {
                        command = new MySqlCommand("SELECT COUNT(cid) FROM content WHERE (cid IN (SELECT cid FROM friend NATURAL JOIN visible WHERE reader = " + reader + ") OR cid NOT IN (SELECT cid FROM visible) OR poster = " + reader + ");", connection);
                        total = (Convert.ToInt32(command.ExecuteScalar()));
                        connection.Close(); //Added close because it was always open
                    }
                    else
                    {
                        command = new MySqlCommand("SELECT COUNT(topic) FROM contopic WHERE topic = '" + TOPIC + "';", connection);
                        total = (Convert.ToInt32(command.ExecuteScalar()));
                        connection.Close(); //Added close because it was always open
                    }
                }

            }
            catch (Exception ex)
            {
                //return Content("An error occured: " + ex.Message);
            }

            PostListModel post_list = new PostListModel
            {
                posts = fetched_Posts,
                total_posts = total,
                cid_list = post_cid_list
            };

            return post_list;
        }

        public List<string> get_friendship(int pid)
        {
            List<string> friends = new List<string>();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySqlConnString"].ConnectionString))
                {
                    if (connection.State != System.Data.ConnectionState.Open)
                        connection.Open();
                    MySqlCommand command = new MySqlCommand("SELECT ftype FROM pft WHERE poster = " + pid + ";", connection);
                    MySqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        friends.Add(dr.GetString("ftype"));
                    }
                    dr.Close();
                    connection.Close();//Added close because it was always open
                }
            }
            catch (Exception ex)
            {
                
            }

            return friends;
        }
        /*****************************************************************************************
         *                                POSTING RELATED ACTIONS                                *
         *****************************************************************************************/
        [HttpPost]
        public void new_post(NewPostModel new_post)
        {   //Create a new post using data from the page and add the new post 
            int cid = 0;

            DateTime time = DateTime.Now;
            string time_s = string.Format("{0:D4}-{1:D2}-{2:D2} {3:D2}:{4:D2}:{5:D2}", time.Year, time.Month, time.Day, time.Hour, time.Minute, time.Second);

            string SQL_Query = "INSERT INTO content(ctype, ctext, poster, ptime) VALUES(";
            if (new_post.ctype)
                SQL_Query += "1, '";
            else
                SQL_Query += "0, '";
            SQL_Query += new_post.ctext + "', " + new_post.pid + ", '" + time_s + "');";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySqlConnString"].ConnectionString))
                {
                    if (connection.State != System.Data.ConnectionState.Open)
                        connection.Open();
                    MySqlCommand command = new MySqlCommand(SQL_Query, connection);
                    command.ExecuteNonQuery();

                    SQL_Query = "SELECT MAX(cid) FROM content;";
                    command = new MySqlCommand(SQL_Query, connection);
                    MySqlDataReader dr = command.ExecuteReader();

                    if (dr.Read())
                        cid = dr.GetInt32("MAX(cid)");
                    dr.Close();

                    if (new_post.ctype)
                    {
                        SQL_Query = "INSERT INTO visible(cid, poster, ftype) VALUES(" + cid + "," + new_post.pid + ",'" + new_post.ftype + "');";
                        command = new MySqlCommand(SQL_Query, connection);
                        command.ExecuteNonQuery();
                    }

                    if (new_post.topic != null)   //We have a topic to add
                    {//First get the latest post we added to get the CID
                        
                        //Make Query to add new topic
                        SQL_Query = "INSERT INTO contopic(cid, topic) VALUES(" + cid + ",'" + new_post.topic + "');";
                        command = new MySqlCommand(SQL_Query, connection);
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
