using System;


namespace BSSiseveeb.Core.Dto
{
    public class RequestDto
    {
        public int Id { get; set; }
        public string Req { get; set; }
        public string Description { get; set; }
        public Domain.RequestStatus Status { get; set; }
        public int EmployeeId { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}

