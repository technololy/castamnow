using CastAmNow.Core.Dtos.Defect;

namespace CastAmNow.UI.Services;

public interface ICastedService
{
    public Task<DefectDto?> SubmitCastedDefectsAsync(CreateDefectDto createDefectDto);

    public Task<IEnumerable<DefectDto>> GetCastedDefectsAsync();
}