namespace MVC.Services.Account.Transfer
{
    public class CreateAccountResponse : BaseResponse
    {
        public int UserId { get; set; }

        public string ActivateAccountToken { get; set; }
    }
}
