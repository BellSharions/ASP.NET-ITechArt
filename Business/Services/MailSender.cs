using System;
using System.Threading.Tasks;
using Business.Interfaces;
using MailKit.Net.Smtp;
using MimeKit;
using Serilog;

namespace Business
{
    public class MailSender : ISmtpService
    {
        private readonly string _host;
        private readonly int _port;
        private readonly string _username;
        private readonly string _password;
        private readonly string _domain;
        public MailSender(SmtpOptions options)
        {
            _host = options.Host;
            _port = options.Port;
            _username = options.Username;
            _password = options.Password;
            _domain = options.Domain;
        }
        
        public async Task SendAsync(string to, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress((string)null, address: _username + _domain));
            message.To.Add(new MailboxAddress((string)null, to));
            message.Subject = subject;
            message.Body = new TextPart("plain") { Text = body };
            try
            {
                using var client = new SmtpClient();
                await client.ConnectAsync(_host, _port, true); 
                await client.AuthenticateAsync(_username, _password);
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
