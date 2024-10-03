using System;

namespace CRMClientApp.Models
{
    public record Project(Guid Id, string? Name, string? Description, string? Photo) { };
}
