using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sparkling.DataInterfaces.Domain;

namespace BSSiseveeb.Core.Domain
{
    public class Employee : Entity
    {
        public string Name { get; set; }
        public DateTime Birthdate { get; set; }
        public DateTime ContractStart { get; set; }
        public DateTime? ContractEnd { get; set; }
        public string PhoneNumber { get; set; }
        public int VacationDays { get; set; }
    }
}