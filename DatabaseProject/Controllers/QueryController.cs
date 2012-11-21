using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace DatabaseProject.Controllers
{
    public class QueryController : Controller
    {
        public ActionResult rate_post(int SESSION_PID, int RATED_CID, int RATING)
        {   //Rate a post with a given CID by the specified user, PID
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
            catch(Exception ex)
            {
                return Content("An error occured: " + ex.Message);
            }
            return View();
        }

        public ActionResult get_avg_rating(int POST_CID)
        {   //Get the average rating of the given post
            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySqlConnString"].ConnectionString))
                {
                    if (connection.State != System.Data.ConnectionState.Open)
                        connection.Open();
                    MySqlCommand command = new MySqlCommand("SELECT AVG(rtg) FROM rate WHERE cid = " + POST_CID + ";", connection);
                    MySqlDataReader dr = command.ExecuteReader();

                    if (dr.Read())
                    {
                       
                    }
                    dr.Close();
                    connection.Close();//Added close because it was always open
                }
            }
            catch (Exception ex)
            {
                return Content("An error occured: " + ex.Message);
            }
            return View();
        }

        public ActionResult get_post_by_topic(string TOPIC)
        {   //Get the post by topic
            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySqlConnString"].ConnectionString))
                {
                    if (connection.State != System.Data.ConnectionState.Open)
                        connection.Open();
                    MySqlCommand command = new MySqlCommand("SELECT * FROM content NATURAL JOIN cotopic WHERE topic = " + TOPIC + ";", connection);
                    MySqlDataReader dr = command.ExecuteReader();

                    while(dr.Read())
                    {
                        
                    }
                    dr.Close();
                    connection.Close();//Added close because it was always open
                }
            }
            catch (Exception ex)
            {
                return Content("An error occured: " + ex.Message);
            }
            return View();
        }

        public ActionResult get_topics()
        {   //Get the post by topic
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

                    }
                    dr.Close();
                    connection.Close();//Added close because it was always open
                }
            }
            catch (Exception ex)
            {
                return Content("An error occured: " + ex.Message);
            }
            return View();
        }

        public ActionResult get_public_posts()
        {   //Get the public posts (CAN BE SEENS WITHOUT LOGGING IN)
            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySqlConnString"].ConnectionString))
                {
                    if (connection.State != System.Data.ConnectionState.Open)
                        connection.Open();
                    MySqlCommand command = new MySqlCommand("SELECT * FROM content WHERE ctype = 0;", connection);
                    MySqlDataReader dr = command.ExecuteReader();

                    while (dr.Read())
                    {

                    }
                    dr.Close();
                    connection.Close();//Added close because it was always open
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
