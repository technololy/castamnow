using Microsoft.EntityFrameworkCore;

namespace CastAmNow.Defect.Data
{
    public class DefectDbContext(DbContextOptions<DefectDbContext> options) : DbContext(options)
    {
        public DbSet<Domain.Defect.Defect> Defects => Set<Domain.Defect.Defect>();
        public DbSet<Domain.Defect.Attachment> Attachments => Set<Domain.Defect.Attachment>();
    }
}
