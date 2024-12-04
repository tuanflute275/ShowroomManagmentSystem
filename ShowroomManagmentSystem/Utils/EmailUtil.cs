namespace ShowroomManagmentSystem.Utils
{
    public static class EmailUtil
    {
        public static async Task SendEmailAsync(
            string toEmail,
            string subject,
            string bodyContent)
        {
            EmailModel model = new EmailModel()
            {
                Subject = subject,
                To = toEmail,
                From = "tuanflute275@gmail.com", 
                Password = "eyar ysoh lacl lpur"
            };

            using (MailMessage mm = new MailMessage(model.From, model.To))
            {
                mm.Subject = model.Subject;
                mm.Body = bodyContent;
                mm.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    NetworkCredential networkCred = new NetworkCredential(model.From, model.Password);
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = networkCred;
                    smtp.Port = 587;

                    await smtp.SendMailAsync(mm);
                }
            }
        }
    }

}
