using System;


namespace BSSiseveeb.Core.Dto
{
    public class EmployeeDto
    {
        public string Id { get; set; }
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
    }
}