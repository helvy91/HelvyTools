using System.Net;
using HelvyTools.Smtp.Data;
using HelvyTools.Configuration;

using Net = System.Net.Mail;

namespace CKHome.Tools.Smtp
{
    public class SmtpClient
    {
        private readonly Configuration.SmtpConfiguration _config;

        public SmtpClient(Configuration.SmtpConfiguration config)
        {
            _config = config;
        }

        public Task SendAsync(MailMessage message)
        {
            using (var smtpMessage = new Net.MailMessage(_config.FromAddress, message.To))
            {
                smtpMessage.Subject = message.Subject;
                smtpMessage.Body = message.Body;

                using (var client = new Net.SmtpClient(_config.Host, _config.Port))
                {
                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential(_config.Username, _config.Password);

                    client.Send(smtpMessage);
                }
            }
            
            return Task.CompletedTask;
        }
    }
}
