using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class productinfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", nullable: true),
                    Platform = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<string>(type: "nvarchar(150)", nullable: true),
                    TotalRating = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "NameIndex",
                table: "Products",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "PlatformIndex",
                table: "Products",
                column: "Platform");

            migrationBuilder.CreateIndex(
                name: "DateCreatedIndex",
                table: "Products",
                column: "DateCreated");

            migrationBuilder.CreateIndex(
                name: "RatingIndex",
                table: "Products",
                column: "TotalRating");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
