using CastAmNow.Core.Abstractions;

namespace CastAmNow.UI.Services;

public class LocalStorageService(Blazored.LocalStorage.ILocalStorageService localStorageService)
    : ILocalStorageService
{
    public async Task<string?> GetItemAsStringAsync(string key)
    {
        var token = await localStorageService.GetItemAsStringAsync(key);

        return token;
    }
}