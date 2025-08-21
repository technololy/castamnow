namespace CastAmNow.Domain
{
    public abstract class BaseEntity<T>
    {
        public T? Id { get; set; }
        public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
        public bool IsDeleted { get; set; } = default;
    }
}
