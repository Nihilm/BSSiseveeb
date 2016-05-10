using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BSSiseveeb.Public.Web.Models
{
    public class RegistrationModel : BaseViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime? Start { get; set; }

        [DataType(DataType.Date)]
        public DateTime? End { get; set; }

        [DataType(DataType.Date)]
        public DateTime? BirthDay { get; set; }

        [Required]
        public int VacationDays { get; set; }

        public string OldRole { get; set; }

        [Display(Name= "Role")]
        public string NewRole { get; set; }

        public List<string> Roles { get; set; } 
    }

    public class ChangeAccountSettingsViewModel : BaseViewModel
    {
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Display(Name = "Messages about vacations")]
        public bool VacationMessages { get; set; }

        [Display(Name = "Messages about requests")]
        public bool RequestMessages { get; set; }

        [Display(Name = "Subscribe to monthly birthday message")]
        public bool MonthlyBirthdayMessages { get; set; }

        [Display(Name = "Subscribe to daily birthday message")]
        public bool DailyBirthdayMessages { get; set; }

        public string Message { get; set; }
    }

}
