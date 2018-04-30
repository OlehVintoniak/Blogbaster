using System.IO;
using System.Web;

namespace Blogbaster.Helpers
{
    public static class ImageHelper
    {
        public static byte[] GetDefaultPostImage()
        {
            return File.ReadAllBytes(HttpContext.Current.Server.MapPath(Constants.DefaultPostImageUrl));
        }
    }
}