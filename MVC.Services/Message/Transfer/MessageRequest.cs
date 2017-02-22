namespace MVC.Services.Message.Transfer
{
    using System;
    using System.Collections.Generic;

    public class MessageRequest
    {
        public MessageRequest()
        {
            this.ToAddresses = new List<MessageAddress>();
        }

        public MessageRequest(string toAddress, string toName = null) : this()
        {
            this.AddToAddress(toAddress, toName);
        }

        public List<MessageAddress> ToAddresses { get; private set; }

        public string SenderName { get; set; }

        public string FromAddress { get; set; }

        public string FromName { get; set; }

        public string Subject { get; set; }

        public string Message { get; set; }

        public Uri MessageUrl { get; set; }

        public bool IsHtml { get; set; }

        public void AddToAddress(string address, string name = null)
        {
            this.ToAddresses.Add(new MessageAddress(address, name));
        }
    }
}
