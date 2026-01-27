using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskService.Application.Tasks.DTO;

namespace TaskService.Application.Tasks.Queries
{
    public record GetTasksQuery() : IRequest<IReadOnlyList<TaskDto>>;
}
