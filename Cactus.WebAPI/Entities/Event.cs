namespace Cactus.WebAPI.Entities
{
    public class Event: BaseEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public string Location { get; set; }

    }
}
