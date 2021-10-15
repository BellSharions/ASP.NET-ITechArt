using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
using Serilog;
using DAL;

namespace Business
{
    public class MailSender
    {
        private readonly string _host;
        private readonly int _port;
        private readonly string _username;
        private readonly string _password;
        public MailSender(SmtpOptions options)
        {
            _host = options.Host;
            _port = options.Port;
            _username = options.Username;
            _password = options.Password;
        }
        
        public async Task SendAsync(string to, string subject, string body)
        {
            //var fromEmail = "nreply92@mail.ru"
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress((string)null, address: _username + "@mail.ru"));
            message.To.Add(new MailboxAddress((string)null, to));
            message.Subject = subject;
            message.Body = new TextPart("plain") { Text = body };
            try
            {
                using var client = new SmtpClient();
                await client.ConnectAsync(_host, _port, true); //("smtp.mail.ru", 465, true);
                await client.AuthenticateAsync(_username, _password); //("nreply92", "Qk2FeydHVAMEqg9e3Erd");
                await client.SendAsync(message).ConfigureAwait(false);
                await client.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Message wasn't sent");
            }
        }
    }
}
