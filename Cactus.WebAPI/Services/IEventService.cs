using Cactus.WebAPI.Modals.Event;

namespace Cactus.WebAPI.Services
{
    public interface IEventService
    {
        Task CreateAsync(CreateEventDto dto);
        Task UpdateAsync(UpdateEventDto dto);
        Task DeleteAsync(string id);
        Task<GetEventDto> GetAsync(string id);
        Task<List<GetEventDto>> GetAsync();
    }
}
