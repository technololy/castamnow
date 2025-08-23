using CastAmNow.Api.Infrastructure;
using CastAmNow.Core.Models;
using CastAmNow.Defect.Data;
using Microsoft.EntityFrameworkCore;

namespace CastAmNow.Defect.API.Repositories
{
    public class DefectRepository(DefectDbContext context) : RepositoryBase<Domain.Defect.Defect, long, DefectDbContext>(context)
    {
        public override IEnumerable<Domain.Defect.Defect> GetAll<TId>(Query<TId>? searchQuery = null, PaginationFilter? paginationFilter = null)
        {
            var defectQuery = searchQuery as DefectQuery;
            var query = context.Set<Domain.Defect.Defect>().Include(d => d.Attachments.Where(a => !a.IsDeleted)).AsQueryable();

            query = query.Where(d => !d.IsDeleted);

            if (!string.IsNullOrEmpty(defectQuery?.Title))
            {
                query = query.Where(d =>
                    d.Title!.Contains(defectQuery.Title));
            }

            if (!string.IsNullOrEmpty(defectQuery?.Description))
            {
                query = query.Where(d =>
                   d.Description!.Contains(defectQuery.Description));
            }

            if (!string.IsNullOrEmpty(defectQuery?.Latitude))
            {
                query = query.Where(d =>
                   d.Latitude!.Contains(defectQuery.Latitude));
            }

            if (!string.IsNullOrEmpty(defectQuery?.Longitude))
            {
                query = query.Where(d =>
                   d.Longitude!.Contains(defectQuery.Longitude));
            }

            if (!string.IsNullOrEmpty(defectQuery?.Location))
            {
                query = query.Where(d =>
                   d.Location!.Contains(defectQuery.Location));
            }

            if (defectQuery?.Category != null && defectQuery?.Category != Core.Dtos.Defect.CategoryDto.None)
            {
                query = query.Where(d => d.Category == (Domain.Defect.Category)defectQuery!.Category);
            }

            if (defectQuery?.Severity != null && defectQuery?.Severity != Core.Dtos.Defect.SeverityDto.None)
            {
                query = query.Where(d => d.Severity == (Domain.Defect.Severity)defectQuery!.Severity);
            }

            if (defectQuery?.Priority != null && defectQuery?.Priority != Core.Dtos.Defect.PriorityDto.None)
            {
                query = query.Where(d => d.Priority == (Domain.Defect.Priority)defectQuery!.Priority);
            }

            if (defectQuery?.Status != null && defectQuery?.Status != Core.Dtos.Defect.StatusDto.None)
            {
                query = query.Where(d => d.Status == (Domain.Defect.Status)defectQuery!.Status);
            }

            if (paginationFilter != null)
            {
                var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
                query = query.Skip(skip).Take(paginationFilter.PageSize);
            }
            return [.. query];
        }
    }
}
