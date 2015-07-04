using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SUrl.Models;
using System.Text.RegularExpressions;
using System.Configuration;

namespace SUrl.Controllers
{
    public class HomeController : Controller
    {
        private SUrlContext db = new SUrlContext();

        /// <summary>        
        /// Index Get Method. Its return Index view
        /// </summary>
        /// <param name="id"></param>
        public ActionResult Index(string id)
        {
            // check id is empty or not, if not empty then search long url from database.
            // then redirect to long url
            if (!string.IsNullOrEmpty(id))
            {                
                if (!db.tblSUrls.Any(x => x.ShortUrl == id))
                {
                    return RedirectToAction("Error");
                }
                else
                {
                    tblSUrl longUrl = new tblSUrl();
                    longUrl = db.tblSUrls.First(x => x.ShortUrl == id);
                    return Redirect(longUrl.LongUrl);
                }
            }

            ViewBag.message = "welcome back, Guest";

            return View();
        }
        
        
        /// <summary>
        /// Index Post Method. Its accepted a longurl 
        /// </summary>
        /// <param name="longurl"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Index")]
        public ActionResult ShortUrlReturn(string longurl)
        {
            
            // shortUrlid variable is used for saving randomly generated short url
            string shortUrlid = null;


            // check longurl parameter is empty or not. 
            // if longurl parameter is empty then return to Index view with ViewBag message
            if (string.IsNullOrEmpty(longurl))
            {
                ViewBag.error = "Please, enter your long url";
                return View();
            } 

            // else check long url is valid or not.
            // if long url is not valid then return to Index view with ViewBag message
            else
            {
                if(!IsUrlValid(longurl))
                {
                    ViewBag.error = "Please, enter a valid url";
                    return View(); 
                }

                if (db.tblSUrls.Any( x => x.LongUrl == longurl))
                {
                    tblSUrl searchLongUrl = new tblSUrl();
                    searchLongUrl = db.tblSUrls.First(x => x.LongUrl == longurl);

                    ViewBag.success = "Your short url " + ConfigurationManager.AppSettings["WebsiteUrl"] + "/" + searchLongUrl.ShortUrl;
                    return View();
                }
            }



            // saving randomly generated short url 
            shortUrlid = shortUrl();

            // check short url already contains in the database or not
            if (db.tblSUrls.Any(x => x.ShortUrl == shortUrlid))
            {
                // if short url already contains in the databse 
                // then its re-generated new short url
                shortUrlid = shortUrl();
            }

            // if firstly generated short url is not present in the database
            // then long url and short url save in the database
            // and initialize ViewBag message
            else
            {
                // create a object for SUrl table
                tblSUrl singleSUrl = new tblSUrl();

                // initialize long url
                singleSUrl.LongUrl = longurl;

                // initialize short url
                singleSUrl.ShortUrl = shortUrlid;

                // save long url and short url into SUrl table
                db.tblSUrls.AddObject(singleSUrl);
                db.SaveChanges();

                ViewBag.success = "Your short url " + ConfigurationManager.AppSettings["WebsiteUrl"] + "/" + shortUrlid;
            }
            

            // return Index View
            return View();
        }


        /// <summary>
        /// This private method used for checking long url is valid or not
        /// Its take a string type long url
        /// And return bool type
        /// </summary>
        /// <param name="url"></param>
        /// <returns> bResult </returns>
        private bool IsUrlValid(string url)
        {
            Regex myRegex = new Regex(@"(http|https|ftp|mailto)://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", RegexOptions.Compiled);
            bool bResult = myRegex.IsMatch(url);
            return bResult;
        }


        /// <summary>
        /// This private method generate randomly short url
        /// then return short url 
        /// </summary>
        /// <returns> result </returns>
        private string shortUrl()
        {
            var chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, 5)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            return result;
        }


        public ActionResult Error()
        {
            return View();
        }


        /// <summary>
        /// This method dispose db connection
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}