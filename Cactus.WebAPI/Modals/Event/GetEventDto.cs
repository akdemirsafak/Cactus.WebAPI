namespace Cactus.WebAPI.Modals.Event
{
    public class GetEventDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public string Location { get; set; }

    }
}
