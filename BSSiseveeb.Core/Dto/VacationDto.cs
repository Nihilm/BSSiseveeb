using System;
using BSSiseveeb.Core.Domain;

namespace BSSiseveeb.Core.Dto
{
    public class VacationDto
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Days { get; set; }
        public string EmployeeId { get; set; }
        public VacationStatus Status { get; set; }
        public string Comments { get; set; }
    }
}