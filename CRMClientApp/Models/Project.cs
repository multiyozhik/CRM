using System;

namespace CRMClientApp.Models
{
    public class Project
    {
        public Guid Id { get; set; }    //если public Guid Id { get;}, то с БД приходят ID = 000.. 
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Photo { get; set; }
    }
}
