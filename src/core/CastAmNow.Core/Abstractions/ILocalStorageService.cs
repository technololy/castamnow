namespace CastAmNow.Core.Abstractions;

public interface ILocalStorageService
{ 
    public Task<string?> GetItemAsStringAsync(string key);
}