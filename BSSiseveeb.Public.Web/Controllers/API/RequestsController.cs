using System;
using System.Linq;
using System.Web.Http;
using System.Web.WebPages;
using BSSiseveeb.Core.Domain;
using BSSiseveeb.Public.Web.Attributes;
using BSSiseveeb.Public.Web.Controllers.API.Helpers;
using BSSiseveeb.Public.Web.Models;
using BSSiseveeb.Core.Mappers;

namespace BSSiseveeb.Public.Web.Controllers.API
{
    [Authorize]
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

            var request = new Request()
            {
                Req = title,
                Description = info,
                EmployeeId = CurrentUser.Id,
                Status = RequestStatus.Pending,
                TimeStamp = DateTime.Now
            };

            RequestRepository.AddIfNew(request);

            RequestRepository.Commit();
            var emails = EmployeeRepository
                    .Where(x => x.RequestMessages == true && x.Role.Rights.HasFlag(AccessRights.Requests)).Select(x => x.Email).ToList();

            EmailHelper.RequestRequested(request, emails);

            return Ok();
        }

        [HttpGet]
        [AuthorizeApi(AccessRights.Standard)]
        public IHttpActionResult GetMyRequest()
        {
            return Ok(RequestRepository
                .Where(x => x.EmployeeId == CurrentUser.Id && x.Cleared == false)
                .OrderByDescending(x => x.TimeStamp)
                .AsDto());
        }

        [HttpPost]
        [AuthorizeApi(AccessRights.Standard)]
        public IHttpActionResult RemoveVacation(GeneralIdModel model)
        {
            var request = RequestRepository.First(x => x.Id == model.Id);
            if(request.Status == RequestStatus.Pending)
            {
                request.Status = RequestStatus.Cancelled;
            }
            request.Cleared = true;
            RequestRepository.SaveOrUpdate(request);
            RequestRepository.Commit();

            return Ok();
        }
    }
}