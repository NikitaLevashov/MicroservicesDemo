using Microsoft.EntityFrameworkCore;
using TaskService.Application.Interfaces;
using TaskService.Domain;
using TaskService.Infrastructure;

namespace TaskService.Infrastructure
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskDbContext _context;

        public TaskRepository(TaskDbContext context)
        {
            _context = context;
        }

        public async Task<List<TaskEntity>> GetAllAsync() => await _context.Tasks.ToListAsync();

        public async Task<TaskEntity?> GetByIdAsync(int id) => await _context.Tasks.FindAsync(id);

        public async Task<TaskEntity> CreateAsync(TaskEntity task, CancellationToken ctx)
        {
            task.CreatedAt = DateTime.UtcNow;
            await _context.Tasks.AddAsync(task, ctx);
            await _context.SaveChangesAsync(ctx);
            return task;
        }

        public async Task UpdateAsync(TaskEntity task)
        {
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task != null)
            {
                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();
            }
        }
    }
}
