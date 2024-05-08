namespace WebAPI.Utils.Mail
{
    public interface IEmailService
    {
        //Metodo assincrono para envio de e-mail
        Task SendEmailAsync(MailRequest mailRequest);
    }
}
