using System;


namespace BSSiseveeb.Core.Dto
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Birthdate { get; set; }
        public DateTime ContractStart { get; set; }
        public DateTime? ContractEnd { get; set; }
        public string PhoneNumber { get; set; }
        public int VacationDays { get; set; }
        public string Email { get; set; }
    }
}