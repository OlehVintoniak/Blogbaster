#region

using System;
using GundiakProject.DomainModels;
using GundiakProject.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using GundiakProject.Enums;
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
            db.Articles.Remove(article);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        public ActionResult ArticlesPage()
        {
            var articles = db.Articles
                .Where(a => a.Status == Status.Published)
                .ToList();
            return View(articles);
        }

        [HttpPost]
        public async Task<JsonResult> Publish(int articleId)
        {
            var article = await db.Articles.FirstOrDefaultAsync(a => a.Id == articleId);
            if (article == null)
            {
                return Json("article not found");
            }

            ChangeStatus(ref article);
            await db.SaveChangesAsync();

            return Json("success");
        }

        #region Helpers
        private void ChangeStatus(ref Article article)
        {
            if (article.Status == Status.Created)
            {
                article.Status = Status.Published;
            }
            else
            {
                article.Status = Status.Created;
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