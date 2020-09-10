using Ghimire_RSS_Feed.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
namespace Ghimire_RSS_Feed.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        public ActionResult Index(string sortOrder, string searchString)
        {
            //sorting based on the type of feeds
           
            ViewBag.SortingTech = sortOrder == "Technology" ? "" : "Technology";
            ViewBag.SortingDesign = sortOrder == "Design" ? "":"Design" ;
            ViewBag.SortingCulture = sortOrder == "Culture" ? "" : "Culture";
            ViewBag.SortingBusiness = sortOrder == "Business" ? "" : "Business";
            ViewBag.SortingPolitice = sortOrder == "Politics" ? "" : "Politics";
            ViewBag.SortingOpinion = sortOrder == "Opinion" ? "" : "Opinion";
            ViewBag.SortingScience = sortOrder == "Science" ? "" : "Science";
            ViewBag.SortingHealth = sortOrder == "Health" ? "" : "Health";
            ViewBag.SortingStyle = sortOrder == "Style" ? "" : "Style";
            ViewBag.SortingTravel = sortOrder == "Travel" ? "T" : "Travel";

            var rSSFeeds = db.RSSFeeds.OrderByDescending(a => a.PublishedDate);

            if (rSSFeeds.Count() == 0)
            {
                return RedirectToAction("Create","RSSFeeds");
            }
            else
            {
                ViewBag.FeedTop = rSSFeeds.First();

                if (!String.IsNullOrEmpty(sortOrder))
                {
                    var feed = Enum.Parse(typeof(FeedType), sortOrder);
                    rSSFeeds = (IOrderedQueryable<RSSFeeds>)rSSFeeds.Where(a => a.FeedType == (FeedType)feed);
                    if (rSSFeeds.Count() != 0)
                    {
                        ViewBag.FeedTop = rSSFeeds.First();
                    }

                }

                if (!String.IsNullOrEmpty(searchString))
                {
                    switch (searchString)
                    {
                        case "Date":
                            rSSFeeds = rSSFeeds.OrderBy(a => a.PublishedDate);
                            break;
                        case "Title":
                            rSSFeeds = rSSFeeds.OrderBy(a => a.Title);
                            break;
                        case "Description":
                            rSSFeeds = rSSFeeds.OrderBy(a => a.Description);
                            break;

                        default:
                            rSSFeeds = rSSFeeds.OrderBy(a => a.PublishedDate);
                            break;
                    }
                }
            }

            return View(rSSFeeds.Skip(1).ToList());
           
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}