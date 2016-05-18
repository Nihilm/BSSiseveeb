using System.Web.Mvc;

namespace BSSiseveeb.Public.Web.Controllers
{
    public partial class ErrorController : BaseController
    {
        public virtual ActionResult UnAuthorized()
        {
            return View();
        }

        public virtual ActionResult ServerError()
        {
            return View();
        }
    }
}