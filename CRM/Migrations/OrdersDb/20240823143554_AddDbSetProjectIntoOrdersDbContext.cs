using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CRMSystem.Migrations.OrdersDb
{
    /// <inheritdoc />
    public partial class AddDbSetProjectIntoOrdersDbContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Photo = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "Description", "Name", "Photo" },
                values: new object[,]
                {
                    { new Guid("5cae337b-74a8-4519-8af0-040a441e4258"), "Описание проекта - Обработка кредитных заявок", "Обработка кредитных заявок", "card1.png" },
                    { new Guid("892aaeb2-ef85-48cd-8bb5-d61e9ff7d6a8"), "Описание проекта - Управление кадрами", "Управление кадрами", "card2.png" },
                    { new Guid("9ee5fb6d-1c64-4d96-9be1-4675379eca0a"), "Описание проекта - Доработка CRM", "Доработка CRM", "card3.png" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Projects");
        }
    }
}
