using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EnviarEmailprbJony
{
    public class EmailSenderService
    {
        private readonly string _smtpHost;
        private readonly int _smtpPort;

        public EmailSenderService(string smtpHost = "smtp.gmail.com", int smtpPort = 587)
        {
            _smtpHost = smtpHost;
            _smtpPort = smtpPort;
        }

        public async Task<bool> SendEmailAsync(string toEmail, string subject, string body, string userName, string password)
        {
            try
            {
                var smtpClient = new SmtpClient(_smtpHost)
                {
                    Port = _smtpPort,
                    Credentials = new NetworkCredential(userName, password),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(userName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(toEmail);

                await smtpClient.SendMailAsync(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                // Aquí puedes manejar el error o registrar el problema
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }
    }
}
