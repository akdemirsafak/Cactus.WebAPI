using Cactus.WebAPI.DbContexts;
using Cactus.WebAPI.Entities;
using Cactus.WebAPI.Modals.Event;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Cactus.WebAPI.Services
{
    public class EventService : IEventService
    {
        private readonly CactusDbContext _dbContext;

        public EventService(CactusDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<GetEventDto> GetAsync(string id)
        {
            var @event = await _dbContext.Events.FindAsync(id);
            if (@event == null)
            {
                return null; // not found
            }

            return @event.Adapt<GetEventDto>();
            
        }

        public async Task<List<GetEventDto>> GetAsync()
        {
            var events = await _dbContext.Events
                .AsNoTracking()
                .ProjectToType<GetEventDto>() //Sadece DTO'daki alanları çekmeye yarar.Sorguda güncelleme yapar.
                .ToListAsync();

            return events.Adapt<List<GetEventDto>>();
        }

        public async Task CreateAsync(CreateEventDto dto)
        {
            var @event= dto.Adapt<Entities.Event>();
            await _dbContext.Events.AddAsync(@event);
            await _dbContext.SaveChangesAsync();
        }
        public async Task UpdateAsync(UpdateEventDto dto)
        {
            var existEvent = await _dbContext.Events.FindAsync(dto.Id);
            existEvent.Name = dto.Name;
            existEvent.Description = dto.Description;
            existEvent.Location = dto.Location;
            existEvent.StartDate = dto.StartDate;
            await _dbContext.SaveChangesAsync();

        }

        public async Task DeleteAsync(string id)
        {
            var @event= await _dbContext.Events.FindAsync(id);
            _dbContext.Events.Remove(@event);
            await _dbContext.SaveChangesAsync();
        }
      
    }
}
