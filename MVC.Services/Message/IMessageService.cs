namespace MVC.Services.Message
{
    using Transfer;

    public interface IMessageService
    {
        MessageResponse SendMessage(MessageRequest message);
    }
}
