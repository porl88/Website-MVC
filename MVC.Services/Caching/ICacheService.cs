namespace MVC.Services.Caching
{
    using System;
    using Transfer;

    public interface ICacheService
    {
        GetValueResponse<T> GetValue<T>(string key);

        SaveValueResponse SaveValue(string key, object value, TimeSpan? duration = null);

        DeleteValueResponse DeleteValue(string key);
    }
}
