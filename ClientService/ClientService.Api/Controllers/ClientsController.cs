
using Microsoft.AspNetCore.Mvc;
using ClientService.Api.Models;
using ClientService.Application.Interfaces;
using ClientService.Domain;

namespace ClientService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientsController : ControllerBase
    {
        private readonly IClientService _service;

        public ClientsController(IClientService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var clients = await _service.GetAllAsync();
            var result = clients.Select(c => new ClientDto(c.Id, c.Name, c.Email, c.Phone, c.CreatedAt));
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var client = await _service.GetByIdAsync(id);
            if (client == null) return NotFound();
            return Ok(new ClientDto(client.Id, client.Name, client.Email, client.Phone, client.CreatedAt));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateClientDto dto)
        {
            var client = new Client
            {
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone
            };

            var created = await _service.CreateAsync(client);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, new ClientDto(created.Id, created.Name, created.Email, created.Phone, created.CreatedAt));
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateClientDto dto)
        {
            var client = await _service.GetByIdAsync(id);
            if (client == null) return NotFound();

            client.Name = dto.Name;
            client.Email = dto.Email;
            client.Phone = dto.Phone;

            await _service.UpdateAsync(client);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
