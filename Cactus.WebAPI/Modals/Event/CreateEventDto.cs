namespace Cactus.WebAPI.Modals.Event
{
    public class CreateEventDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public string Location { get; set; }

    }
}
