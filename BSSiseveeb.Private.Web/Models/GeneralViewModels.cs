using System.Collections.Generic;
using BSSiseveeb.Core.Dto;

namespace BSSiseveeb.Private.Web.Models
{
    public class WorkersViewModel
    {
        public List<EmployeeDto> Employees { get; set; }
    }

    public class IndexViewModel
    {
        public List<string> Vacations { get; set; }
        public List<EmployeeDto> Employees { get; set; }
    }
}