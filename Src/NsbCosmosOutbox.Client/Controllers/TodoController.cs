using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NsbCosmosOutbox.Shared.Commands;
using NServiceBus;

namespace NsbCosmosOutbox.Client.Controllers
{
    [Route("api/v1/todos")]
    public class TodoController : ControllerBase
    {
        private readonly IMessageSession _session;

        public TodoController(IMessageSession session)
        {
            _session = session;
        }

        [HttpPost]
        public async Task<ActionResult> AddTodo([FromBody] ViewModel viewModel)
        {
            var message = new AddTodo
            {
                TodoId = Guid.NewGuid(),
                Task = viewModel.Task
            };
            await _session.Send(message);
            return Accepted();
        }

        public class ViewModel
        {
            [Required] public string Task { get; set; }
        }
    }
}