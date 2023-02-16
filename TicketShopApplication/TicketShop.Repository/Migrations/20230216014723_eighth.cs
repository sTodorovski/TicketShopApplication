using Microsoft.EntityFrameworkCore.Migrations;

namespace TicketShop.Repository.Migrations
{
    public partial class eighth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Korisnik",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Mejl",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalEmail",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalUsername",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Korisnik",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Mejl",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "NormalEmail",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "NormalUsername",
                table: "AspNetUsers");
        }
    }
}
