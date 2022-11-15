using AutoMapper;
using LumaEventService.Data;
using LumaEventService.Models;
using LumaEventService.Models.DTO;
using System.Text.Json;

namespace LumaEventService.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;

        public EventService(IEventRepository eventRepository, IMapper mapper)
        {
            _eventRepository = eventRepository;
            _mapper = mapper;
        }

        public IEnumerable<ReadEventDTO> GetAllEvents(string? loggedInUsername)
        {
            try
            {
                IEnumerable<Event> eventsList = _eventRepository.GetAllEvents(loggedInUsername);
                return _mapper.Map<IEnumerable<ReadEventDTO>>(eventsList);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<ReadEventDTO> GetNextEvents(string? loggedInUsername)
        {
            try
            {
                IEnumerable<Event> eventsList = _eventRepository.GetNextEvents(loggedInUsername);
                return _mapper.Map<IEnumerable<ReadEventDTO>>(eventsList);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ReadEventDTO GetEventById(string eventId, string? loggedInUsername)
        {
            try
            {
                Event userEvent = _eventRepository.GetEventById(eventId, loggedInUsername);
                return _mapper.Map<ReadEventDTO>(userEvent);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddNewEvent(ReadEventDTO userEvent, string? loggedInUsername)
        {
            try
            {
                if (string.IsNullOrEmpty(userEvent.Title.Trim())) userEvent.Title = "Sem título";

                Event utcUserEvent = _mapper.Map<Event>(userEvent);
                utcUserEvent.UserName = loggedInUsername;

                ValidateEventStartDate(utcUserEvent.EventUtcDateStart);
                ValidateEventEndDate(utcUserEvent.EventUtcDateStart, utcUserEvent.EventUtcDateEnd);
                ValidateConcurrentEvent(utcUserEvent, loggedInUsername);

                _eventRepository.AddNewEvent(utcUserEvent);
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ReadEventDTO UpdateEvent(string eventId, ReadEventDTO userEvent, string? loggedInUsername)
        {
            try
            {
                ValidateEventStartDate(userEvent.EventLocalDateStart.ToUniversalTime());
                ValidateEventEndDate(userEvent.EventLocalDateStart, userEvent.EventLocalDateEnd);

                if (GetEventById(eventId, loggedInUsername) == null)
                    return null;

                Event updatedEvent = _mapper.Map<Event>(userEvent);
                updatedEvent.EventId = eventId;
                updatedEvent.UserName = loggedInUsername;

                _eventRepository.UpdateEvent(updatedEvent);
                return userEvent;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ReadEventDTO RemoveEvent(string eventId, string? loggedInUsername)
        {
            try
            {
                ReadEventDTO userEvent = GetEventById(eventId, loggedInUsername);

                if (userEvent != null)
                    _eventRepository.RemoveEvent(_mapper.Map<Event>(userEvent));

                return userEvent;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void ValidateEventStartDate(DateTime eventUtcStartDate)
        {
            if (DateTime.UtcNow > eventUtcStartDate)
                throw new ArgumentException("O evento não pode ser anterior a data presente!");
        }

        private static void ValidateEventEndDate(DateTime eventLocalDateStart, DateTime eventLocalDateEnd)
        {
            if (eventLocalDateStart > eventLocalDateEnd)
                throw new ArgumentException("Data fim não pode ser anterior a data de início!");
        }

        private void ValidateConcurrentEvent(Event utcUserEvent, string? loggedInUsername)
        {
            Event concurrentEvent = _eventRepository.GetEventByDateAndUser(utcUserEvent, loggedInUsername);

            if (concurrentEvent != null)
                throw new ArgumentException(JsonSerializer.Serialize(new { Message = "Já existe um evento cadastrado nesta data!", Evento = _mapper.Map<ReadEventDTO>(concurrentEvent) }));
        }
    }
}
