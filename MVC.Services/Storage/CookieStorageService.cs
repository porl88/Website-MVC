namespace MVC.Services.Storage
{
    using System;
    using System.Web;
    using System.Web.Script.Serialization;
    using Core.Exceptions;
    using Transfer;

    public class CookieStorageService : ILocalStorageService
    {
        private readonly HttpContextBase context;
        private readonly IExceptionHandler exceptionHandler;

        public CookieStorageService(HttpContextBase context, IExceptionHandler exceptionHandler)
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
                var cookie = this.GetCookie(key);
                if (cookie != null && !string.IsNullOrWhiteSpace(cookie.Value))
                {
                    try
                    {
                        var json = new JavaScriptSerializer();
                        var value =  json.Deserialize<T>(cookie.Value);
                        response.Status = StatusCode.OK;
                        response.Value = value;
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
            catch (Exception ex)
            {
                response.Status = StatusCode.InternalServerError;
                this.exceptionHandler.HandleException(ex);
            }

            return response;
        }

        public SaveValueResponse SaveValue(string key, object value, DateTime? expiryDate = null)
        {
            var response = new SaveValueResponse();
            var json = new JavaScriptSerializer();

            try
            {
                this.SaveValueToCookie(key, json.Serialize(value), expiryDate);
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
                this.DeleteCookie(key);
                response.Status = StatusCode.OK;
            }
            catch (Exception ex)
            {
                response.Status = StatusCode.InternalServerError;
                this.exceptionHandler.HandleException(ex);
            }

            return response;
        }

        public DeleteAllValuesResponse DeleteAllValues()
        {
            var response = new DeleteAllValuesResponse();

            try
            {
                foreach (HttpCookie cookie in this.context.Request.Cookies)
                {
                    this.DeleteCookie(cookie.Name);
                }

                response.Status = StatusCode.OK;
            }
            catch(Exception ex)
            {
                response.Status = StatusCode.InternalServerError;
                this.exceptionHandler.HandleException(ex);
            }

            return response;
        }

        private HttpCookie GetCookie(string cookieName)
        {
            return this.context.Request.Cookies[cookieName];
        }

        private HttpCookie CreateCookie(string cookieName, DateTime? expiryDate)
        {
            var cookie = new HttpCookie(cookieName);
            cookie.HttpOnly = true;

            if (expiryDate != null && expiryDate > DateTime.Now)
            {
                cookie.Expires = (DateTime)expiryDate;
            }

            return cookie;
        }

        private void SaveValueToCookie(string key, string value, DateTime? expiryDate)
        {
            if (!string.IsNullOrWhiteSpace(key) && !string.IsNullOrWhiteSpace(value))
            {
                var cookie = this.CreateCookie(key, expiryDate);
                cookie.Value = value;
                this.context.Response.Cookies.Add(cookie);
            }
            else
            {
                throw new ArgumentException("Missing a key and/or value.");
            }
        }

        private void DeleteCookie(string cookieName)
        {
            if (!string.IsNullOrWhiteSpace(cookieName))
            {
                var cookie = new HttpCookie(cookieName);
                cookie.Expires = DateTime.Now.AddDays(-2);
                this.context.Response.Cookies.Add(cookie);
            }
        }
    }
}
