using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskService.Application.Interfaces;
using TaskService.Application.Tasks.Commands;
using TaskService.Domain;

namespace TaskService.Application.Tasks.Handlers
{
    public class CreateTaskHandler : IRequestHandler<CreateTaskCommand, TaskEntity>
    {
        private readonly ITaskRepository _taskRepository;
        public CreateTaskHandler(ITaskRepository taskRepository)
        {
                _taskRepository = taskRepository;
        }
        public async Task<TaskEntity> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            var taskEntity = new TaskEntity
            {
                Status = request.Status,
                ClientId = request.ClientId,
                Description = request.Description,
                CreatedAt = DateTime.UtcNow,
                Title = request.Title,
                UpdatedAt = DateTime.UtcNow,
            };

            var task = await _taskRepository.CreateAsync(taskEntity, cancellationToken);

            return task;
        }
    }
}
