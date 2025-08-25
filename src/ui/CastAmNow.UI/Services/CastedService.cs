using CastAmNow.Core.Models;
using CastAmNow.Core.Services;
using CastAmNow.UI.Pages.Report;

namespace CastAmNow.UI.Services;

public class CastedService(IBackendApiService api) : ICastedService
{
    public async Task<Response<bool>> SubmitCastedDefectsAsync(Casted.HorrorSubmission horrorSubmission)
    {
        var response = await api.PostAsync<bool, Casted.HorrorSubmission>("api/CreateDefect",horrorSubmission,true);
        return response;
    }

    public async Task<List<DefectQuery>> GetCastedDefectsAsync()
    {
        var response = await api.GetAsync<List<DefectQuery>>("api/GetDefects");
        return response.Data ?? [];    
    }
}

public interface ICastedService
{
    Task<Response<bool>> SubmitCastedDefectsAsync(Casted.HorrorSubmission horrorSubmission);
    
    Task<List<DefectQuery>> GetCastedDefectsAsync();
}