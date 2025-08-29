using CastAmNow.Core.Dtos.Defect;

namespace CastAmNow.Core.Models
{
    public class DefectQuery : Query<long>
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public CategoryDto Category { get; set; } = CategoryDto.None;
        public SeverityDto Severity { get; set; } = SeverityDto.None;
        public PriorityDto Priority { get; set; } = PriorityDto.None;
        public StatusDto Status { get; set; } = StatusDto.None;
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        public string? Location { get; set; }
    }
}
