namespace MVC.Services.Caching
{
    using System;
    using Core.Exceptions;
    using Transfer;

    public class NullCacheService : ICacheService
    {
        public GetValueResponse<T> GetValue<T>(string key)
        {
            return new GetValueResponse<T>
            {
                Status = StatusCode.NotFound
            };
        }

        public SaveValueResponse SaveValue(string key, object value, TimeSpan? duration = default(TimeSpan?))
        {
            return new SaveValueResponse
            {
                Status = StatusCode.OK
            };
        }

        public DeleteValueResponse DeleteValue(string key)
        {
            return new DeleteValueResponse
            {
                Status = StatusCode.OK
            };
        }
    }
}
