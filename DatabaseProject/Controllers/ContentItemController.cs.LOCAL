using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace DatabaseProject.Controllers
{

    public class ContentItemController : Controller
    {
        //
        // GET: /ContentItem/
        [Authorize]
        public ActionResult Index()
        {
            ViewBag.Message = "Trending content among your groups";
            return View();
            //return Content("Hello, World!");
        }

        //
        // GET: /ContentItem/GetPosts
        public ActionResult get_by_topic()
        {
            string dummy = "";
            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySqlConnString"].ConnectionString))
                {
                    if (connection.State != System.Data.ConnectionState.Open)
                        connection.Open();
                    MySqlCommand command = new MySqlCommand("SELECT * FROM content NATURAL JOIN contopic WHERE topic = 'buttsecks';", connection);
                    MySqlDataReader dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        dummy += ("| " + dr.GetString(0) + " |");
                    }
                    dr.Close();
                    connection.Close();//Added close because it was always open
                    return Content(dummy);
                }
            }
            catch (Exception ex)
            {
                return Content("An error occured: " + ex.Message);
            }
            return View();
        }


        //
        // GET: /ContentItem/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /ContentItem/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /ContentItem/Edit/5

        public ActionResult Edit(int id)
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
        // GET: /ContentItem/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /ContentItem/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
