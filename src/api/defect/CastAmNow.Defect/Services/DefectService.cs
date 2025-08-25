using AutoMapper;
using CastAmNow.Api.Infrastructure.Abstractions;
using CastAmNow.Core.Dtos.Defect;
using CastAmNow.Core.Models;
using CastAmNow.Defect.API.Abstractions;

namespace CastAmNow.Defect.API.Services
{
    internal class DefectService(IRepository<Domain.Defect.Defect> repository, IMapper mapper, ILogger<DefectService> logger) : IDefectService
    {
        public async Task<bool> CreateDefect(CreateDefectDto createDefectDto)
        {
            var defect = mapper.Map<Domain.Defect.Defect>(createDefectDto);
            var saved = repository.Add(defect);
            var isSaved = await repository.Commit();
            return isSaved;
        }

        public async Task<(bool isNotFound, bool isSaved)> Delete(long id)
        {
            var existingEntity = repository.Get(id);
            if (existingEntity == null)
            {
                return (true, false);
            }
            // Soft delete by setting IsDeleted flag
            existingEntity.IsDeleted = true;
            repository.Update(existingEntity);

            var isSaved = await repository.Commit();
            if (isSaved)
            {
                return (false, isSaved);
            }

            return (false, false);
        }

        public Task<IEnumerable<DefectDto>> GetAll(DefectQuery? defectQuery = null, PaginationFilter? paginationFilter = null)
        {
            return Task.FromResult(mapper.Map<IEnumerable<DefectDto>>(repository.GetAll(defectQuery, paginationFilter)));
        }

        public async Task<(bool notFound, bool isUpdated)> UpdateDefect(long id, DefectDto defectDto)
        {
            var existingEntity = repository.Get(id);
            if (existingEntity == null)
            {
                return (true, false);
            }
            // Map updated values and perform update
            var updatedEntity = mapper.Map(defectDto, existingEntity);
            repository.Update(updatedEntity);

            var isSaved = await repository.Commit();
            if (isSaved)
            {
                defectDto.Id = updatedEntity.Id;
                return (false, true);
            }

            return (false, false);
        }
    }
}
