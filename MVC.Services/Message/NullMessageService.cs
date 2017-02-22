namespace MVC.Services.Message
{
    using Transfer;

    public class NullMessageService : IMessageService
    {
        public MessageResponse SendMessage(MessageRequest message)
        {
            return new MessageResponse
            {
                Success = true
            };
        }
    }
}
