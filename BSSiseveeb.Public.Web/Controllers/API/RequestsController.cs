using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.WebPages;
using BSSiseveeb.Core.Contracts.Repositories;
using BSSiseveeb.Core.Domain;
using BSSiseveeb.Public.Web.Attributes;
using BSSiseveeb.Public.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace BSSiseveeb.Public.Web.Controllers.API
{
    [AuthorizeLevel(AccessRights.Level1)]
    public class RequestsController : BaseApiController
    {
        [HttpPost]
        public IHttpActionResult SetRequest(ApiControllerModels.RequestModel model)
        {
            var title = model.Title;
            var info = model.Info;

            if (title.IsEmpty())
            {
                return BadRequest();
            }

            var currentUser =
                HttpContext.Current.GetOwinContext()
                    .GetUserManager<ApplicationUserManager>()
                    .FindById(User.Identity.GetUserId())
                    .EmployeeId;

            RequestRepository.AddIfNew(new Request()
            {
                Req = title,
                Description = info,
                EmployeeId = currentUser,
                Status = RequestStatus.Pending,
                TimeStamp = DateTime.Now
            });

            RequestRepository.Commit();
            return Ok();
        }
    }
}