using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BSSiseveeb.Core.Domain;
using BSSiseveeb.Core.Dto;

namespace BSSiseveeb.Public.Web.Models
{
    public class WorkersViewModel
    {
        public IList<EmployeeDto> Employees { get; set; }
    }
    public class IndexViewModel
    {
        public List<string> Vacations { get; set; }
        public IList<EmployeeDto> Employees { get; set; }
    }
}