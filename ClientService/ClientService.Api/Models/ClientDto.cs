
namespace ClientService.Api.Models
{
    public record ClientDto(int Id, string Name, string Email, string Phone, DateTime CreatedAt);
    public record CreateClientDto(string Name, string Email, string Phone);
    public record UpdateClientDto(string Name, string Email, string Phone);
}
