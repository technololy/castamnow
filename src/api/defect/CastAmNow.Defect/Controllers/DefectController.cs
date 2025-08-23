using AutoMapper;
using CastAmNow.Api.Infrastructure;
using CastAmNow.Api.Infrastructure.Extensions;
using CastAmNow.Core.Dtos.Defect;
using CastAmNow.Core.Models;
using CastAmNow.Defect.API.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace CastAmNow.Defect.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class DefectController(IDefectService defectService, IHttpContextAccessor httpContextAccessor,IMapper mapper, ILogger<DefectController> logger) : BaseController( httpContextAccessor, mapper)
    {
        [HttpGet(Name = "GetDefects")]
        public async Task<IActionResult> Get([FromQuery] DefectQuery searchQuery, [FromQuery] PaginationQuery paginationQuery)
        {
            logger.LogInformation("Fetching from defect api!");
            var paginationFilter = mapper.Map<PaginationFilter>(paginationQuery);
            var defectDto = await defectService.GetAll(searchQuery, paginationFilter);
            return Ok(new Response<IEnumerable<DefectDto>> { Data = defectDto, Error = false, Message = "Retrieved successfully" });
        }

        [HttpPost(Name = "CreateDefect")]
        public async Task<IActionResult> Post([FromBody] CreateDefectDto createDefectDto)
        {
            logger.LogInformation("Creating defect");
            createDefectDto.Location = ClientIp != null ? await ClientIp.GetLocationFromIpAsync() : string.Empty;
            var result = await defectService.CreateDefect(createDefectDto);
            if (!result)
            {
                logger.LogError("Failed to create defect");
                return BadRequest(new Response<string> { Data = null, Error = true, Message = "Failed to create defect" });
            }
            return Ok(new Response<bool> { Error = false, Message = "Defect created successfully", Data = true});
        }

        [HttpPut("{id}", Name = "UpdateDefect")]
        public async Task<IActionResult> Put(long id, [FromBody] DefectDto defectDto)
        {
            logger.LogInformation("Updating defect with id {id}", id);
            var (notFound, updated) = await defectService.UpdateDefect(id, defectDto);
            if (notFound)
            {
                return NotFound(new Response<object> { Error = true, Message = "Defect not found" });
            }

            if (updated)
            {
                return Ok(new Response<DefectDto>
                {
                    Data = defectDto,
                    Error = false,
                    Message = "Defect updated successfully"
                });
            }

            return BadRequest(new Response<DefectDto>
            {
                Data = defectDto,
                Error = true,
                Message = "Unable to update defect"
            });
        }

        [HttpDelete("{id}", Name = "DeleteDefect")]
        public async Task<IActionResult> Delete(long id)
        {
            logger.LogInformation("Deleting defect with id {id}", id);
            var (isNotFound, isSaved) = await defectService.Delete(id);
            if (isNotFound)
            {
                return NotFound(new Response<object> { Error = true, Message = "Village square not found" });
            }
            if (isSaved)
            {
                return Ok(new Response<bool>
                {
                    Data = isSaved,
                    Error = false,
                    Message = "Village square deleted successfully"
                });
            }

            return BadRequest(new Response<bool>
            {
                Data = false,
                Error = true,
                Message = "Unable to delete VillageSquare"
            });
        }
    }
}
