using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Ghimire_RSS_Feed.Models;
using Microsoft.AspNet.Identity;
using System.IO;

namespace Ghimire_RSS_Feed.Controllers
{
    
    public class RSSFeedsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: RSSFeeds
        [Authorize]
        public async Task<ActionResult> Index( string searchString)
        {
            var userId = User.Identity.GetUserId();
            var rSSFeeds = db.RSSFeeds.Where(a => a.Id == userId).Include(r => r.ApplicationUser);

            if (!String.IsNullOrEmpty(searchString))
            {
                switch (searchString)
                {
                    case "Date":
                        rSSFeeds = rSSFeeds.OrderBy(a=>a.PublishedDate);
                        break;
                    case "Title":
                        rSSFeeds = rSSFeeds.OrderBy(a => a.Title);
                        break;
                    case "Description":
                        rSSFeeds = rSSFeeds.OrderBy(a => a.Description);
                        break;

                    default:
                        break;
                }


            }

            return View(await rSSFeeds.ToListAsync());
        }

        // GET: RSSFeeds/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RSSFeeds rSSFeeds = await db.RSSFeeds.FindAsync(id);
            if (rSSFeeds == null)
            {
                return HttpNotFound();
            }
            return View(rSSFeeds);
        }

        [Authorize]
        // GET: RSSFeeds/Create
        public ActionResult Create()
        {
           // ViewBag.Id = new SelectList(db.ApplicationUsers, "Id", "FirstName");
            return View();
        }

        // POST: RSSFeeds/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "RSSFeedsId,Title,Description,PublishedDate,Image,FeedType,Feedurl,Id")] RSSFeeds rSSFeeds , HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {

                //uploading image file
                if(file != null)
                {
                    byte[] imageBytes = null;
                    BinaryReader reader = new BinaryReader(file.InputStream);
                    imageBytes = reader.ReadBytes((int)file.ContentLength);

                    rSSFeeds.Image = imageBytes;
                }
               

                rSSFeeds.RSSFeedsId = Guid.NewGuid();
                //assign ID as the user ID
                rSSFeeds.Id = User.Identity.GetUserId();
                rSSFeeds.PublishedDate = DateTime.UtcNow;

                db.RSSFeeds.Add(rSSFeeds);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

           // ViewBag.Id = new SelectList(db.ApplicationUsers, "Id", "FirstName", rSSFeeds.Id);
            return View(rSSFeeds);
        }


        public ActionResult RetrieveImage(Guid id)
        {
            byte[] cover = GetImageFromDataBase(id);
            if (cover != null)
            {
                return File(cover, "image/jpg");
            }
            else
            {
                return null;
            }
        }
        public byte[] GetImageFromDataBase(Guid Id)
        {

            var q = db.RSSFeeds.Where(a => a.RSSFeedsId == Id).Select(b => b.Image);

            byte[] cover = null;
            ;
            if (q.Count() != 0)
            {
                cover = q.First();
            }
           
            return cover;
        }


        // GET: RSSFeeds/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RSSFeeds rSSFeeds = await db.RSSFeeds.FindAsync(id);
            if (rSSFeeds == null)
            {
                return HttpNotFound();
            }
          //  ViewBag.Id = new SelectList(db.ApplicationUsers, "Id", "FirstName", rSSFeeds.Id);
            return View(rSSFeeds);
        }

        // POST: RSSFeeds/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "RSSFeedsId,Title,Description,PublishedDate,Image,FeedType,Feedurl,Id")] RSSFeeds rSSFeeds)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rSSFeeds).State = EntityState.Modified;
                rSSFeeds.Id = User.Identity.GetUserId();
                rSSFeeds.PublishedDate = DateTime.UtcNow;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
          //  ViewBag.Id = new SelectList(db.ApplicationUsers, "Id", "FirstName", rSSFeeds.Id);
            return View(rSSFeeds);
        }

        // GET: RSSFeeds/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RSSFeeds rSSFeeds = await db.RSSFeeds.FindAsync(id);
            if (rSSFeeds == null)
            {
                return HttpNotFound();
            }
            return View(rSSFeeds);
        }

        // POST: RSSFeeds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            RSSFeeds rSSFeeds = await db.RSSFeeds.FindAsync(id);
            db.RSSFeeds.Remove(rSSFeeds);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
