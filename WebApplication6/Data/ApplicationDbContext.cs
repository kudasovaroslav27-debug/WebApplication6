using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using WebApplication6.Entities;
using WebApplication6.SomeInterface;

namespace WebApplication6.Data
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext 
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; } 

        public DbSet<UserSetting> UserSettings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasMany(u => u.UserSettings)
                .WithOne(us => us.User)
                .HasForeignKey(us => us.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        /// <summary>
        /// Асинхронно сохраняет все изменения, сделанные в этом контексте, в базу данных.
        /// Явная реализация метода из IApplicationDbContext.
        /// </summary>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Асинхронно начинает новую транзакцию в базе данных.
        /// Явная реализация метода из IApplicationDbContext.
        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            return await Database.BeginTransactionAsync(cancellationToken);
        }

        /// <summary>
        /// Возвращает объект Entry для данной сущности.
        /// Явная реализация метода из IApplicationDbContext.
        /// </summary>
        public new EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class
        {
            return base.Entry(entity);
        }
    }
}

