using System.Web.Mvc;
using BSSiseveeb.Public.Web.Attributes;


namespace BSSiseveeb.Public.Web.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        [AuthorizeLevel1]
        public ActionResult Workers()
        {
            return View();
        }

        [AuthorizeLevel1]
        public ActionResult Requests()
        {
            return View();
        }

        [AuthorizeLevel1]
        public ActionResult Calendar()
        {
            return View();
        }

        [AuthorizeLevel5]
        public ActionResult Admin()
        {
            return View();
        }
    }
}