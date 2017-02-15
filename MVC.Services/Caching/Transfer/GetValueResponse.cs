namespace MVC.Services.Caching.Transfer
{
    public class GetValueResponse<T> : BaseResponse
    {
        public T Value { get; set; }
    }
}
