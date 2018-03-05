using System.IO;
using System.Web;

namespace GundiakProject.Helpers
{
    public static class ImageHelper
    {
        public static byte[] GetDefaultArticleImage()
        {
            return File.ReadAllBytes(HttpContext.Current.Server.MapPath(Constants.DefaultArticleImageUrl));
        }
    }
}