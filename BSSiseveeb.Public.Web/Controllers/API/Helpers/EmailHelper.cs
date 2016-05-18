using BSSiseveeb.Core.Domain;
using Castle.Core.Internal;
using System.Collections.Generic;
using System.Net.Mail;

namespace BSSiseveeb.Public.Web.Controllers.API.Helpers
{
    public class EmailHelper
    {
        public static void SendEmail(IEnumerable<string> emails, string subject, string body)
        {
            var message = new MailMessage();
            foreach (var email in emails)
            {
                message.To.Add(new MailAddress(email));
            }
            if (!emails.IsNullOrEmpty())
            {
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true;
                using (var smtp = new SmtpClient())
                {
                    smtp.Send(message);
                }
            }
        }

        public static void VacationModified(Vacation vacation, IEnumerable<string> emails)
        {
            var start = vacation.StartDate.ToString("d");
            var end = vacation.EndDate.ToString("d");
            var subject = "Teie Brightspark puhkus muudeti administraatori poolt";
            var body = "<h2>Teie puhkust muudeti</h2></br>" +
                $"<p>Nüüd teie puhkus algab {start} ja lõppeb {end} </p>" +
                "<p>Kui teil on küsimusi, siis kontakteeruge administraatoriga</p>";
            SendEmail(emails, subject, body);
        }

        public static void VacationRequested(Vacation vacation, IEnumerable<string> emails)
        {
            var start = vacation.StartDate.ToString("d");
            var end = vacation.EndDate.ToString("d");
            var subject = $"Uus puhkuse taotlus {vacation.Employee.Name} poolt";
            var body = $"<p>{vacation.Employee.Name} taotleb puhkust {start} - {end} perioodiks.</p>";
            SendEmail(emails, subject, body);
        }

        public static void VacationApproved(Vacation vacation, IEnumerable<string> emails)
        {
            var start = vacation.StartDate.ToString("d");
            var end = vacation.EndDate.ToString("d");
            var subject = "Teie puhkuse taotlus on heakskiidetud";
            var body = $"<p>Teie taotletud puhkus perioodiks {start} - {end} on heakskiidetud ja lisatud puhkuste kalendrisse. </p>" +
                "<p>Kui teil on küsimusi, siis kontakteeruge administraatoriga</p>";
            SendEmail(emails, subject, body);
        }

        public static void VacationDenied(Vacation vacation, IEnumerable<string> emails)
        {
            var start = vacation.StartDate.ToString("d");
            var end = vacation.EndDate.ToString("d");
            var subject = "Teie puhkuse taotlus on tagasilükatud";
            var body = $"<p>Teie taotletud puhkus perioodiks {start} - {end} on tagasilükatud. </p>" +
                "<p>Kui teil on küsimusi, siis konktakteeruge administraatoriga</p>";
            SendEmail(emails, subject, body);
        }

        public static void RequestRequested(Request request, IEnumerable<string> emails)
        {
            var subject = $"Uus töömaterjalide taotlus {request.Employee.Name} poolt";
            var body = $"<p> {request.Employee.Name} taotleb endale järgnevaid töövahendeid: </br>" +
                $"{request.Req} - {request.Description} <br/> Taotlus edastati: {request.TimeStamp}</p>";
            SendEmail(emails, subject, body);
        }

        public static void RequestApproved(Request request, IEnumerable<string> emails)
        {
            var subject = "Teie taotlus töömaterjalide järele sai heakskiidu";
            var body = $"<p>Teie taotlus töömaterjalide {request.Req} järele sai heakskiidu.</p>" +
                "<p>Kui teil on küsimusi, siis kontakteeruge administraatoriga</p>";
            SendEmail(emails, subject, body);
        }

        public static void RequestDenied(Request request, IEnumerable<string> emails)
        {
            var subject = "Teie taotlus töömaterjalide järele lükati tagasi";
            var body = $"<p>Teie taotlus töömaterjalide {request.Req} järele lükati tagasi.</p>" +
                "<p>Kui teil on küsimusi, siis kontakteeruge administraatoriga</p>";
            SendEmail(emails, subject, body);
        }

        public static void TodayBirthday(IEnumerable<Employee> birthdays, IEnumerable<string> emails)
        {
            var subject = "Tänased sünnipäevalised!";
            var body = $"<ul><li>Täna on sünnipäev:</li>";

            foreach (var birthday in birthdays)
            {
                body += $"<li>{birthday.Name}</li>";
            }

            body += "</ul></br><h3>Soovime neile kõigile head sünnipäeva!</h3>";
            SendEmail(emails, subject, body);
        }
        
        public static void MonthBirthday(IEnumerable<Employee> birthdays, IEnumerable<string> emails)
        {
            var subject = "Selle kuu sünnipäevalised!";
            var body = $"<ul><li>Sellel kuul on sünnipäev:</li>";

            foreach (var birthday in birthdays)
            {
                body += $"<li>{birthday.Name}: {birthday.Birthdate.Value.ToString("d")}</li>";
            }

            body += "</ul></br><h3>Soovime neile kõigile head sünnipäeva!</h3>";
            SendEmail(emails, subject, body);
        }
    }
}