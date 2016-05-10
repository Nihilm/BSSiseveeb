using System;
using Sparkling.DataInterfaces.Domain;

namespace BSSiseveeb.Core.Domain
{
    public class Employee : EntityWithTypedId<string>
    {
        public string Name { get; set; }
        public DateTime? Birthdate { get; set; }
        public DateTime? ContractStart { get; set; }
        public DateTime? ContractEnd { get; set; }
        public string PhoneNumber { get; set; }
        public int VacationDays { get; set; }
        public string Email { get; set; }
        public bool VacationMessages { get; set; }
        public bool RequestMessages { get; set; }
        public bool MonthlyBirthdayMessages { get; set; }
        public bool DailyBirthdayMessages { get; set; }
        public int RoleId { get; set; }
        public bool IsInitialized { get; set; }

        public virtual Role Role { get; set; }


        public bool Equals(Employee other)
        {
            if (other == null)
                return false;

            if (this.Id == other.Id)
                return true;
            else
                return false;
        }

        public override bool Equals(Object obj)
        {
            if (obj == null)
                return false;

            Employee employeeObj = obj as Employee;
            if (employeeObj == null)
                return false;
            else
                return Equals(employeeObj);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public static bool operator == (Employee emp1, Employee emp2)
        {
            if (((object)emp1) == null || ((object)emp2) == null)
                return Object.Equals(emp1, emp2);

            return emp1.Equals(emp2);
        }

        public static bool operator != (Employee emp1, Employee emp2)
        {
            if (((object)emp1) == null || ((object)emp2) == null)
                return !Object.Equals(emp1, emp2);

            return !(emp1.Equals(emp2));
        }
    }
}