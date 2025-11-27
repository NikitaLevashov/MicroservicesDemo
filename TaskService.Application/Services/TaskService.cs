//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using TaskService.Application.Interfaces;

//namespace TaskService.Application.Services
//{
//    public class TaskServiceApp : ITaskService
//    {
//        private readonly TaskDbContext _context;

//        public TaskServiceApp(TaskDbContext context)
//        {
//            _context = context;
//        }

//        public async Task<List<TaskEntity>> GetAllAsync() => await _context.Tasks.ToListAsync();

//        public async Task<TaskEntity?> GetByIdAsync(int id) => await _context.Tasks.FindAsync(id);

//        public async Task<TaskEntity> CreateAsync(TaskEntity task)
//        {
//            task.CreatedAt = DateTime.UtcNow;
//            _context.Tasks.Add(task);
//            await _context.SaveChangesAsync();
//            return task;
//        }

//        public async Task UpdateAsync(TaskEntity task)
//        {
//            _context.Tasks.Update(task);
//            await _context.SaveChangesAsync();
//        }

//        public async Task DeleteAsync(int id)
//        {
//            var task = await _context.Tasks.FindAsync(id);
//            if (task != null)
//            {
//                _context.Tasks.Remove(task);
//                await _context.SaveChangesAsync();
//            }
//        }
//    }
//}
