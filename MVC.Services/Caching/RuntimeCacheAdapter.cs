namespace MVC.Services.Caching
{
    using System;
    using System.Runtime.Caching;
    using Core.Exceptions;
    using Transfer;

    public class RuntimeCacheAdapter : ICacheService
    {
        private ObjectCache cache;

        private readonly IExceptionHandler exceptionHandler;

        public RuntimeCacheAdapter(IExceptionHandler exceptionHandler)
        {
            this.exceptionHandler = exceptionHandler;
            this.cache = MemoryCache.Default;
        }

        public GetValueResponse<T> GetValue<T>(string key)
        {
            var response = new GetValueResponse<T>();

            try
            {
                if (!string.IsNullOrWhiteSpace(key))
                {
                    var value = (T)this.cache.Get(key);

                    if (value != null)
                    {
                        response.Value = value;
                        response.Status = StatusCode.OK;
                    }
                    else
                    {
                        response.Status = StatusCode.NotFound;
                    }
                }
                else
                {
                    response.Status = StatusCode.BadRequest;
                }
            }
            catch (Exception ex)
            {
                response.Status = StatusCode.InternalServerError;
                this.exceptionHandler.HandleException(ex);
            }

            return response;
        }

        public SaveValueResponse SaveValue(string key, object value, TimeSpan? duration = null)
        {
            var response = new SaveValueResponse();

            try
            {
                var item = new CacheItem(key, value);
                var policy = new CacheItemPolicy();

                if (duration != null)
                {
                    policy.SlidingExpiration = (TimeSpan)duration;
                };

                this.cache.Set(item , policy);
                response.Status = StatusCode.OK;
            }
            catch (ArgumentException)
            {
                response.Status = StatusCode.BadRequest;
            }
            catch (Exception ex)
            {
                response.Status = StatusCode.InternalServerError;
                this.exceptionHandler.HandleException(ex);
            }

            return response;
        }

        public DeleteValueResponse DeleteValue(string key)
        {
            var response = new DeleteValueResponse();

            try
            {
                this.cache.Remove(key);
                response.Status = StatusCode.OK;
            }
            catch (ArgumentException)
            {
                response.Status = StatusCode.BadRequest;
            }
            catch (Exception ex)
            {
                response.Status = StatusCode.InternalServerError;
                this.exceptionHandler.HandleException(ex);
            }

            return response;
        }
    }
}
