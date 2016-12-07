using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.DAL
{
    /// <summary>
    /// Simple class that just sends an email to the configured recipients when they need to be alerted
    /// </summary>
    public static class NotificationManager
    {
        #region "Methods"

        /// <summary>
        /// Sends an email message
        /// </summary>
        /// <param name="message">Message to send</param>
        public static void SendNotification(string message)
        {
            try
            {
                var smtpServerAddress = ConfigurationManager.AppSettings["smtpServerAddress"];
                var smtpServerPort = int.Parse(ConfigurationManager.AppSettings["smtpServerPort"]);
                var from = ConfigurationManager.AppSettings["from"];
                var to = ConfigurationManager.AppSettings["to"];

                using (SmtpClient smtpClient = new SmtpClient(smtpServerAddress, smtpServerPort))
                using (MailMessage mailMessage = new MailMessage(from, to))
                {
                    mailMessage.IsBodyHtml = true;
                    mailMessage.Subject = "New notification from Inventory Application!!!";
                    mailMessage.Body = String.Format("TimeStamp: {0}<br/>Message: {1}", DateTime.Now, message);
                    smtpClient.Send(mailMessage);
                }
            }
            catch
            {
                // Not dealing with any notification errors such as SMTP server not available or lack of settings, for instance
            }
        }

        #endregion "Methods"
    }
}