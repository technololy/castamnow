namespace CastAmNow.Domain.Defect
{
    public class Defect : BaseEntity<long>
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public Category Category { get; set; }
        public Severity Severity { get; set; }
        public Priority Priority { get; set; }
        public Status Status { get; set; } = Status.Open;
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        public string? Location { get; set; }
        public virtual ICollection<Attachment> Attachments { get; set; } = [];
    }
}
