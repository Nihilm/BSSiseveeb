using System;
using BSSiseveeb.Core.Domain;

namespace BSSiseveeb.Core.Dto
{
    public class RequestDto
    {
        public int Id { get; set; }
        public string Req { get; set; }
        public string Description { get; set; }
        public RequestStatus Status { get; set; }
        public string EmployeeId { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}