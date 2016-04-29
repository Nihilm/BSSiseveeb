using System;
using System.Web.Http;
using System.Web.WebPages;
using BSSiseveeb.Core.Domain;
using BSSiseveeb.Public.Web.Attributes;
using BSSiseveeb.Public.Web.Models;

namespace BSSiseveeb.Public.Web.Controllers.API
{
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
            return Ok();
        }
    }
}