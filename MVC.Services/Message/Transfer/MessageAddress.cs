namespace MVC.Services.Message.Transfer
{
    public class MessageAddress
    {
        public MessageAddress(string address)
        {
            this.Address = address;
        }

        public MessageAddress(string address, string displayName) : this(address)
        {
            this.DisplayName = DisplayName;
        }

        public string Address { get; }

        public string DisplayName { get; }
    }
}
