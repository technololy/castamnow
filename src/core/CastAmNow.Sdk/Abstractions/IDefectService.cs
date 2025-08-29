using CastAmNow.Core.Dtos.Defect;
using CastAmNow.Core.Models;
using Refit;

namespace CastAmNow.Sdk.Abstractions
{
    public interface IDefectService
    {
        [Get("/api/Defect")]
        Task<ApiResponse<Response<IEnumerable<DefectDto>>>> GetDefectAsync(
            [Query] Query<long>? searchQuery = default,
            [Query] PaginationQuery? paginationQuery = default);

        [Post("/api/Defect")]
        Task<ApiResponse<Response<DefectDto>>> CreateDefectAsync(
            [Body] CreateDefectDto ameboDto);

        [Put("/api/Defect/{id}")]
        Task<ApiResponse<Response<DefectDto>>> UpdateDefectAsync(
            long id,
            [Body] DefectDto ameboDto);

        [Delete("/api/Defect/{id}")]
        Task<ApiResponse<Response<DefectDto>>> DeleteDefectAsync(long id);
    }
}
