
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskService.Application.Tasks.Commands;
using TaskService.Application.Tasks.Queries;

namespace TaskService.Api.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    public class TaskController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TaskController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Создать новую задачу
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskCommand command, CancellationToken ctx)
        {
            var id = await _mediator.Send(command, ctx);
            return Ok(new { TaskId = id });
        }

        /// <summary>
        /// Получить все задачи
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllTasks()
        {
            var tasks = await _mediator.Send(new GetTasksQuery());
            return Ok(tasks);
        }

        /// <summary>
        /// Получить задачу по Id
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            var task = await _mediator.Send(new GetTaskByIdQuery(id));
            if (task == null)
                return NotFound();
            return Ok(task);
        }

        /// <summary>
        /// Обновить задачу
        /// </summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] UpdateTaskCommand command)
        {
            //if (id != command.Id)
            //    return BadRequest("ID mismatch");

            var result = await _mediator.Send(command);
            return result is not null ? Ok() : NotFound();
        }

        /// <summary>
        /// Удалить задачу
        /// </summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var result = await _mediator.Send(new DeleteTaskCommand(id));
            return result is not 0 ? Ok() : NotFound();
        }
    }
}
