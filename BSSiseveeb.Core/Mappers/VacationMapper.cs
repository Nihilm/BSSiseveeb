using BSSiseveeb.Core.Domain;
using BSSiseveeb.Core.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BSSiseveeb.Core.Mappers
{
    public static class VacationMapper
    {
        public static VacationDto AsDto(this Vacation vacation)
        {
            return new VacationDto()
            {
                StartDate = vacation.StartDate,
                EndDate = vacation.EndDate,
                Id = vacation.Id,
                Days = vacation.Days,
                EmployeeId = vacation.EmployeeId,
                Status = vacation.Status,
                Comments = vacation.Comments
            };
        }

        public static IList<VacationDto> AsDto(this IQueryable<Vacation> query)
        {
            return query.Select(AsDto()).ToList();
        }

        public static Expression<Func<Vacation, VacationDto>> AsDto()
        {
            return (vacation) => new VacationDto()
            {
                StartDate = vacation.StartDate,
                EndDate = vacation.EndDate,
                Id = vacation.Id,
                Days = vacation.Days,
                EmployeeId = vacation.EmployeeId,
                Status = vacation.Status,
                Comments = vacation.Comments
            };
        }
    }
}
