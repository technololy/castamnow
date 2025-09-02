using CastAmNow.Core.Dtos.Defect;
using CastAmNow.Core.Models;
using CastAmNow.Sdk;
using CastAmNow.Sdk.Abstractions;
using Microsoft.Extensions.Logging;

namespace CastAmNow.UI.Services;

public class CastedService(
    IDefectApi api,
    ILogger<CastedService> logger) : ICastedService
{
    public async Task<bool> SubmitCastedDefectsAsync(CreateDefectDto createDefectDto)
    {
        var response = await api.DefectService.CreateDefectAsync(createDefectDto);
        logger.LogInformation("response from api:{statusCode} is {message}. the route is {route}",
            response.StatusCode,
            response.Content?.Message,
            response.RequestMessage?.RequestUri?.ToString());
        return response.IsSuccessStatusCode;
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
        var response = await api.DefectService.GetDefectAsync(new DefectQuery()
        {
            Title = searchTerm,
            Description = searchTerm,
        });
        logger.LogInformation("response from api:{statusCode} is {message}. the route is {route}",
            response.StatusCode,
            response.Content?.Message,
            response.RequestMessage?.RequestUri?.ToString());
        if (!response.IsSuccessStatusCode)
        {
            logger.LogWarning("Failed to search defects with term {searchTerm}. Status code: {statusCode}", searchTerm, response.StatusCode);
            return [];
        }
        if (response.Content?.Data is null || !response.Content.Data.Any())
        {
            logger.LogInformation("No defects found matching the search term {searchTerm}", searchTerm);
            return [];
        }
        logger.LogInformation("Found {count} defects matching the search term {searchTerm}", response.Content.Data.Count(), searchTerm);
        return response.Content?.Data ?? [];
    }
}