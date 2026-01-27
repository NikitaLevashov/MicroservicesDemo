using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskService.Application.Tasks.Commands
{
    public record DeleteTaskCommand : IRequest<int>
    {
        public DeleteTaskCommand(int id)
        {
            this.Id = id;
        }
        public int Id { get; init; } 
    }
}
