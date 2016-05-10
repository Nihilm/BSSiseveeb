using System;
using System.Linq;
using System.Web.Http;
using System.Web.WebPages;
using BSSiseveeb.Core.Domain;
using BSSiseveeb.Public.Web.Attributes;
using BSSiseveeb.Public.Web.Controllers.API.Helpers;
using BSSiseveeb.Public.Web.Models;

namespace BSSiseveeb.Public.Web.Controllers.API
{
    [Authorize]
    public class RequestsController : BaseApiController
    {
        [HttpPost]
        public IHttpActionResult SetRequest(RequestModel model)
        {
            if (!CurrentUser.Role.Rights.HasFlag(AccessRights.Standard))
            {
                return BadRequest("ERROR: Teil Puuduvad kasutaja õigused");
            }

            var title = model.Title;
            var info = model.Info;

            if (title.IsEmpty())
            {
                return BadRequest("ERROR: Taotlusel puudub pealkiri");
            }

            RequestRepository.AddIfNew(new Request()
            {
                Req = title,
                Description = info,
                EmployeeId = CurrentUser.Id,
                Status = RequestStatus.Pending,
                TimeStamp = DateTime.Now
            });

            RequestRepository.Commit();
            var emails = EmployeeRepository
                    .Where(x => x.RequestMessages == true && x.Role.Rights.HasFlag(AccessRights.Requests)).Select(x => x.Email).ToList();

            var subject = "New request";
            var body = $"<p> New request from {CurrentUser.Name}, he/she needs {model.Title}. Additional information: {model.Info} </p>";

            EmailHelper.SendEmail(emails, subject, body);

            return Ok();
        }
    }
}