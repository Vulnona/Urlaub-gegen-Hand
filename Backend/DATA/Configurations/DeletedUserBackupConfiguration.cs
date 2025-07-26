using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Backend.Models;

namespace Backend.DATA.Configurations
{
    public class DeletedUserBackupConfiguration : IEntityTypeConfiguration<DeletedUserBackup>
    {
        public void Configure(EntityTypeBuilder<DeletedUserBackup> builder)
        {
            builder.ToTable("DeletedUserBackups");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.DeletedAt).IsRequired();
        }
    }
}
