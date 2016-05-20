using BSSiseveeb.Core.Domain;
using System.Collections.Generic;

namespace BSSiseveeb.Core.Contracts.Services
{
    public interface IEmailService : IApplicationService
    {
        void SendEmail(IEnumerable<string> emails, string subject, string body);
        void VacationModified(Vacation vacation, IEnumerable<string> emails);
        void VacationRequested(Vacation vacation, IEnumerable<string> emails);
        void VacationApproved(Vacation vacation, IEnumerable<string> emails);
        void VacationDenied(Vacation vacation, IEnumerable<string> emails);
        void RequestRequested(Request request, IEnumerable<string> emails);
        void RequestApproved(Request request, IEnumerable<string> emails);
        void RequestDenied(Request request, IEnumerable<string> emails);
        void TodayBirthday(IEnumerable<Employee> birthdays, IEnumerable<string> emails);
        void MonthBirthday(IEnumerable<Employee> birthdays, IEnumerable<string> emails);
    }
}
