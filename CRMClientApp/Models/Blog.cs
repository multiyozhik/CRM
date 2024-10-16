using System;

namespace CRMClientApp.Models
{
    public class Blog
    {
        public Guid Id { get; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Photo { get; set; }
        public DateTime CreateAt { get; set; }
    }
}