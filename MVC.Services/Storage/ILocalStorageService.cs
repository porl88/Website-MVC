namespace MVC.Services.Storage
{
    using System;
    using Transfer;

    public interface ILocalStorageService
    {
        GetValueResponse<T> GetValue<T>(string key);

        SaveValueResponse SaveValue(string key, object value, DateTime? expiryDate = null);

        DeleteValueResponse DeleteValue(string key);

        DeleteAllValuesResponse DeleteAllValues();
    }
}
