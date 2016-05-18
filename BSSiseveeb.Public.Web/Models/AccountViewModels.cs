using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BSSiseveeb.Public.Web.Models
{
    public class RegistrationModel
    {
        public string Id { get; set; }
        public string Name { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Telefon: ")]
        public string Phone { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Lepingu algus: ")]
        public DateTime? Start { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Lepingu lõpp: ")]
        public DateTime? End { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Sünnipäev: ")]
        public DateTime? BirthDay { get; set; }

        [Required]
        [Display(Name = "Puhkusepäevad: ")]
        public int VacationDays { get; set; }

        public string OldRole { get; set; }

        [Display(Name = "Roll: ")]
        public string NewRole { get; set; }

        [Display(Name = "Skype: ")]
        public string Skype { get; set; }

        [Display(Name = "Isikukood: ")]
        public string SocialSecurityID { get; set; }

        public List<string> Roles { get; set; }
    }

    public class ChangeAccountSettingsViewModel
    {
        [DataType(DataType.Date)]
        [Display(Name = "Sünnipäev: ")]
        public DateTime? BirthDay { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Telefon: ")]
        public string Phone { get; set; }

        [Display(Name = "Teated puhkuste kohta")]
        public bool VacationMessages { get; set; }

        [Display(Name = "Teated töömaterjalide taotluste kohta")]
        public bool RequestMessages { get; set; }

        [Display(Name = "Igakuine sünnipäevade meeldetuletus")]
        public bool MonthlyBirthdayMessages { get; set; }

        [Display(Name = "Igapäevane sünnipäevade meeldetuletus")]
        public bool DailyBirthdayMessages { get; set; }

        [Display(Name = "Skype: ")]
        public string Skype { get; set; }

        [Display(Name = "Isikukood: ")]
        public string SocialSecurityID { get; set; }

        public string Message { get; set; }
    }
}
