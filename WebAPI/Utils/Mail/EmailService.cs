
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;

namespace WebAPI.Utils.Mail
{
    public class EmailService : IEmailService
    {
        //Variável que armazena as configs de EmailSettings
        private readonly EmailSettings emailSettings;
        
        //Construtor que recebe a dependence injection de EmailSettings
        public EmailService(IOptions <EmailSettings> options) 
        {
            emailSettings = options.Value;
                

        }

        //Método para envio de e-mail
        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            try
            {
                //Objeto que representa o e-mail
                var email = new MimeMessage();

                //Define o remetente do e-mail
                email.Sender = MailboxAddress.Parse(emailSettings.Email);

                email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));

                email.Subject = mailRequest.Subject;

                //Cria o corpo do e-mail
                var builder = new BodyBuilder();

                //Define o crpo do email como Html
                builder.HtmlBody = mailRequest.Body;

                //Define o corpo do email no objeto MimeMessage
                email.Body = builder.ToMessageBody();

                //Cria um cliente SMTP para envio de email
                using (var smtp = new SmtpClient()) 
                {
                    //Conecta-se ao servidor SMTP usando os dados de emailSettings
                    smtp.Connect(emailSettings.Host, emailSettings.Port, SecureSocketOptions.StartTls);
                    
                    //Autentica-se no servidor SMTP usando os daos de emailSettings
                    smtp.Authenticate(emailSettings.Email,emailSettings.Password);

                    //Envia o email
                    await smtp.SendAsync(email);

                }


            }
            catch (Exception)
            {
                throw;
            }

            
        }
    }
}
