using System;
using System.IO;
using System.Web;

namespace GundiakProject.Helpers
{
    public static class ImageHelper
    {
        public static string UploadImg(HttpPostedFileBase file)
        {
            var imgUrl = Constants.DefaultImageUrl;
            if (file != null && file.ContentLength > 0)
            {
                var guid = Guid.NewGuid().ToString();
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/Images/"), "article_img_" + guid + "_" + fileName);
                file.SaveAs(path);
                imgUrl = "/Content/Images/" + "article_img_" + guid + "_" + fileName;
            }
            return imgUrl;
        }

        public static void DeleteImg(string imgUrl)
        {
            var imgPath = HttpContext.Current.Server.MapPath(imgUrl);
            if (File.Exists(imgPath) && imgPath != Constants.DefaultImageUrl && imgPath != null)
            {
                File.Delete(imgPath);
            }
        }
    }
}