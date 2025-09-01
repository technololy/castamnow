using CastAmNow.Core.Dtos.Defect;

namespace CastAmNow.UI.Services;

public interface ICastedService
{
    public Task<IEnumerable<DefectDto>> SearchCastedDefectsAsync(string searchTerm);

    public Task<DefectDto?> SubmitCastedDefectsAsync(CreateDefectDto createDefectDto);

    public Task<IEnumerable<DefectDto>> GetCastedDefectsAsync();
}