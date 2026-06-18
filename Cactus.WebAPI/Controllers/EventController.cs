using Cactus.WebAPI.Modals.Event;
using Cactus.WebAPI.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Cactus.WebAPI.Controllers
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

        // GET: api/<EventController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _eventService.GetAsync());
        }

        // GET api/<EventController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            return Ok(await _eventService.GetAsync(id));
        
        }

        // POST api/<EventController>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]CreateEventDto createEventDto)
        {
            await _eventService.CreateAsync(createEventDto);
            return Ok();
        
        }

        // PUT api/<EventController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateEventDto updateEventDto)
        {
            await _eventService.UpdateAsync(updateEventDto);
            return Ok();
        }

        // DELETE api/<EventController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id) 
        {
            await _eventService.DeleteAsync(id);
            return Ok();
        }
    }
}
