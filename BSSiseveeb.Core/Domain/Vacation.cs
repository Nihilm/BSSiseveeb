using System;
using Sparkling.DataInterfaces.Domain;

namespace BSSiseveeb.Core.Domain
{
    public class Vacation : Entity
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Days { get; set; }
        public int EmployeeId { get; set; }
        public VacationStatus Status { get; set; }
    }
    public enum VacationStatus
    {
        Approved,
        Declined,
        Pending
    }
}
