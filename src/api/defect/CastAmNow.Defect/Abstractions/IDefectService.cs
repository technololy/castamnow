using CastAmNow.Core.Dtos.Defect;
using CastAmNow.Core.Models;

namespace CastAmNow.Defect.API.Abstractions
{
    public interface IDefectService
    {
        Task<bool> CreateDefect(CreateDefectDto createDefectDto);
        Task<IEnumerable<DefectDto>> GetAll(DefectQuery? defectQuery = default, PaginationFilter? paginationFilter = default);
        Task<(bool notFound, bool isUpdated)> UpdateDefect(long id, DefectDto defectDto);
        Task<(bool isNotFound, bool isSaved)> Delete(long id);
    }
}
