using NotificationService.Application.Abstraction;
using NotificationService.Application.Abstractions;
using NotificationService.Application.Interfaces;
using NotificationService.Applications.Abstractions;
using NotificationService.Domain.Notifications;

namespace NotificationService.Application.Services
{

    public class NotificationServiceApp : INotificationService
    {
        private readonly INotificationReadRepository _readRepo;
        private readonly INotificationWriteRepository _writeRepo;
        private readonly IUnitOfWork _uow;

        public NotificationServiceApp(
            INotificationReadRepository readRepo,
            INotificationWriteRepository writeRepo,
            IUnitOfWork uow)
        {
            _readRepo = readRepo;
            _writeRepo = writeRepo;
            _uow = uow;
        }

        public async Task<List<Notification>> GetAllAsync()
        {
            var list = await _readRepo.GetAllAsync(CancellationToken.None);
            return list.ToList();
        }

        public async Task<Notification?> GetByIdAsync(int id)
            => await _readRepo.GetByIdAsync(id, CancellationToken.None);

        public async Task<Notification> CreateAsync(Notification notification)
        {
            // агрегат уже должен быть создан через доменный ctor
            await _writeRepo.AddAsync(notification, CancellationToken.None);
            await _uow.SaveChangesAsync(CancellationToken.None);
            return notification;
        }

        public async Task UpdateAsync(Notification notification)
        {
            // EF отслеживает, поэтому SaveChanges достаточно
            await _uow.SaveChangesAsync(CancellationToken.None);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _writeRepo.FindAsync(id, CancellationToken.None);
            if (entity is null) return;

            // Можно добавить доменное событие "Deleted"
            // но удаления уведомлений обычно нет — лог хранится

            // физическое удаление:
            // _context.Remove(entity); НЕ НУЖНО — делаем через UoW
            // Для этого нужен метод Remove в репозитории — по желанию

            throw new NotSupportedException("Deleting notifications is not recommended in CQRS/DDD");
        }
    }

}
