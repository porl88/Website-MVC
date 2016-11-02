namespace MVC.Core.Exceptions
{
    using System;
    using System.Data.Entity.Validation;
    using System.IO;
    using System.Net.Mail;
    using System.Reflection;
    using System.Text;
    using System.Web;
    using Configuration;

    public class EmailExceptionHandler : IExceptionHandler
    {
        private const int EmailLimit = 30;
        private static int counter;
        private static DateTime timeStamp;
        private readonly ISystemSettings systemSettings;
        private Exception exception;

        public EmailExceptionHandler(ISystemSettings systemSettings)
        {
            this.systemSettings = systemSettings;
        }

        public void HandleException
        (
            Exception ex,
            string message = null,
            [CallerMemberName] string memberName = null,
            [CallerFilePath] string filePath = null,
            [CallerLineNumber] int lineNumber = 0
        )
        {
            if (timeStamp != DateTime.Today)
            {
                counter = 0;
                timeStamp = DateTime.Today;
            }

            counter++;

            if (counter <= EmailLimit)
            {
                try
                {
                    if (this.systemSettings.IsProductionEnviroment)
                    {
                        this.exception = ex;
                        this.SendEmail(message, memberName, filePath, lineNumber);
                    }
                }
                catch
                {
                }
            }
        }

        private void SendEmail(string message, string memberName, string filePath, int lineNumber)
        {
            var domain = AppDomain.CurrentDomain.BaseDirectory;
            var from = "donotreply@" + Website.DomainName;
            var to = Email.TechnicalSupport;
            var subject = $"ERROR - {Website.DomainName}: {this.exception.Message} in {domain}, {memberName}, Line {lineNumber}";
            var body = this.CreateEmailBody(message, memberName, filePath, lineNumber);
            var mailMessage = new MailMessage(from, to, subject, body);
            var client = new SmtpClient();
            client.Send(mailMessage);
        }

        private string CreateEmailBody(string message, string memberName, string filePath, int lineNumber)
        {
            var emailFilePath = AppDomain.CurrentDomain.BaseDirectory + @"\exceptions\exception-email.html";

            if (File.Exists(emailFilePath))
            {
                var body = File.ReadAllText(emailFilePath);

                var context = HttpContext.Current;

                if (context != null)
                {
                    var request = context.Request;
                    body = body
                        .Replace("##Url##", request.Url.AbsoluteUri)
                        .Replace("##UrlReferrer##", request.UrlReferrer == null ? "None" : request.UrlReferrer.AbsoluteUri)
                        .Replace("##UserAgent##", request.UserAgent)
                        .Replace("##RequestType##", request.RequestType)
                        .Replace("##IpAddress##", request.UserHostAddress);

                    var user = context.User;
                    if (user != null)
                    {
                        body = body
                        .Replace("##UserName##", user.Identity.Name);
                    }
                }

                if (this.exception != null)
                {
                    body = body
                        .Replace("##ExceptionType##", this.exception.GetType().ToString())
                        .Replace("##Exception##", this.exception.Message)
                        .Replace("##Source##", this.exception.Source)
                        .Replace("##StackTrace##", this.exception.StackTrace)
                        .Replace("##InnerExceptions##", this.CreateInnerExceptionHtml())
                        .Replace("##Error##", this.exception.ToString());
                }

                body = body
                      .Replace("##CallerMemberName##", memberName)
                      .Replace("##CallerLineNumber##", lineNumber.ToString())
                      .Replace("##CallerFilePath##", filePath)
                      .Replace("##CustomMessage##", message)
                      .Replace("##ServerName##", Environment.MachineName);

                return body;
            }

            return string.Empty;
        }

        private string CreateInnerExceptionHtml()
        {
            var error = new StringBuilder();
            var exception = this.exception.InnerException;

            while (exception != null)
            {
                error.Append("<table>");
                error.AppendFormat("<tr><th>Message</th><td>{0}</td></tr>", exception.Message);
                error.AppendFormat("<tr><th>Stack Trace</th><td>{0}</td></tr>", exception.StackTrace);

                if (exception is DbEntityValidationException)
                {
                    error.Append("<tr><th>Entity Validation Exceptions</th><td>");

                    var entityException = (DbEntityValidationException)exception;
                    foreach (var eve in entityException.EntityValidationErrors)
                    {
                        error.AppendFormat("<p>Entity of type \"{0}\" in state \"{1}\" has the following validation errors:</p>", eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        error.Append("<ul>");
                        foreach (var validationErrors in eve.ValidationErrors)
                        {
                            error.AppendFormat("<li>Property: \"{0}\", Error: \"{1}\"</li>", validationErrors.PropertyName, validationErrors.ErrorMessage).AppendLine();
                        }
                        error.Append("</ul>");
                    }

                    error.Append("</td></tr>");
                }

                error.Append("</table>");

                exception = exception.InnerException;
            }

            return error.ToString();
        }
    }
}
