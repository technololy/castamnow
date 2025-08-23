using AutoMapper;
using CastAmNow.Core.Models;
namespace CastAmNow.Defect.API.Installers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Core.Dtos.Defect.CreateDefectDto, Domain.Defect.Defect>().ReverseMap();
            CreateMap<Core.Dtos.Defect.AttachmentDto, Domain.Defect.Attachment>().ReverseMap();
            CreateMap<Core.Dtos.Defect.UpdateDefectDto, Domain.Defect.Defect>().ReverseMap();
            CreateMap<Domain.Defect.Defect, Core.Dtos.Defect.DefectDto>().ReverseMap();
            CreateMap<PaginationQuery, PaginationFilter>().ReverseMap();
        }
    }
}
