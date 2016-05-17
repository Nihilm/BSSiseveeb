using System;
using Sparkling.DataInterfaces.Domain;

namespace BSSiseveeb.Core.Domain
{
    public class Request : Entity
    {
        public string Req { get; set; }
        public string Description { get; set; }
        public RequestStatus Status { get; set; }
        public string EmployeeId { get; set; }
        public DateTime TimeStamp { get; set; }
        public bool Cleared { get; set; }

        public virtual Employee Employee { get; set; }
    }

    public enum RequestStatus
    {
        Confirmed,
        Declined,
        Pending,
        Cancelled
    }
}