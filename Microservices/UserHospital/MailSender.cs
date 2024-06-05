﻿
using UserHospital.GlobalExceptions;

namespace UserHospital
{
    public class MailSender
    {
        public static void sendMail(string ToMail,string subject, string message)
        {
            System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
            try
            {
                mailMessage.From = new System.Net.Mail.MailAddress("thanaykumaryr@outlook.com", "FUNDOO NOTES");
                mailMessage.To.Add(ToMail);
                mailMessage.Subject = subject;
                mailMessage.Body = message;
                mailMessage.IsBodyHtml = true;
                System.Net.Mail.SmtpClient smtpClient = new System.Net.Mail.SmtpClient("smtp-mail.outlook.com");

                smtpClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;

                smtpClient.Port = 587;

                // Enable SSL/TLS
                smtpClient.EnableSsl = true;

                string loginName = "thanaykumaryr@outlook.com";
                string loginPassword = "thanay@123";

                System.Net.NetworkCredential networkCredential = new System.Net.NetworkCredential(loginName, loginPassword);
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = networkCredential;

                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught: " + ex.Message);
                throw new EmailSendingException("Failed to send email: " + ex.Message);
            }
            finally
            {
                mailMessage.Dispose();
            }
        }

    }
}
