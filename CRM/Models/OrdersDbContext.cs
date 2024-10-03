using Microsoft.EntityFrameworkCore;
using CRMSystem.Models;
using System.Reflection.Metadata;

namespace CRMSystem.Models
{
    public class OrdersDbContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public OrdersDbContext(DbContextOptions<OrdersDbContext> options): base(options)
        {
            Database.EnsureCreated();            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>().HasData(
                new Project(Guid.Parse("5cae337b-74a8-4519-8af0-040a441e4258"),
                    "Обработка кредитных заявок",
                    "Описание проекта - Обработка кредитных заявок",
                    "card1.png"),
                new Project(Guid.Parse("892aaeb2-ef85-48cd-8bb5-d61e9ff7d6a8"),
                    "Управление кадрами",
                    "Описание проекта - Управление кадрами",
                    "card2.png"),
                new Project(Guid.Parse("9ee5fb6d-1c64-4d96-9be1-4675379eca0a"), 
                    "Доработка CRM",
                    "Описание проекта - Доработка CRM",
                    "card3.png"));

            modelBuilder.Entity<Service>().HasData(
                    new Service(Guid.Parse("cb9216ae-685c-480f-a39f-0052bc7ad1e3"), 
                    "Разработка ДПБ", 
                    "Разработка декларации промышленной безопасности"), 
                        new Service(Guid.Parse("a4369d1c-3692-44ab-ad7d-be51010a6e6c"),
                    "Разработка мероприятий по ПБ",
                    "Разработка мероприятий по промышленной безопасности в составе проекта"));

            modelBuilder.Entity<Blog>().HasData(
                        new Blog(Guid.Parse("741c1246-fdd9-40df-9b56-0b151b6335b1"),
                    "Производственная безопасность",
                    "Текст карточки по производственной безопасности по решения автоматизации", 
                    "blog-1.png",
                    new DateTime(2024, 06, 25)),
                        new Blog(Guid.Parse("c0e71c35-706f-4876-8012-3e06229a729c"),
                    "Промышленная безопасность",
                    "Описание блога 2",
                    "blog-2.png",
                    new DateTime(2024, 07, 10)),
                        new Blog(Guid.Parse("2e3efdd7-e4df-48d4-9eac-f645387ddfc3"),
                    "Пожарная безопасность",
                    "Описание блога 3", 
                    "blog-3.png",
                    new DateTime(2024, 08, 5)),
                        new Blog(Guid.Parse("01cd5849-72ae-4165-a97e-0f10d6a7dc39"),
                    "Разработка проектной документации",
                    "Описание блога 4",
                    "blog-4.png",
                    new DateTime(2024, 08, 19)));
        }
    }
}