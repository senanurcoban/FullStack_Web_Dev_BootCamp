using System.Net;                // Bu kütüphaneler ağ üzerinden iletişim ve e-posta gönderimi için gereklidir.
using System.Net.Mail;

namespace UserIdentityApp.Models
{
    
    public class SmtpEmailSender : IEmailSender{
        // SMTP sunucu bilgileri property olarak tanımlandı.
        private string? _host;
        private int _port;
        private bool _enableSSL;
        private string? _username;
        private string? _password;

        public SmtpEmailSender(string? host, int port,bool enableSSL,string? username,string? password){
            _host=host;
            _port=port;
            _enableSSL = enableSSL;
            _username = username;
            _password = password;
        }

        // E-posta gönderme işlevi için metod tanımlandı.
        public Task SendEmailAsync(string email, string subject, string message){
            // SmtpClient nesnesi ile mesaj gönderimi sağlayacağız.
            var client = new SmtpClient(_host,_port){
                Credentials = new NetworkCredential(_username, _password),   // SMTP'nin kimlik doğrulaması için kullanıcı adı,şifre ataması için.
                EnableSsl = _enableSSL    // SSL kullanımı belirtildi.
            };
            return client.SendMailAsync(new MailMessage(_username ?? "",email,subject,message){IsBodyHtml = true});
        }
    }
}