namespace CRMSystem.Models
{
    public record Blog(Guid Id, string? Name, string? Description, string? Photo, DateTime CreateAt) { }
}