using Microsoft.EntityFrameworkCore;
using WebApplication6.Entities;

namespace WebApplication6
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

     
        public DbSet<User> Users { get; set; }

        public DbSet<UserSetting> UserSettings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Настройка уникального индекса для Email и Username
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            // Настройка связи User и UserSetting
            modelBuilder.Entity<User>()
                .HasMany(u => u.UserSettings)      // У пользователя много настроек
                .WithOne(us => us.User)            // Настройка принадлежит одному пользователю
                .HasForeignKey(us => us.UserId)    // Внешний ключ в UserSetting
                .OnDelete(DeleteBehavior.Cascade); // При удалении пользователя, удалить все его настройки
        }
    }
}

