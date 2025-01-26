using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using AssetsMonitor.Interfaces;
using AssetsMonitor.Models;
using Microsoft.Extensions.Logging;
using AssetsMonitor.Settings;

namespace AssetsMonitor.Services
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly SmtpSettings _smtpSettings;
        private readonly ILogger<EmailSenderService> _logger;

        public EmailSenderService(SmtpSettings smtpSettings, ILogger<EmailSenderService> logger)
        {
            _smtpSettings = smtpSettings;
            _logger = logger;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            _logger.LogInformation("Iniciando o envio de email para {Email}", email);

            try
            {
                var client = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port)
                {
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
                    EnableSsl = _smtpSettings.EnableSsl,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_smtpSettings.Username),
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = true,
                };

                mailMessage.To.Add(email);

                await client.SendMailAsync(mailMessage);

                _logger.LogInformation("Email enviado com sucesso para {Email}", email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao enviar email para {Email}", email);
                throw;
            }
        }
    }
}