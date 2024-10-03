using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CRMSystem.Migrations.OrdersDb
{
    /// <inheritdoc />
    public partial class AddDbSetServiceAndBlogIntoOrdersDbContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Blogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Photo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Blogs",
                columns: new[] { "Id", "CreateAt", "Description", "Name", "Photo" },
                values: new object[,]
                {
                    { new Guid("01cd5849-72ae-4165-a97e-0f10d6a7dc39"), new DateTime(2024, 8, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "Описание блога 4", "Разработка проектной документации", "blog-4.png" },
                    { new Guid("2e3efdd7-e4df-48d4-9eac-f645387ddfc3"), new DateTime(2024, 8, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Описание блога 3", "Пожарная безопасность", "blog-3.png" },
                    { new Guid("741c1246-fdd9-40df-9b56-0b151b6335b1"), new DateTime(2024, 6, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Текст карточки по производственной безопасности по решения автоматизации", "Производственная безопасность", "blog-1.png" },
                    { new Guid("c0e71c35-706f-4876-8012-3e06229a729c"), new DateTime(2024, 7, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Описание блога 2", "Промышленная безопасность", "blog-2.png" }
                });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("a4369d1c-3692-44ab-ad7d-be51010a6e6c"), "Разработка мероприятий по промышленной безопасности в составе проекта", "Разработка мероприятий по ПБ" },
                    { new Guid("cb9216ae-685c-480f-a39f-0052bc7ad1e3"), "Разработка декларации промышленной безопасности", "Разработка ДПБ" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Blogs");

            migrationBuilder.DropTable(
                name: "Services");
        }
    }
}
