using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskService.Domain;

namespace TaskService.Application.Tasks.Commands
{
    public record CreateTaskCommand(
        string Title,
        string Description,
        string Status,
        int ClientId): IRequest<TaskEntity>
    ;
}
