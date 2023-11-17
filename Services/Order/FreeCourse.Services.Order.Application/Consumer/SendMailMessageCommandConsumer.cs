using FreeCourse.Shared.Messages;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using FreeCourse.Services.Order.Domain.OrderAggregate;
using Microsoft.Extensions.Configuration;

namespace FreeCourse.Services.Order.Application.Consumer
{
    public class SendMailMessageCommandConsumer : IConsumer<SendMailMessageCommand>
    {
        public async Task Consume(ConsumeContext<SendMailMessageCommand> context)
        {
            string smtpHost = "smtp.gmail.com";
            int smtpPort = 587;
            string smtpUsername = "didem88.dd@gmail.com";
            string smtpPassword = "klmn1234";
            bool enableSsl = true;

            using (var smtpClient = new SmtpClient(smtpHost, smtpPort))
            {
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                smtpClient.EnableSsl = enableSsl;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

                var mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(smtpUsername);
                mailMessage.To.Add("didem88.dd@gmail.com");
                mailMessage.Subject = "test";
                mailMessage.Body = "test";
                mailMessage.IsBodyHtml = true;
               

                try
                {
                    smtpClient.Send(mailMessage);
                   

                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }


        }
    }
}
