using System.Collections.Generic;
using BSSiseveeb.Core.Dto;

namespace BSSiseveeb.Public.Web.Models
{
    public class WorkersViewModel : BaseViewModel
    {
        public List<EmployeeDto> Employees { get; set; }
    }

    public class IndexViewModel : BaseViewModel
    {
        public List<string> Vacations { get; set; }
        public List<EmployeeDto> Employees { get; set; }
    }
}