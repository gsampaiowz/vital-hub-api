namespace WebAPI.Utils.Mail
{
    public class MailRequest
    {
        //Email do destinatario
        public string? ToEmail { get; set; }


        //Assunto do email
        public string? Subject { get; set;}

        //Corpo do email
        public string? Body { get; set;}

    }
}
