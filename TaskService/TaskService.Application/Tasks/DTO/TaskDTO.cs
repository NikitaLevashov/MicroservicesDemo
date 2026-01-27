using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskService.Application.Tasks.DTO
{
    public record TaskDto(int Id, string Title, string Description, string Status, int ClientId, DateTime CreatedAt, DateTime? UpdatedAt);
    public record CreateTaskDto(string Title, string Description, string Status, int ClientId);
    public record UpdateTaskDto(string Title, string Description, string Status, int ClientId);
}
