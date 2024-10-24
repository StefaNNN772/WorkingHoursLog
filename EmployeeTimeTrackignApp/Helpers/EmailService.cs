using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeTimeTrackignApp.Helpers
{
    public class EmailService
    {
        private string smtpServer;
        private int port;
        private string senderEmail;
        private string senderPassword;

        public EmailService()
        {
            this.smtpServer = "smtp.gmail.com";
            this.port = 587;
            this.senderEmail = "";
            this.senderPassword = "";
        }

        public bool SendEmail(string recipientEmail, string subject, string body)
        {
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(senderEmail);
                    mail.To.Add(recipientEmail);
                    mail.Subject = subject;
                    mail.Body = body;
                    mail.IsBodyHtml = true;

                    using (SmtpClient smtp = new SmtpClient(smtpServer, port))
                    {
                        smtp.Credentials = new NetworkCredential(senderEmail, senderPassword);
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }
    }
}
