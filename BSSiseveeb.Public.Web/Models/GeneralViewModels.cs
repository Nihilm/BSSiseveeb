using BSSiseveeb.Core.Dto;
using System;
using System.Collections.Generic;

namespace BSSiseveeb.Public.Web.Models
{
    public class WorkersViewModel
    {
        public WorkersViewModel()
        {
            ImportViewModel = new CsvImportViewModel();
        }

        public IList<EmployeeDto> Employees { get; set; }
        public CsvImportViewModel ImportViewModel { get; set; }
    }

    public class IndexViewModel
    {
        public IList<EmployeeVacationModel> Vacations { get; set; }
        public IList<EmployeeDto> Employees { get; set; }
    }

    public class EmployeeVacationModel
    {
        public string EmployeeName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string StartDateFormatted => StartDate.ToString("d");
        public string EndDateFormatted => EndDate.ToString("d");
    }
}