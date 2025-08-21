namespace CastAmNow.Domain.Defect
{
    public class Attachment : BaseEntity<long>
    {
        public long DefectId { get; set; }
        public string? FileUrl { get; set; }
        public FileType FileType { get; set; }
    }
}
