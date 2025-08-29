namespace CastAmNow.Core.Dtos.Defect
{
    public class DefectDto
    {
        public long Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public CategoryDto Category { get; set; }
        public SeverityDto Severity { get; set; }
        public PriorityDto Priority { get; set; }
        public StatusDto Status { get; set; } = StatusDto.Open;
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        public string? Location { get; set; }
        public virtual ICollection<AttachmentDto> Attachments { get; set; } = [];
        public DateTimeOffset CreatedAt { get;}
        public DateTimeOffset UpdatedAt { get;}
    }
}
