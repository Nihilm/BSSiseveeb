using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using BSSiseveeb.Core.Contracts.Repositories;
using BSSiseveeb.Core.Domain;
using BSSiseveeb.Public.Web.Attributes;
using BSSiseveeb.Public.Web.Models;

namespace BSSiseveeb.Public.Web.Controllers.API
{
    [AuthorizeLevel(AccessRights.Level5)]
    public class AdminApiController : BaseApiController
    {


        [HttpGet]
        public IEnumerable<Vacation> GetPendingVacations()
        {
            return VacationRepository.Where(x => x.Status == VacationStatus.Pending).ToList();
        }

        [HttpGet]
        public IEnumerable<Request> GetPendingRequests()
        {
            return RequestRepository.Where(x => x.Status == RequestStatus.Pending).ToList();
        }

        [HttpPost]
        public IHttpActionResult ApproveVacation(ApiControllerModels.GeneralIdModel model)
        {
            var id = model.Id;
            var vacation = VacationRepository.First(x => x.Id == id);
            vacation.Status = VacationStatus.Approved;
            VacationRepository.SaveOrUpdate(vacation);
            VacationRepository.Commit();
            return Ok();
        }

        [HttpPost]
        public IHttpActionResult DeclineVacation(ApiControllerModels.GeneralIdModel model)
        {
            var id = model.Id;
            var vacation = VacationRepository.First(x => x.Id == id);
            vacation.Status = VacationStatus.Declined;
            VacationRepository.SaveOrUpdate(vacation);
            VacationRepository.Commit();
            return Ok();
        }

        [HttpPost]
        public IHttpActionResult ApproveRequest(ApiControllerModels.GeneralIdModel model)
        {
            var id = model.Id;
            var request = RequestRepository.First(x => x.Id == id);
            request.Status = RequestStatus.Confirmed;
            RequestRepository.SaveOrUpdate(request);
            RequestRepository.Commit();
            return Ok();
        }

        [HttpPost]
        public IHttpActionResult DeclineRequest(ApiControllerModels.GeneralIdModel model)
        {
            var id = model.Id;
            var request = RequestRepository.First(x => x.Id == id);
            request.Status = RequestStatus.Declined;
            RequestRepository.SaveOrUpdate(request);
            RequestRepository.Commit();
            return Ok();
        }

        [HttpGet]
        public IEnumerable<Vacation> GetConfirmedVacations()
        {
            return VacationRepository.Where(x => x.Status == VacationStatus.Approved).ToList();
        }

        [HttpPost]
        public IHttpActionResult ModifyVacation(ApiControllerModels.VacationModel model)
        {
            var id = model.Id;
            if (model.End < model.Start)
            {
                return BadRequest();
            }

            var vacation = VacationRepository.First(x => x.Id == id);
            vacation.StartDate = model.Start;
            vacation.EndDate = model.End;
            VacationRepository.SaveOrUpdate(vacation);
            VacationRepository.Commit();
            return Ok();
        }

        [HttpPost]
        public IHttpActionResult DeleteVacation(ApiControllerModels.GeneralIdModel model)
        {
            var id = model.Id;
            VacationRepository.Delete(id);
            VacationRepository.Commit();
            return Ok();
        }
    }
}