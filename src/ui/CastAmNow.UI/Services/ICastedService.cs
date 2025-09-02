using CastAmNow.Core.Dtos.Defect;
using CastAmNow.Core.Models;

namespace CastAmNow.UI.Services;

public interface ICastedService
{
    public Task<IEnumerable<DefectDto>> SearchCastedDefectsAsync(string searchTerm);

    public Task<bool> SubmitCastedDefectsAsync(CreateDefectDto createDefectDto);

    public Task<IEnumerable<DefectDto>> GetCastedDefectsAsync(PaginationQuery? paginationQuery = null);
}