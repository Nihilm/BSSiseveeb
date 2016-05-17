using BSSiseveeb.Public.Web.Models;
using System.Web.Mvc;

namespace BSSiseveeb.Public.Web.Controllers
{
    public partial class MenuController : BaseController
    {
        [ChildActionOnly]
        public virtual ActionResult MainMenu()
        {
            return PartialView(new MenuViewModel()
            {
                UserRole = CurrentUserRole
            });
        }
    }
}