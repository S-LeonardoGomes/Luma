using LumaEventService.Models.DTO;
using LumaEventService.Services;
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
        public IActionResult GetAllEvents()
        {
            try
            {
                IEnumerable<ReadEventDTO> eventsList = _eventService.GetAllEvents();

                if (eventsList == null) return NoContent();

                return Ok(eventsList);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro interno do servidor: {ex.Message}");
            }
        }

        [HttpGet("NextEvents")]
        public IActionResult GetNextEvents()
        {
            try
            {
                IEnumerable<ReadEventDTO> eventsList = _eventService.GetNextEvents();

                if (eventsList == null) return NoContent();

                return Ok(eventsList);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro interno do servidor: {ex.Message}");
            }
        }

        [HttpGet("EventById/{id}", Name = "GetEventById")]
        public IActionResult GetEventById([FromRoute] string id)
        {
            try
            {
                ReadEventDTO userEvent = _eventService.GetEventById(id);

                if (userEvent == null) return NoContent();

                return Ok(userEvent);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro interno do servidor: {ex.Message}");
            }
        }

        [HttpPost("AddEvent")]
        public IActionResult AddNewEvent([FromBody] ReadEventDTO newEvent)
        {
            try
            {
                _eventService.AddNewEvent(newEvent);
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
        public IActionResult UpdateEvent([FromRoute] string id, [FromBody] ReadEventDTO userEvent)
        {
            try
            {
                ReadEventDTO updatedEvent = _eventService.UpdateEvent(id, userEvent);
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
        public IActionResult RemoveEvent([FromRoute] string id)
        {
            try
            {
                ReadEventDTO removedEvent = _eventService.RemoveEvent(id);
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
