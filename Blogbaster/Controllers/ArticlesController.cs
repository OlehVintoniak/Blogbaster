#region

using Blogbaster.Core.Data.Entities;
using Blogbaster.Core.Services.Interfaces;
using Blogbaster.Filters;
using Blogbaster.Helpers;
using Microsoft.AspNet.Identity;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

#endregion

namespace Blogbaster.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly IPostService _postService;

        public ArticlesController(IPostService postService)
        {
            _postService = postService;
        }

        #region Get
        // GET: Articles
        [OnlyForAdmin]
        public ActionResult Index()
        {
            var articles = _postService.GetAll().ToList();
            return View(articles);
        }

        // GET: Articles/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Article article = _postService.FindById(id);

            if (article == null)
                return HttpNotFound();
            return View(article);
        }
        #endregion

        #region Create
        // GET: Articles/Create
        public ActionResult Create()
        {
            return View("CreateArticle");
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

                await _postService.Add(article);
                return RedirectToRoute(new {controller="Articles", action="ArticlesPage" });
            }
            return View("CreateArticle",article);
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
        [OnlyForAdmin]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var article = _postService.FindById(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // POST: Articles/Delete/5
        [OnlyForAdmin]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await _postService.DeleteById(id);
            return RedirectToAction("Index");
        }
        #endregion

        public ActionResult ArticlesPage()
        {
            return View();
        }

        public ActionResult Articles(int pageIndex, int pageSize)
        {
            var articles = _postService.GetPaginated(pageIndex, pageSize).ToList();
            return PartialView("_ArticlesList", articles);
        }

        public JsonResult ActiclesCount()
        {
            return Json(_postService.PublishedPostsCount(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [OnlyForAdmin]
        public async Task<JsonResult> Publish(int articleId)
        {
            var article = _postService.FindById(articleId);
            if (article == null)
            {
                return Json("article not found");
            }

            await _postService.ChangeStatus(article);
            return Json("Done");
        }
    }
}