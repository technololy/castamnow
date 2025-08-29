using CastAmNow.Core.Dtos.Defect;
using CastAmNow.Sdk;

namespace CastAmNow.UI.Services;

public class CastedService(IDefectApi api) : ICastedService
{
    public async Task<DefectDto> SubmitCastedDefectsAsync(CreateDefectDto createDefectDto)
    {
        var response = await api.DefectService.CreateDefectAsync(createDefectDto);
        return response.Content?.Data ?? new DefectDto();
    }

    public async Task<IEnumerable<DefectDto>> GetCastedDefectsAsync()
    {
        var response2 = await api.DefectService.GetDefectAsync();
        return response2.Content?.Data ?? [];
    }
}

public interface ICastedService
{
    public Task<DefectDto> SubmitCastedDefectsAsync(CreateDefectDto createDefectDto);

    public Task<IEnumerable<DefectDto>> GetCastedDefectsAsync();
}