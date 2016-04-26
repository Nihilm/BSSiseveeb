using BSSiseveeb.Core.Domain;
using BSSiseveeb.Public.Web.Attributes;

namespace BSSiseveeb.Public.Web.Controllers.API
{
    [AuthorizeLevel(AccessRights.Level1)]
    public class WorkersController : BaseApiController
    {
    }
}