using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BankTransaction.BAL.Abstract;
using BankTransaction.Configuration;
using BankTransaction.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace BankTransaction.BAL.Implementation.Infrastucture
{
    public class EmailSender : ISender
    {
        private readonly EmailConfig emailConfig;
        private readonly ILogger<EmailSender> logger;

        public EmailSender(EmailConfig emailConfig, ILogger<EmailSender> logger)
        {
            this.emailConfig = emailConfig;
            this.logger = logger;
        }
        public async Task SendEmailAsync(CustomMessage message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            var messBodyBuilder = new BodyBuilder { HtmlBody = string.Format("<h3>{0}</h3>", message.Content) };
            if (message.Attachments != null && message.Attachments.Any())
            {
                byte[] fileBytes;
                foreach (var attachment in message.Attachments)
                {
                    using (var memStream = new MemoryStream())
                    {
                        attachment.CopyTo(memStream);
                        fileBytes = memStream.ToArray();
                    }

                    messBodyBuilder.Attachments.Add(attachment.FileName, fileBytes);
                }
            }
            emailMessage.Body = messBodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                try
                {
                    client.CheckCertificateRevocation = false;

                    await client.ConnectAsync(emailConfig.SmtpServer, emailConfig.Port, SecureSocketOptions.Auto);
                    //await client.ConnectAsync(emailConfig.SmtpServer, emailConfig.Port, true);
                    await client.AuthenticateAsync(emailConfig.UserName, emailConfig.Password);
                    await client.SendAsync(emailMessage);

                }
                catch (Exception ex)
                {
                    logger.LogError($"Exeption on email sending {0}", ex.Message);
                }
                finally
                {
                    await client.DisconnectAsync(true);
                    client.Dispose();
                }

            }
        }
    }
}
