using System.Collections.Generic;
using BSSiseveeb.Core.Dto;

namespace BSSiseveeb.Public.Web.Models
{
    public class WorkersViewModel
    {
        public IList<EmployeeDto> Employees { get; set; }
    }

    public class IndexViewModel
    {
        public IList<string> Vacations { get; set; }
        public IList<EmployeeDto> Employees { get; set; }
    }
}