using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using AssetsMonitor.Interfaces;
using Microsoft.Extensions.Logging;
using AssetsMonitor.Settings;
using System.IO;

namespace AssetsMonitor.Services
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly SmtpSettings _smtpSettings;
        private readonly ILogger<EmailSenderService> _logger;
        private readonly AlertSettings _alertSettings;

        public EmailSenderService(SmtpSettings smtpSettings, ILogger<EmailSenderService> logger, AlertSettings alertSettings)
        {
            _smtpSettings = smtpSettings;
            _logger = logger;
            _alertSettings = alertSettings;
        }

        public async Task SendAlertEmailAsync(string subject, string symbol, decimal price, string action)
        {
            _logger.LogInformation("Iniciando o envio de email para {Email}", _alertSettings.RecipientEmail);

            try
            {
                using (var client = ConfigureSmtpClient())
                {
                    var mailMessage = CreateAlertMailMessage(subject, action, symbol, price);
                    await client.SendMailAsync(mailMessage);
                }

                _logger.LogInformation("Email enviado com sucesso para {Email}", _alertSettings.RecipientEmail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao enviar email para {Email}", _alertSettings.RecipientEmail);
                throw;
            }
        }

        private SmtpClient ConfigureSmtpClient()
        {
            var client = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port)
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
                EnableSsl = _smtpSettings.EnableSsl,
            };

            _logger.LogInformation("Conexão com {Smtp} estabelecida com sucesso", _smtpSettings.Host);
            return client;
        }

        private MailMessage CreateAlertMailMessage(string subject, string action, string symbol, decimal price)
        {
            string templatePath = Path.Combine(AppContext.BaseDirectory, _alertSettings.TemplatePath);
            string emailBody = File.ReadAllText(templatePath);

            emailBody = emailBody.Replace("{{Symbol}}", symbol)
                                 .Replace("{{Price}}", price.ToString("C"))
                                 .Replace("{{Date}}", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"))
                                 .Replace("{{Action}}", action);

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_smtpSettings.Username),
                Subject = subject,
                Body = emailBody,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(_alertSettings.RecipientEmail);
            return mailMessage;
        }
    }
}