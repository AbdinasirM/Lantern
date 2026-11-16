using Lantern.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Lantern.Database;

public class LanternDbContext : DbContext
{
    public DbSet<ConversationEntity> Conversations { get; set; }
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<DeviceEntity> Devices { get; set; }
    public DbSet<MessageEntity> Messages { get; set; }
    public DbSet<PairingCodeEntity> PairingCodes { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<DeviceEntity>()
        .HasOne<UserEntity>()
        .WithMany()
        .HasForeignKey(device => device.UserId);

        modelBuilder.Entity<ConversationEntity>()
        .HasOne<UserEntity>()
        .WithMany()
        .HasForeignKey(conversation => conversation.UserId);

        modelBuilder.Entity<MessageEntity>()
        .HasOne<ConversationEntity>()
        .WithMany()
        .HasForeignKey(message => message.ConversationId);

        modelBuilder.Entity<PairingCodeEntity>()
        .HasOne<UserEntity>()
        .WithMany()
        .HasForeignKey(pairing => pairing.UserId);

        //indexes
        modelBuilder.Entity<PairingCodeEntity>()
        .HasIndex(p => p.CodeHash)
        .IsUnique();
    }

    public LanternDbContext(DbContextOptions<LanternDbContext> lanternDbContextoptions) : base(lanternDbContextoptions)
    {

    }

}