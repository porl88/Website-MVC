namespace MVC.Services
{
    using MVC.Core.Exceptions;

    public abstract class BaseResponse
	{
        public bool Success { get; set; }

        public string Message { get; set; }

		public StatusCode Status { get; set; }
	}
}
