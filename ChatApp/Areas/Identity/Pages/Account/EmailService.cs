using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Client;
using sib_api_v3_sdk.Model;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

public class EmailService
{
    public async Task<bool> SendEmailAsync(string email, string subject, string confirmLink)
    {
        try
        {
            Configuration.Default.ApiKey["api-key"] = "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";

            var apiInstance = new TransactionalEmailsApi();
            string SenderName = "ChatApp";
            string SenderEmail = "wulaia.anri@gmail.com";
            SendSmtpEmailSender Email = new SendSmtpEmailSender(SenderName, SenderEmail);
            string ToName = "NewUser";
            SendSmtpEmailTo smtpEmailTo = new SendSmtpEmailTo(email, ToName);
            List<SendSmtpEmailTo> To = new List<SendSmtpEmailTo>();
            To.Add(smtpEmailTo);

            var sendSmtpEmail = new SendSmtpEmail(Email, To, null, null, confirmLink, null, subject);
            CreateSmtpEmail result = await apiInstance.SendTransacEmailAsync(sendSmtpEmail);

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
    }

}
