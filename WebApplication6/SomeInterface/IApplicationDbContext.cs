using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using WebApplication6.Entities;

namespace WebApplication6.SomeInterface
{
    /// <summary>
    /// Интерфейс для контекста базы данных приложения.
    /// </summary>
    public interface IApplicationDbContext
    {
        /// <summary>
        /// Коллекция сущностей пользователей.
        /// </summary>
        DbSet<User> Users { get; set; }

        /// <summary>
        /// Коллекция сущностей настроек пользователя.
        /// </summary>
        DbSet<UserSetting> UserSettings { get; set; }
       
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Асинхронно начинает новую транзакцию в базе данных.
        /// </summary>
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Возвращает объект Entry для данной сущности,
        /// который позволяет получать доступ к информации о сущности и ее состоянии.
        /// </summary>
        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
    }
}
