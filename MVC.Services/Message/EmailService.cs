namespace MVC.Services.Message
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Mail;
    using Core.Exceptions;
    using Transfer;

    public class EmailService : IMessageService
    {
        private readonly IExceptionHandler exceptionHandler;

        public EmailService(IExceptionHandler exceptionHandler)
        {
            this.exceptionHandler = exceptionHandler;
        }

        public MessageResponse SendMessage(MessageRequest request)
        {
            var response = new MessageResponse();

            if (this.ValidateMessageRequest(request))
            {
                try
                {
                    var message = new MailMessage();
                    request.ToAddresses.ForEach(x => message.To.Add(new MailAddress(x.Address, x.DisplayName)));
                    message.From = new MailAddress(request.FromAddress, request.FromName);
                    message.Subject = request.Subject;

                    if (request.MessageUrl != null)
                    {
                        using (var client = new WebClient())
                        {
                            message.Body = client.DownloadString(request.MessageUrl.AbsoluteUri);
                        }
                    }
                    else
                    {
                        message.Body = request.Message;
                    }

                    message.IsBodyHtml = request.IsHtml;

                    var smtp = new SmtpClient();
                    smtp.Send(message);

                    response.Success = true;
                }
                catch (Exception ex)
                {
                    this.exceptionHandler.HandleException(ex);
                    response.Message = Resources.Common.InternalServerError;
                }
            }
            else
            {
                response.Message = Resources.Common.BadRequest;
            }

            return response;
        }

        private bool ValidateMessageRequest(MessageRequest request)
        {
            return !string.IsNullOrWhiteSpace(request.Subject) && (!string.IsNullOrWhiteSpace(request.Message) || request.MessageUrl != null) && !string.IsNullOrWhiteSpace(request.FromAddress) && request.ToAddresses.Any(x => !string.IsNullOrWhiteSpace(x.Address));
        }
    }
}
