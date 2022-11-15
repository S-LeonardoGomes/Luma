using LumaEventService.Models.DTO;
using LumaEventService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LumaEventService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet("AllEvents")]
        [Authorize(Roles = "Common")]
        public IActionResult GetAllEvents()
        {
            try
            {
                string? loggedInUsername = User.Identity.Name;
                IEnumerable<ReadEventDTO> eventsList = _eventService.GetAllEvents(loggedInUsername);

                if (eventsList == null) return NoContent();

                return Ok(eventsList);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro interno do servidor: {ex.Message}");
            }
        }

        [HttpGet("NextEvents")]
        [Authorize(Roles = "Common")]
        public IActionResult GetNextEvents()
        {
            try
            {
                string? loggedInUsername = User.Identity.Name;
                IEnumerable<ReadEventDTO> eventsList = _eventService.GetNextEvents(loggedInUsername);

                if (eventsList == null) return NoContent();

                return Ok(eventsList);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro interno do servidor: {ex.Message}");
            }
        }

        [HttpGet("EventById/{id}", Name = "GetEventById")]
        [Authorize(Roles = "Common")]
        public IActionResult GetEventById([FromRoute] string id)
        {
            try
            {
                string? loggedInUsername = User.Identity.Name;
                ReadEventDTO userEvent = _eventService.GetEventById(id, loggedInUsername);

                if (userEvent == null) return NoContent();

                return Ok(userEvent);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro interno do servidor: {ex.Message}");
            }
        }

        [HttpPost("AddEvent")]
        [Authorize(Roles = "Common")]
        public IActionResult AddNewEvent([FromBody] ReadEventDTO newEvent)
        {
            try
            {
                string? loggedInUsername = User.Identity.Name;
                _eventService.AddNewEvent(newEvent, loggedInUsername);
                return CreatedAtRoute("GetEventById", routeValues: new { Id = newEvent.EventId }, value: newEvent);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro interno do servidor: {ex.Message}");
            }
        }

        [HttpPut("UpdateEvent/{id}")]
        [Authorize(Roles = "Common")]
        public IActionResult UpdateEvent([FromRoute] string id, [FromBody] ReadEventDTO userEvent)
        {
            try
            {
                string? loggedInUsername = User.Identity.Name;
                ReadEventDTO updatedEvent = _eventService.UpdateEvent(id, userEvent, loggedInUsername);
                if (updatedEvent == null) return NotFound($"Evento #{id} não encontrado!");

                return Ok(updatedEvent);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro interno do servidor: {ex.Message}");
            }
        }

        [HttpDelete("RemoveEvent/{id}")]
        [Authorize(Roles = "Common")]
        public IActionResult RemoveEvent([FromRoute] string id)
        {
            try
            {
                string? loggedInUsername = User.Identity.Name;
                ReadEventDTO removedEvent = _eventService.RemoveEvent(id, loggedInUsername);
                if (removedEvent == null) return NotFound($"Evento #{id} não encontrado!");

                return Ok(removedEvent);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro interno do servidor: {ex.Message}");
            }
        }
    }
}
