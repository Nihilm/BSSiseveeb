using System;


namespace BSSiseveeb.Core.Dto
{
    public class VacationDto
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Days { get; set; }
        public int EmployeeId { get; set; }
        public Domain.VacationStatus Status { get; set; }
        public string Comments { get; set; }
    }
}
