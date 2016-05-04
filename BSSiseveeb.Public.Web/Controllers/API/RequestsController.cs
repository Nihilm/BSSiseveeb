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
    public class RequestsController : BaseApiController
    {
        [HttpPost]
        [AuthorizeApi(AccessRights.Standard)]
        public IHttpActionResult SetRequest(RequestModel model)
        {
            var title = model.Title;
            var info = model.Info;

            if (title.IsEmpty())
            {
                return BadRequest("ERROR: Taotlusel puudub pealkiri");
            }

            var currentUser = CurrentUser.EmployeeId;

            RequestRepository.AddIfNew(new Request()
            {
                Req = title,
                Description = info,
                EmployeeId = currentUser,
                Status = RequestStatus.Pending,
                TimeStamp = DateTime.Now
            });

            RequestRepository.Commit();
            var roles = RoleManager.Roles.Where(x => x.Rights.HasFlag(AccessRights.Requests)).Select(x => x.Id);
            var emails = EmployeeRepository.Where(x => x.Account.Messages == "Yes" && roles.Contains(x.Account.RoleId)).Select(x => x.Email).ToList();
            var subject = "New request";
            var body = $"<p> New request from {CurrentUser.Employee.Name}, he/she needs {model.Title}. Additional information: {model.Info} </p>";

            EmailHelper.SendEmail(emails, subject, body);

            return Ok();
        } 
    }
}