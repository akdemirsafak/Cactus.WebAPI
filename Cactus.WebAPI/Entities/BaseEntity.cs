namespace Cactus.WebAPI.Entities
{
    public abstract class BaseEntity
    {
        public BaseEntity()
        {
            Id= Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
