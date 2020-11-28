namespace BankTransaction.Configuration
{
    public class EmailConfig
    {
        public string From { get; set; }

        public string SmtpServer { get; set; }
        public bool EnableSSL { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public bool enable_starttls_auto { get; set; }

    }
}
