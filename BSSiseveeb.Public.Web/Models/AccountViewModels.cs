using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BSSiseveeb.Public.Web.Models
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegistrationModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 6, ErrorMessage="Kasutajanimi peab olema vähemalt 6 tähemärki pikk")]
        public string Username { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 6)]
        public string Name { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Start { get; set; }

        [DataType(DataType.Date)]
        public DateTime? End { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime BirthDay { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Kasutajanimi peab olema vähemalt 6 tähemärki pikk")]
        public string Password { get; set; }

        [Required]
        public int VacationDays { get; set; }

        public string OldRole { get; set; }

        [Display(Name= "Role")]
        public string NewRole { get; set; }

        public List<string> Roles { get; set; } 
    }

    public class ChangeAccountSettingsViewModel
    {
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [StringLength(50, MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [StringLength(50, MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Message { get; set; }

        public string Messages { get; set; }
    }

}
