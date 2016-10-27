namespace MVC.Services.Account.Transfer
{
    public class CreateAccountResponse : BaseResponse
    {
        public string ActivateAccountToken { get; set; }

        public CreateAccountStatus CreateAccountStatus { get; set; }
    }
}
