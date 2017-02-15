namespace MVC.Services.Caching
{
    using System;
    using System.Web;
    using Core.Exceptions;
    using Transfer;

    public class HttpContextCacheAdapter : ICacheService
    {
        private readonly HttpContextBase context;
        private readonly IExceptionHandler exceptionHandler;

        public HttpContextCacheAdapter(HttpContextBase context, IExceptionHandler exceptionHandler)
        {
            this.context = context;
            this.exceptionHandler = exceptionHandler;
        }

        public GetValueResponse<T> GetValue<T>(string key)
        {
            var response = new GetValueResponse<T>();

            try
            {
                if (!string.IsNullOrWhiteSpace(key))
                {
                    var value = (T)this.context.Cache.Get(key);

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
                this.context.Cache.Insert(key, value);
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
                this.context.Cache.Remove(key);
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
