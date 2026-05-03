using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskService.Domain;

namespace TaskService.Application.Interfaces
{
    public interface ITaskRepository
    {
        Task<List<TaskEntity>> GetAllAsync();
        Task<TaskEntity?> GetByIdAsync(int id);
        Task<TaskEntity> CreateAsync(TaskEntity task, CancellationToken ctx);
        Task UpdateAsync(TaskEntity task);
        Task DeleteAsync(int id);
    }
}
