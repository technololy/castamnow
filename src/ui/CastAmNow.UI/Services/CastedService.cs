using CastAmNow.Core.Dtos.Defect;
using CastAmNow.Sdk;
using CastAmNow.Sdk.Abstractions;
using Microsoft.Extensions.Logging;

namespace CastAmNow.UI.Services;

public class CastedService(
    IDefectApi api,
    ILogger<CastedService> logger) : ICastedService
{
    public async Task<DefectDto?> SubmitCastedDefectsAsync(CreateDefectDto createDefectDto)
    {
        var response = await api.DefectService.CreateDefectAsync(createDefectDto);
        logger.LogInformation("response from api:{statusCode} is {message}. the route is {route}",
            response.StatusCode,
            response.Content?.Message,
            response.RequestMessage?.RequestUri?.ToString());
        return !response.IsSuccessStatusCode ? null : response.Content?.Data;
    }

    public async Task<IEnumerable<DefectDto>> GetCastedDefectsAsync()
    {
        var response = await api.DefectService.GetDefectAsync();
        logger.LogInformation("response from api:{statusCode} is {message}. the route is {route}",
            response.StatusCode,
            response.Content?.Message,
            response.RequestMessage?.RequestUri?.ToString());
        return response.Content?.Data ?? [];
    }
    public async Task<IEnumerable<DefectDto>> SearchCastedDefectsAsync(string searchTerm)
    {
        var response = await api.DefectService.GetDefectAsync();
        logger.LogInformation("response from api:{statusCode} is {message}. the route is {route}",
            response.StatusCode,
            response.Content?.Message,
            response.RequestMessage?.RequestUri?.ToString());
        return response.Content?.Data ?? [];
    }
}