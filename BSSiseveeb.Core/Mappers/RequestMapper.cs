using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BSSiseveeb.Core.Domain;
using BSSiseveeb.Core.Dto;

namespace BSSiseveeb.Core.Mappers
{
    public static class RequestMapper
    {
        public static RequestDto AsDto(this Request request)
        {
            return new RequestDto()
            {
                Req = request.Req,
                Description = request.Description,
                Id = request.Id,
                Status = request.Status,
                EmployeeId = request.EmployeeId,
                TimeStamp = request.TimeStamp
            };
        }

        public static IList<RequestDto> AsDto(this IQueryable<Request> query)
        {
            return query.Select(AsDto()).ToList();
        }

        public static Expression<Func<Request, RequestDto>> AsDto()
        {
            return (request) => new RequestDto()
            {
                Req = request.Req,
                Description = request.Description,
                Id = request.Id,
                Status = request.Status,
                EmployeeId = request.EmployeeId,
                TimeStamp = request.TimeStamp
            };
        }
    }
}
