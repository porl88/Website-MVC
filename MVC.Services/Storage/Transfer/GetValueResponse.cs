namespace MVC.Services.Storage.Transfer
{
    public class GetValueResponse<T> : BaseResponse
    {
        public T Value { get; set; }
    }
}
