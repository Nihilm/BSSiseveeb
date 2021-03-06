﻿using System;
using Sparkling.DataInterfaces.Domain;

namespace BSSiseveeb.Core.Domain
{
    public class Vacation : Entity
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Days { get; set; }
        public string EmployeeId { get; set; }
        public VacationStatus Status { get; set; }
        public string Comments { get; set; }

        public virtual Employee Employee { get; set; }
    }

    public enum VacationStatus
    {
        Approved,
        Declined,
        Pending,
        Retired
    }
}
