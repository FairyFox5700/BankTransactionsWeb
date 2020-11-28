using Microsoft.AspNetCore.Http;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BankTransaction.Models
{
    public class CustomMessage
    {

        public List<MailboxAddress> To { get; set; } = new List<MailboxAddress>();
        public string Subject { get; set; }
        public string Content { get; set; }
        public IFormFileCollection Attachments { get; set; }
        public CustomMessage(IEnumerable<string> to, string subject, string content, IFormFileCollection attachments)
        {
            To.AddRange(to.Select(adressInString => new MailboxAddress(adressInString)));
            Subject = subject;
            Content = content;
            Attachments = attachments;
        }

    }
}
