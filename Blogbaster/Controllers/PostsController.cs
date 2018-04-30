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
    public class PostsController : Controller
    {
        private readonly IPostService _postService;
        private readonly ICategoryService _categoryService;

        public PostsController(IPostService postService, ICategoryService categoryService)
        {
            _postService = postService;
            _categoryService = categoryService;
        }

        #region Get
        // GET: Posts
        [OnlyForAdmin]
        public ActionResult Index()
        {
            var posts = _postService.GetAll().ToList();
            return View(posts);
        }

        // GET: Posts/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Post post = _postService.FindById(id);

            if (post == null)
                return HttpNotFound();
            return View(post);
        }
        #endregion

        #region Create
        // GET: Posts/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(_categoryService.GetAll(), "Id", "Name");
            return View("CreatePost");
        }

        // POST: Posts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Post post)
        {
            if (ModelState.IsValid)
            {
                post.DateCreated = DateTime.Now;
                post.ApplicationUserId = User.Identity.GetUserId();

                #region SetImage
                if (Request.Files[0] != null)
                {
                    byte[] imageData = null;
                    using (var binaryReader = new BinaryReader(Request.Files[0].InputStream))
                    {
                        imageData = binaryReader.ReadBytes(Request.Files[0].ContentLength);
                    }
                    post.Image = imageData;
                }
                if (post.Image.Length == 0)
                {
                    post.Image = ImageHelper.GetDefaultPostImage();
                }
                #endregion

                await _postService.Add(post);
                return RedirectToRoute(new {controller="Posts", action="PostsPage" });
            }

            ViewBag.CategoryId = new SelectList(_categoryService.GetAll(), "Id", "Name", post.CategoryId);
            return View("CreatePost",post);
        }
        #endregion

        #region Update (not implemented)
        //// GET: Posts/Edit/5
        //public async Task<ActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Post post = await db.Posts.FindAsync(id);
        //    if (post == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(post);
        //}

        //// POST: Posts/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Edit([Bind(Include = "Id,Title,Text,Status,ApplicationUserId")] Post post)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(post).State = EntityState.Modified;
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }
        //    return View(post);
        //}
        #endregion

        #region Delete
        // GET: Posts/Delete/5
        [OnlyForAdmin]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var post = _postService.FindById(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Posts/Delete/5
        [OnlyForAdmin]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await _postService.DeleteById(id);
            return RedirectToAction("Index");
        }
        #endregion

        public ActionResult PostsPage()
        {
            return View();
        }

        public ActionResult Posts(int pageIndex, int pageSize)
        {
            var posts = _postService.GetPaginated(pageIndex, pageSize).ToList();
            return PartialView("_PostsList", posts);
        }

        public JsonResult PostsCount()
        {
            return Json(_postService.PublishedPostsCount(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [OnlyForAdmin]
        public async Task<JsonResult> Publish(int postId)
        {
            var post = _postService.FindById(postId);
            if (post == null)
            {
                return Json("post not found");
            }

            await _postService.ChangeStatus(post);
            return Json("Done");
        }
    }
}