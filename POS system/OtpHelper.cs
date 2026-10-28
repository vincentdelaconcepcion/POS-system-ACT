using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;

namespace POS_system
{
    internal static class OtpHelper
    {
        public static string Generate()
        {
            byte[] b = new byte[4];
            using (var rng = RandomNumberGenerator.Create()) rng.GetBytes(b);
            return (BitConverter.ToUInt32(b, 0) % 1000000).ToString("D6");
        }

        public static void SendEmail(string toEmail, string code)
        {
            string host = ConfigurationManager.AppSettings["SmtpHost"];
            string portText = ConfigurationManager.AppSettings["SmtpPort"];
            string user = ConfigurationManager.AppSettings["SmtpUser"];
            string pass = ConfigurationManager.AppSettings["SmtpPassword"];

            if (string.IsNullOrWhiteSpace(host) || string.IsNullOrWhiteSpace(portText) ||
                string.IsNullOrWhiteSpace(user))
                throw new InvalidOperationException("SMTP settings are missing in App.config.");

            if (string.IsNullOrWhiteSpace(pass))
                throw new InvalidOperationException(
                    "SmtpPassword is empty in App.config. Put your Google App Password there.");

            pass = pass.Replace(" ", "").Trim();
            int port = int.Parse(portText);

            // Gmail requires TLS 1.2
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;

            try
            {
                using (var msg = new MailMessage(user, toEmail))
                using (var smtp = new SmtpClient(host, port))
                {
                    msg.Subject = "POS System password reset code";
                    msg.Body = "Your verification code is: " + code +
                               "\n\nIt expires in 5 minutes. If you didn't request this, ignore this email.";

                    smtp.EnableSsl = true;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(user, pass);
                    smtp.Timeout = 20000;
                    smtp.Send(msg);
                }
            }
            catch (SmtpException ex)
            {
                // Show the real reason instead of "Failure sending mail."
                string detail = ex.Message;
                Exception inner = ex.InnerException;
                while (inner != null)
                {
                    detail += "\n" + inner.Message;
                    inner = inner.InnerException;
                }
                throw new InvalidOperationException(detail);
            }
        }

        public static string MaskEmail(string email)
        {
            int at = email.IndexOf('@');
            if (at < 2) return email;
            return email.Substring(0, 2) + new string('*', at - 2) + email.Substring(at);
        }
    }
}