using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace Infrastructure.Services
{
    /// <summary>
    /// Email service implementation using SMTP (Gmail).
    /// Configuration from appsettings.json
    /// </summary>
    public class EmailService : IEmailService
    {
        private readonly string _host;
        private readonly int _port;
        private readonly string _fromAddress;
        private readonly string _password;
        private readonly bool _enableSsl;

        public EmailService(IConfiguration configuration)
        {
            _host = configuration["EmailSettings:Host"] ?? throw new InvalidOperationException("Email host not configured");
            _port = int.Parse(configuration["EmailSettings:Port"] ?? "587");
            _fromAddress = configuration["EmailSettings:FromAddress"] ?? throw new InvalidOperationException("Email address not configured");
            _password = configuration["EmailSettings:Password"] ?? throw new InvalidOperationException("Email password not configured");
            _enableSsl = bool.Parse(configuration["EmailSettings:EnableSsl"] ?? "true");
        }

        public async Task SendAsync(string to, string subject, string body)
        {
            try
            {
                using (var client = new SmtpClient(_host, _port))
                {
                    client.EnableSsl = _enableSsl;
                    client.Credentials = new NetworkCredential(_fromAddress, _password);
                    client.Timeout = 10000;

                    var message = new MailMessage
                    {
                        From = new MailAddress(_fromAddress),
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = true
                    };

                    message.To.Add(to);

                    await client.SendMailAsync(message);
                    
                    System.Diagnostics.Debug.WriteLine($"Email sent successfully to {to}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error sending email to {to}: {ex.Message}");
                throw;
            }
        }

        public async Task SendWithAttachmentAsync(
            string to,
            string subject,
            string body,
            string attachmentFileName,
            Stream attachmentStream)
        {
            try
            {
                using (var client = new SmtpClient(_host, _port))
                {
                    client.EnableSsl = _enableSsl;
                    client.Credentials = new NetworkCredential(_fromAddress, _password);
                    client.Timeout = 10000;

                    var message = new MailMessage
                    {
                        From = new MailAddress(_fromAddress),
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = true
                    };

                    message.To.Add(to);

                    // Add attachment
                    attachmentStream.Position = 0;
                    var attachment = new Attachment(attachmentStream, attachmentFileName);
                    message.Attachments.Add(attachment);

                    await client.SendMailAsync(message);

                    System.Diagnostics.Debug.WriteLine($"Email with attachment sent successfully to {to}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error sending email with attachment to {to}: {ex.Message}");
                throw;
            }
        }
    }
}
