using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BSSiseveeb.Core.Domain;
using BSSiseveeb.Core.Dto;

namespace BSSiseveeb.Core.Mappers
{
    public static class EmployeeMapper
    {
        public static EmployeeDto AsDto(this Employee employee)
        {
            return new EmployeeDto()
            {
                ContractStart = employee.ContractStart,
                VacationDays = employee.VacationDays,
                Id = employee.Id,
                Name = employee.Name,
                PhoneNumber = employee.PhoneNumber,
                Email = employee.Email,
                Birthdate = employee.Birthdate,
                ContractEnd = employee.ContractEnd
            };
        }

        public static IList<EmployeeDto> AsDto(this IQueryable<Employee> query)
        {
            return query.Select(AsDto()).ToList();
        }

        public static Expression<Func<Employee, EmployeeDto>> AsDto()
        {
            return (e) => new EmployeeDto()
            {
                Birthdate = e.Birthdate,
                ContractEnd = e.ContractEnd,
                VacationDays = e.VacationDays,
                Id = e.Id,
                Name = e.Name,
                PhoneNumber = e.PhoneNumber,
                Email = e.Email,
                ContractStart = e.ContractStart
            };
        }
    }
}