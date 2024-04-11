
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace WebAPI.Utils.Mail
    {
    public class EmailService : IEmailService
        {
        //variável que armazena as configs de EmailSettings
        private readonly EmailSettings emailSettings;

        //construtor que recebe a dependence injection de EmailSettings
        public EmailService(IOptions<EmailSettings> options)
            {
            emailSettings = options.Value;
            }

        //método para envio de e-mail
        public async Task SendEmailAsync(MailRequest mailRequest)
            {
            try
                {
                //objeto que representa o e-mail
                var email = new MimeMessage();

                //define o remetente do e-mail
                email.Sender = MailboxAddress.Parse(emailSettings.Email);

                //define o destinatário do e-mail
                email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));

                //define o assunto do e-mail
                email.Subject = mailRequest.Subject;

                //cria o corpo do e-mail
                var builder = new BodyBuilder();

                //define o corpo do e-mail como HTML
                builder.HtmlBody = mailRequest.Body;

                //define o corpo do e-mail no objeto MimeMessage
                email.Body = builder.ToMessageBody();

                //cria um cliente SMTP para envio de email
                using SmtpClient smtp = new();

                //conecta-se ao servidor SMTP usando os dados de emailSettings
                smtp.Connect(emailSettings.Host, emailSettings.Port, SecureSocketOptions.StartTls);

                //autentica-se no servidor SMTP usando os dados de emailSettings
                smtp.Authenticate(emailSettings.Email, emailSettings.Password);

                //envia o email
                await smtp.SendAsync(email);

                }
            catch (Exception)
                {

                throw;
                }
            }
        }
    }
