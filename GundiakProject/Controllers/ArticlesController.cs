#region

using System;
using System.Collections.Generic;
using GundiakProject.DomainModels;
using GundiakProject.Models;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Mvc;
using Antlr.Runtime;
using GundiakProject.Enums;
using GundiakProject.Helpers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

#endregion

namespace GundiakProject.Controllers
{
    public class ArticlesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private readonly UserManager<ApplicationUser> _userManager;

        public ArticlesController()
        {
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }

        #region Get
        // GET: Articles
        public async Task<ActionResult> Index()
        {
            var articles = db.Articles.Include(a => a.ApplicationUser);
            return View(await articles.ToListAsync());
        }

        // GET: Articles/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = await db.Articles.FindAsync(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }
        #endregion

        #region Create
        // GET: Articles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Articles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Article article)
        {
            if (ModelState.IsValid)
            {
                article.DateCreated = DateTime.Now;
                article.ApplicationUserId = User.Identity.GetUserId();

                #region SetImage
                if (Request.Files[0] != null)
                {
                    byte[] imageData = null;
                    using (var binaryReader = new BinaryReader(Request.Files[0].InputStream))
                    {
                        imageData = binaryReader.ReadBytes(Request.Files[0].ContentLength);
                    }
                    article.Image = imageData;
                }
                if (article.Image.Length == 0)
                {
                    article.Image = ImageHelper.GetDefaultArticleImage();
                }
                #endregion

                db.Articles.Add(article);
                await db.SaveChangesAsync();
                return RedirectToRoute(new {controller="Home", action="Index" });
            }
            return View(article);
        }
        #endregion

        #region Update (not implemented)
        //// GET: Articles/Edit/5
        //public async Task<ActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Article article = await db.Articles.FindAsync(id);
        //    if (article == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(article);
        //}

        //// POST: Articles/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Edit([Bind(Include = "Id,Title,Text,Status,ApplicationUserId")] Article article)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(article).State = EntityState.Modified;
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }
        //    return View(article);
        //}
        #endregion

        #region Delete
        // GET: Articles/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = await db.Articles.FindAsync(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // POST: Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Article article = await db.Articles.FindAsync(id);

            #region EmailNotification
            if(article != null)
                await EmailHelper.SendEmail(article.ApplicationUser.Email, "Sorry!",
                    $"Your article {article.Title} was deleted!");
            #endregion

            db.Articles.Remove(article);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        public ActionResult ArticlesPage()
        {
            return View();
        }

        public ActionResult Articles(int pageIndex, int pageSize)
        {
            var articles = db.Articles
                .Where(a => a.Status == Status.Published)
                .OrderByDescending(a => a.DatePublished).AsQueryable()
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToList();

            return PartialView("_ArticlesList", articles);
        }
        public JsonResult ActiclesCount()
        {
            var count = db.Articles.Count(a => a.Status == Status.Published);
            return Json(count, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> Publish(int articleId)
        {
            var article = await db.Articles.FirstOrDefaultAsync(a => a.Id == articleId);
            if (article == null)
            {
                return Json("article not found");
            }

            #region EmailNotification

            if (!article.WasPublished)
            {
                await EmailHelper.SendEmail(article.ApplicationUser.Email, "Congratulation!",
                    $"Your article {article.Title} was succesfuly published!");
            }

            #endregion

            var result = ChangeStatus(ref article);
            await db.SaveChangesAsync();

            return Json(result);
        }

        #region Helpers
        private string ChangeStatus(ref Article article)
        {
            if (article.Status == Status.Created)
            {
                if (!article.WasPublished)
                    article.WasPublished = true;

                article.Status = Status.Published;
                article.DatePublished = DateTime.Now;
                return "Published";
            }
            else
            {
                article.Status = Status.Created;
                return "Created";
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}