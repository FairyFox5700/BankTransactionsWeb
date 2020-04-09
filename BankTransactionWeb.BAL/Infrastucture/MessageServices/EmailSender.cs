using BankTransactionWeb.BAL.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankTransactionWeb.BAL.Infrastucture.MessageServices
{
    public class CustomMessage
    {

        public List<MailboxAddress> To { get; set; } = new List<MailboxAddress>();
        public string Subject { get; set; }
        public string Content { get; set; }
        public IFormFileCollection Attachments { get; set; }
        public CustomMessage(IEnumerable<string> to, string subject, string content, IFormFileCollection attachments)
        {
            To.AddRange(to.Select(adressInString=> new MailboxAddress(adressInString)));
            Subject = subject;
            Content = content;
            Attachments = attachments;
        }

    }
    public class EmailSender: ISender
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
            if(message.Attachments!=null && message.Attachments.Any())
            {
                byte[] fileBytes;
                foreach (var attachment in message.Attachments)
                {
                    using(var memStream = new MemoryStream())
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
                    await client.ConnectAsync(emailConfig.SmtpServer, emailConfig.Port, true);
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

