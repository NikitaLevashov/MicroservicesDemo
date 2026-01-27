namespace TaskService.Api.Models
{
    public record TaskDto(int Id, string Title, string Description, string Status, int ClientId, DateTime CreatedAt, DateTime? UpdatedAt);
    public record CreateTaskDto(string Title, string Description, string Status, int ClientId);
    public record UpdateTaskDto(string Title, string Description, string Status, int ClientId);
}
