namespace MVC.WebUI.Controllers
{
    using System.Web.Mvc;
    using Config = Core.Configuration;
    using Services.Message;
    using Models.Shared;
    using Services.Message.Transfer;
    using System.Collections.Generic;
    using System.Net.Mail;

    public class ContactController : Controller
    {
        private readonly IMessageService messageService;

        public ContactController(IMessageService messageService)
        {
            this.messageService = messageService;
        }

        // GET: /contact
        public ViewResult Index()
        {
            return this.View();
        }

        // GET: /contact/email
        [ChildActionOnly]
        public PartialViewResult Email()
        {
            return this.PartialView();
        }

        // POST: /contact/email
        [ChildActionOnly, HttpPost, ValidateAntiForgeryToken]
        public PartialViewResult Email(EmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                var fullName = $"{model.FirstName} {model.LastName}".Trim();

                var message = new MessageRequest(Config.Email.TechnicalSupport)
                {
                    FromName = fullName,
                    FromAddress = model.Email,
                    Subject = model.Subject,
                    Message = model.Message
                };

                this.messageService.SendMessage(message);

                // send copy to user
                message = new MessageRequest(model.Email, fullName)
                {
                    FromAddress = "donotreply@" + Config.Website.DomainName,
                    Subject = $"Copy of message sent to {Config.Website.DomainName}: {message.Subject}"
                };

                this.messageService.SendMessage(message);

                return this.PartialView("email-confirm");
            }

            return this.PartialView();
        }
    }
}